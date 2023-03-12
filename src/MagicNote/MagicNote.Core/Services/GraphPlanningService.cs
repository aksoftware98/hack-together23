using MagicNote.Core.Interfaces;
using MagicNote.Shared;
using MagicNote.Shared.DTOs;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicNote.Core.Services
{
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
		public async Task<PlanResult> AnalyzeNoteAsync(SubmitNoteRequest note)
		{
			var items = new List<PlanItem>();
			// 1- Divide the note into sentences 
			var sentences = note.Query.Split("\r\n");
			// 2- Analyze each sentence and get the intent
			foreach (var sentence in sentences)
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
								Title = eventDescription.Text,
								Plan = entityType,
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

							var meetingPeople = new List<MeetingPerson>(); 
							var people = analyzeResult?.Prediction?.Entities?.Where(e => e.Category.Equals("Person"));
							var contactsFilter = string.Join(" or ", people.Select(p => $"contains(displayName, '{p.Text}')"));
							var userEntities = (await _graph
														.Me
														.Contacts
														.Request()
														.Select("id,displayName,emailAddresses")
														.Filter(contactsFilter)
														.GetAsync());

							foreach (var person in people)
							{
								var user = userEntities.FirstOrDefault(u => u.DisplayName.Contains(person.Text));
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
								Title = eventDescription?.Text ?? $"Meeting with {string.Join(",", people.Select(p => p.Text))}",
								Plan = entityType,
								StartTime = startTime,
								EndTime = endTime,
							};
							items.Add(planItem);
							break;
						}
					case PlanEntityType.ToDoItem:
						{
							// Get the entity
							var eventDescription = analyzeResult?.Prediction?.Entities?.FirstOrDefault(e => e.Category.Equals("Event"));
							
							var planItem = new PlanItem()
							{
								Title = eventDescription.Text,
								Plan = entityType,
							};
							items.Add(planItem);
							break;
						}
					default:
						continue;
				}
				
			}

			return new PlanResult
			{
				Items = items
			};
		}

		public Task SubmitPlanAsync(PlanResult plan)
		{
			throw new NotImplementedException();
		}
	}
}
