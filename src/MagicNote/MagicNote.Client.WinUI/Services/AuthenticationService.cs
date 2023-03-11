﻿using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicNote.Client.WinUI.Services
{
	public class AuthenticationService
	{


		private static string[] Scopes = new string[] { "user.read", "Calendars.ReadWrite", "Tasks.ReadWrite", "Contacts.ReadWrite" };
		private const string ClientId = "ec137e7d-ceb1-453a-bb59-65dc4be40822";
		private const string Tenant = "common";
		private const string Authority = "https://login.microsoftonline.com/" + Tenant;

		// The MSAL Public client app
		private static IPublicClientApplication PublicClientApp;

		private static AuthenticationResult authResult;


		/// <summary>
		/// Signs in the user and obtains an access token for Microsoft Graph
		/// </summary>
		/// <param name="scopes"></param>
		/// <returns> Access Token</returns>
		public async Task<string> SignInAsync()
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
				authResult = await PublicClientApp.AcquireTokenSilent(Scopes, firstAccount)
												  .ExecuteAsync();
			}
			catch (MsalUiRequiredException ex)
			{
				Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

				authResult = await PublicClientApp.AcquireTokenInteractive(Scopes)
												  .ExecuteAsync()
												  .ConfigureAwait(false);
				
			}
			return authResult.AccessToken;
		}


		/// <summary>
		/// Signs out the current user from the application
		/// </summary>
		/// <returns></returns>
		public async Task SignOutAsync()
		{
			IEnumerable<IAccount> accounts = await PublicClientApp.GetAccountsAsync().ConfigureAwait(false);
			while (accounts.Any())
			{
				await PublicClientApp.RemoveAsync(accounts.FirstOrDefault());
				accounts = await PublicClientApp.GetAccountsAsync().ConfigureAwait(false);
			}
		}

	}
}
