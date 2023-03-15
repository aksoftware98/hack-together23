using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagicNote.Client.ViewModels.Exceptions;
using MagicNote.Client.ViewModels.Interfaces;
using MagicNote.Client.ViewModels.Models;
using MagicNote.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicNote.Client.ViewModels
{
	public partial class PlanningViewModel : BaseViewModel
	{

		private readonly IPlanningClient _planningClient;
		private readonly User _user;
		private readonly INavigationService _navigation;
		private readonly IMessageDialogService _dialogsService;
		public PlanningViewModel(IPlanningClient planningClient,
								 User user,
								 INavigationService navigation,
								 IMessageDialogService dialogsService)
		{
			_planningClient = planningClient;
			_user = user;
			_navigation = navigation;
			_dialogsService = dialogsService;
		}

		#region Proeprties 
		[ObservableProperty]
		private bool _isPlanSubmitted = false;

		[ObservableProperty]
		private string _note = string.Empty;

		[ObservableProperty]
		private PlanViewModel? _plan = null;

		#endregion

		[RelayCommand]
		private async Task AnalyzeNoteAsync()
		{
			if (string.IsNullOrWhiteSpace(Note))
				return;

			try
			{
				IsBusy = true;
				var plan = await _planningClient.AnalyzeNoteAsync(_user.AccessToken, Note);
				Plan = new(plan);

				IsPlanSubmitted = true;
			}
			catch (ApiException ex)
			{
				// TODO: Log the error
				await _dialogsService.ShowOkDialogAsync("Error", ex.Message);
			}
			catch (Exception)
			{
				// TODO: Log the error 
				await _dialogsService.ShowOkDialogAsync("Error", "An error occured while submitting the plan. Please try again later.");
			}

			IsBusy = false;
		}

		[RelayCommand]
		private async Task SubmitPlanAsync()
		{
			// TODO: Validate the plan client-side 
			if (Plan == null)
				return;
			IsBusy = true;

			var planRequest = BuildRequestObject();
			try
			{
				await _planningClient.SubmitPlanAsync(_user.AccessToken, planRequest);

				_navigation.NavigateTo("PlanSubmittedPage");
			}
			catch (ApiException ex)
			{
				// TODO: Log the error
				await _dialogsService.ShowOkDialogAsync("Error", ex.Message);
			}
			catch (Exception)
			{
				// TODO: Log the error 
				await _dialogsService.ShowOkDialogAsync("Error", "An error occured while submitting the plan. Please try again later.");
			}
			IsBusy = false;

		}

		private PlanDetails BuildRequestObject()
		{
			return new()
			{
				Items = Plan.Items.Select(p => new PlanItem
				{
					Type = p.Type,
					EndTime = p.EndTime,
					StartTime = p.StartTime,
					Title = p.Title,
					People = p.Contacts?.Select(c => new MeetingPerson()
					{
						Email = c.Email,
						Name = c.DisplayName,
						AddContact = c.AddContact,
						AddEmailToContact = c.UpdateEmail,
						Id = c.Id
					}) ?? Enumerable.Empty<MeetingPerson>()
				})
			};
		}
	}
}
