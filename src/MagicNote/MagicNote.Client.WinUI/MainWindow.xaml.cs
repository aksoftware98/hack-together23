// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
namespace MagicNote.Client.WinUI
{
	public sealed partial class MainWindow : Window
	{
		

		public MainWindow()
		{
			this.InitializeComponent();
			new MicaActivator(this);
			ExtendsContentIntoTitleBar = true;
			SetTitleBar(AppTitleBar);
		}

		//Set the scope for API call to user.read
		private string[] scopes = new string[] { "user.read" };

		// Below are the clientId (Application Id) of your app registration and the tenant information.
		// You have to replace:
		// - the content of ClientID with the Application Id for your app registration
		private const string ClientId = "ec137e7d-ceb1-453a-bb59-65dc4be40822";

		private const string Tenant = "common"; // Alternatively "[Enter your tenant, as obtained from the Azure portal, e.g. kko365.onmicrosoft.com]"
		private const string Authority = "https://login.microsoftonline.com/" + Tenant;

		// The MSAL Public client app
		private static IPublicClientApplication PublicClientApp;

		private static string MSGraphURL = "https://graph.microsoft.com/v1.0/";
		private static AuthenticationResult authResult;
		

		/// <summary>
		/// Signs in the user and obtains an access token for Microsoft Graph
		/// </summary>
		/// <param name="scopes"></param>
		/// <returns> Access Token</returns>
		private static async Task<string> SignInUserAndGetTokenUsingMSAL(string[] scopes)
		{
			// Initialize the MSAL library by building a public client application
			PublicClientApp = PublicClientApplicationBuilder.Create(ClientId)
				.WithAuthority(Authority)
				.WithRedirectUri("https://login.microsoftonline.com/common/oauth2/nativeclient")
				 .WithLogging((level, message, containsPii) =>
				 {
					 Debug.WriteLine($"MSAL: {level} {message} ");
				 }, LogLevel.Warning, enablePiiLogging: false, enableDefaultPlatformLogging: true)
				.Build();

			// It's good practice to not do work on the UI thread, so use ConfigureAwait(false) whenever possible.
			IEnumerable<IAccount> accounts = await PublicClientApp.GetAccountsAsync().ConfigureAwait(false);
			IAccount firstAccount = accounts.FirstOrDefault();

			try
			{
				authResult = await PublicClientApp.AcquireTokenSilent(scopes, firstAccount)
												  .ExecuteAsync();
			}
			catch (MsalUiRequiredException ex)
			{
				// A MsalUiRequiredException happened on AcquireTokenSilentAsync. This indicates you need to call AcquireTokenAsync to acquire a token
				Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

				authResult = await PublicClientApp.AcquireTokenInteractive(scopes)
												  .ExecuteAsync()
												  .ConfigureAwait(false);

			}
			return authResult.AccessToken;
		}

		private async void SignInButton_Click(object sender, RoutedEventArgs e)
		{
			await SignInUserAndGetTokenUsingMSAL(scopes);
		}

		private void SignOutButton_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}