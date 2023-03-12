using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagicNote.Client.ViewModels.Interfaces;
using MagicNote.Client.ViewModels.Models;
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
		public PlanningViewModel(IPlanningClient planningClient,
								 User user,
								 INavigationService navigation)
		{
			_planningClient = planningClient;
			_user = user;
			_navigation = navigation;
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

			IsBusy = true;
			var plan = await _planningClient.AnalyzeNoteAsync(_user?.AccessToken, Note);
			await Task.Delay(2500);
			Plan = new(plan);
			
			IsPlanSubmitted = true;
			IsBusy = false;
		}

		[RelayCommand]
		private async Task SubmitPlanAsync()
		{
			// TODO: Validate the plan client-side 
			IsBusy = true;

			//await _planningClient.SubmitPlanAsync(new());
			await Task.Delay(4000);
			_navigation.NavigateTo("PlanSubmittedPage");
			IsBusy = false;
		}
	}
}
