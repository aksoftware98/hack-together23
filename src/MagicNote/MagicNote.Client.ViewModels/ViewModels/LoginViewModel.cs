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
	public partial class LoginViewModel : BaseViewModel
	{

		private readonly IAuthenticationProvider _authProvider;
		private readonly INavigationService _navigation;
		public LoginViewModel(IAuthenticationProvider authProvider, 
							  INavigationService navigation)
		{
			_authProvider = authProvider;
			_navigation = navigation;
		}

		public event Action<User> OnLoginUserSuccessfully = delegate { };

		[RelayCommand]
		private async Task SignInAsync()
        {
			IsBusy = true;
			var user = await _authProvider.SignInAsync();
			OnLoginUserSuccessfully.Invoke(user);
			_navigation.NavigateTo("PlanningPage");
			IsBusy = false; 
        }

		[RelayCommand]
		private async Task SignOutAsync()
		{
			IsBusy = true; 
			await _authProvider.SignOutAsync();
			IsBusy = false; 
		}

	}
}
