// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using MagicNote.Client.ViewModels;
using MagicNote.Client.WinUI.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MagicNote.Client.WinUI.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class LoginPage : Page
	{
		private LoginViewModel _viewModel;
		public LoginPage()
		{
			this.InitializeComponent();
			DataContext = _viewModel = new LoginViewModel(new AuthenticationService(), 
														  App.NavigationService, 
														  new MessageDialogService(this));
			_viewModel.OnLoginUserSuccessfully += OnLoginUserSuccessfully;
		}

		private void OnLoginUserSuccessfully(ViewModels.Models.User user)
		{
			App.LoginUser(user);
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			_viewModel.OnLoginUserSuccessfully -= OnLoginUserSuccessfully;
		}
	}
}
