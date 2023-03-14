using MagicNote.Core.Interfaces;
using MagicNote.Shared;
using MagicNote.Shared.DTOs;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MagicNote.Core.Services
{
	// TODO: Handle the date of the customer timezone 
	public class GraphPlanningService : IPlanningService
	{

		private readonly ILanguageUnderstnadingService _language;
		private readonly GraphServiceClient _graph;

		public GraphPlanningService(ILanguageUnderstnadingService language,
									GraphServiceClient graph)
		{
			_language = language;
			_graph = graph;
		}

		/// <summary>
		/// Understand a note using AI and build a productivty plan out of it for user to review and fill the remaining data
		/// </summary>
		/// <param name="note">Note query request</param>
		/// <returns></returns>
		public async Task<PlanDetails> AnalyzeNoteAsync(SubmitNoteRequest note)
		{
			// TODO: Refactor and clean up the code
			var items = new List<PlanItem>();
			// 1- Divide the note into sentences 
			note.Query = note.Query
								.Replace(",", "\r")
								.Replace(".", "\r")
								.Replace(";", "\r");
			var sentences = note.Query.Split(new[] { "\r\n", "\r" }, StringSplitOptions.None);
			// 2- Analyze each sentence and get the intent
			foreach (var sentence in sentences.Where(s => !string.IsNullOrWhiteSpace(s)))
			{
				var analyzeResponse = await _language.AnalyzeTextAsync(sentence);
				var analyzeResult = analyzeResponse.Result;
				if (analyzeResult?.Prediction?.TopIntent?.Equals("None") ?? false)
					continue;

				var entityType = analyzeResult?.Prediction?.TopIntent switch
				{
					"AddReminder" => PlanEntityType.Event,
					"CreateMeeting" => PlanEntityType.Meeting,
					"AddToDo" => PlanEntityType.ToDoItem,
					_ => PlanEntityType.None,
				};

				// Get the top entity 
				switch (entityType)
				{
					case PlanEntityType.Event:
						{
							// Get the entity
							var eventDescription = analyzeResult?.Prediction?.Entities?.FirstOrDefault(e => e.Category.Equals("Event"));
							var eventTime = analyzeResult?.Prediction?.Entities?.FirstOrDefault(e => e.Category.Equals("Time"));
							var timeDataType = eventTime.Resolutions.FirstOrDefault();
							DateTime startTime = DateTime.Now.AddDays(1);
							if (timeDataType.DateTimeSubKind.Equals("Date"))
							{
								startTime = DateTime.ParseExact($"{note.Date:yyyy-MM-dd} {timeDataType.Value}", "yyyy-MM-dd HH:mm:ss", null);
							}
							else if (timeDataType.DateTimeSubKind.Equals("DateTime"))
							{
								startTime = DateTime.ParseExact(timeDataType.Value, "yyyy-MM-dd HH:mm:ss", null);
							}
							DateTime endTime = startTime.AddHours(1);
							var planItem = new PlanItem()
							{
								Title = CapitilizeFirstLetter(eventDescription.Text),
								Type = entityType,
								StartTime = startTime,
								EndTime = endTime,
							};
							items.Add(planItem);
							break;
						}
					case PlanEntityType.Meeting:
						{
							// Get the entity
							var eventDescription = analyzeResult?.Prediction?.Entities?.FirstOrDefault(e => e.Category.Equals("Event"));
							var eventTime = analyzeResult?.Prediction?.Entities?.FirstOrDefault(e => e.Category.Equals("Time"));
							var timeDataType = eventTime?.Resolutions?.FirstOrDefault();
							DateTime startTime = DateTime.Now.AddDays(1);
							if (eventTime != null)
							{
								if (timeDataType.DateTimeSubKind.Equals("Date"))
								{
									startTime = DateTime.ParseExact($"{note.Date:yyyy-MM-dd} {timeDataType.Value}", "yyyy-MM-dd HH:mm:ss", null);
								}
								else if (timeDataType.DateTimeSubKind.Equals("DateTime"))
								{
									startTime = DateTime.ParseExact(timeDataType.Value, "yyyy-MM-dd HH:mm:ss", null);
								}
							}
							DateTime endTime = startTime.AddHours(1);

							var meetingPeople = new List<MeetingPerson>();
							var people = analyzeResult?.Prediction?.Entities?.Where(e => e.Category.Equals("Person"));
							var contactsFilter = string.Join(" or ", people.Select(p => $"contains(displayName, '{p.Text}')"));
							var userEntities = (await _graph
														.Me
														.Contacts
														.GetAsync(config =>
														{
															config.QueryParameters.Filter = contactsFilter;
															config.QueryParameters.Select = new[] { "id", "displayName", "emailAddresses" };
														}));

							foreach (var person in people)
							{
								var user = userEntities?.Value?.FirstOrDefault(u => u.DisplayName.Contains(person.Text));
								if (user != null)
								{
									meetingPeople.Add(new MeetingPerson()
									{
										Id = user.Id,
										Email = user.EmailAddresses?.FirstOrDefault()?.Address,
										Name = user.DisplayName,
										AddContact = false,
										AddEmailToContact = false,
									});
								}
								else
									meetingPeople.Add(new MeetingPerson()
									{
										Name = person.Text,
										AddContact = true,
										AddEmailToContact = true,
									});
							}

							var planItem = new PlanItem()
							{
								Title = CapitilizeFirstLetter(eventDescription?.Text) ?? $"Meeting with {string.Join(",", people.Select(p => p.Text))}",
								Type = entityType,
								StartTime = startTime,
								EndTime = endTime,
								People = meetingPeople
							};
							items.Add(planItem);
							break;
						}
					case PlanEntityType.ToDoItem:
						{
							// Get the entity
							string title = sentence;
							var eventDescription = analyzeResult?.Prediction?.Entities?.FirstOrDefault(e => e.Category.Equals("Event"));

							if (eventDescription != null)
								title = CapitilizeFirstLetter(eventDescription.Text);

							var planItem = new PlanItem()
							{
								Title = title,
								Type = entityType,
							};
							items.Add(planItem);
							break;
						}
					default:
						continue;
				}

			}

			return new PlanDetails
			{
				Items = items
			};
		}

		public async Task SubmitPlanAsync(PlanDetails plan)
		{
			var json = JsonSerializer.Serialize(plan);
			ArgumentNullException.ThrowIfNull(plan);

			var tomorrow = DateTime.Now.AddDays(1);
			var year = tomorrow.Year;
			var month = tomorrow.Month;
			var day = tomorrow.Day;

			// Get the planned list of the to-do list
			TodoTaskListCollectionResponse? plannedListResults = null;
			try
			{
				plannedListResults  = await _graph.Me
										.Todo
										.Lists
										.GetAsync(config =>
										{
											config.QueryParameters.Filter = $"displayName eq 'Tasks'";
										});
			}
			catch (Exception ex)
			{

			}
			
			var plannedList = plannedListResults?.Value?.FirstOrDefault();

			// Build the batch requests
			var batchReqeustContent = new BatchRequestContent(_graph);

			// Check the to-do items in the request 
			var items = plan.Items.Where(i => i.Type == PlanEntityType.ToDoItem);

			if (items.Any())
			{
				foreach (var item in items)
				{
					var todoItem = new TodoTask
					{
						Title = item.Title,
						DueDateTime = new DateTimeTimeZone()
						{
							DateTime = tomorrow.ToString("yyyy-MM-ddTHH:mm:ss.ffff"),
							TimeZone = "Asia/Dubai"
						}
					};
					var todoItemRequest = _graph
											.Me
											.Todo
											.Lists[plannedList?.Id]
											.Tasks
											.ToPostRequestInformation(todoItem);
					await batchReqeustContent.AddBatchRequestStepAsync(todoItemRequest);
				}
			}

			// Check if there is meetings in the call 
			var meetings = plan.Items.Where(i => i.Type == PlanEntityType.Meeting);
			if (meetings.Any())
			{
				foreach (var item in meetings)
				{
					var calendarMeeting = new Event
					{
						Subject = item.Title,
						Start = new DateTimeTimeZone
						{
							DateTime = new DateTime(year, month, day, item.StartTime.Value.Hour, item.StartTime.Value.Minute, 0).ToString("yyyy-MM-ddTHH:mm:ss.ffff"),
							TimeZone = "Asia/Dubai"
						},
						End = new DateTimeTimeZone
						{
							DateTime = new DateTime(year, month, day, item.EndTime.Value.Hour, item.EndTime.Value.Minute, 0).ToString("yyyy-MM-ddTHH:mm:ss.ffff"),
							TimeZone = "Asia/Dubai"
						},
						IsOnlineMeeting = true,
						Attendees = item.People.Select(p => new Attendee
						{
							EmailAddress = new EmailAddress
							{
								Address = p.Email,
								Name = p.Name
							}
						}).ToList()
					};

					var calendarEventRequest = _graph
												.Me
												.Events
												.ToPostRequestInformation(calendarMeeting);

					await batchReqeustContent.AddBatchRequestStepAsync(calendarEventRequest);
				}
			}

			// Check if there is meetings in the call 
			var events = plan.Items.Where(i => i.Type == PlanEntityType.Event);
			if (events.Any())
			{
				foreach (var item in events)
				{
					var calendarEvent = new Event
					{
						Subject = item.Title,
						Start = new DateTimeTimeZone
						{
							DateTime = new DateTime(year, month, day, item.StartTime.Value.Hour, item.StartTime.Value.Minute, 0).ToString("yyyy-MM-ddTHH:mm:ss.ffff"),
							TimeZone = "Asia/Dubai"
						},
						End = new DateTimeTimeZone
						{
							DateTime = new DateTime(year, month, day, item.EndTime.Value.Hour, item.EndTime.Value.Minute, 0).ToString("yyyy-MM-ddTHH:mm:ss.ffff"),
							TimeZone = "Asia/Dubai"
						}
					};

					var calendarEventRequest = _graph
												.Me
												.Events
												.ToPostRequestInformation(calendarEvent);

					await batchReqeustContent.AddBatchRequestStepAsync(calendarEventRequest);
				}
			}

			await _graph.Batch.PostAsync(batchReqeustContent);
		}

		private string CapitilizeFirstLetter(string text)
		{
			if (text.Length == 0)
				return text;
			else if (text.Length == 1)
				return text.ToUpper();
			else
				return text.Substring(0, 1).ToUpper() + text.Substring(1);
		}
	}
}
