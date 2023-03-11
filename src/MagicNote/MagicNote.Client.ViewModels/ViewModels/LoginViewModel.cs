using CommunityToolkit.Mvvm.Input;
using MagicNote.Client.ViewModels.Interfaces;
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

        public LoginViewModel(IAuthenticationProvider authProvider)
        {
            _authProvider = authProvider; 
        }

		[RelayCommand]
		private async Task SignInAsync()
        {
			IsBusy = true;
			var user = await _authProvider.SignInAsync(); 
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
