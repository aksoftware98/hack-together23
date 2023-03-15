using MagicNote.Client.ViewModels.Interfaces;
using MagicNote.Client.ViewModels.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicNote.Client.WinUI.Services
{
	public class AuthenticationService : IAuthenticationProvider
	{


		private static string[] Scopes = new string[] { "user.read", "Calendars.ReadWrite", "Tasks.ReadWrite", "Contacts.ReadWrite", "MailboxSettings.Read" };
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
		public async Task<User> SignInAsync()
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
			catch (Exception ex)
			{
				throw;
			}
			return new User(authResult.AccessToken, authResult.ClaimsPrincipal.FindFirst("name").Value, "https://img.freepik.com/free-vector/businessman-character-avatar-isolated_24877-60111.jpg?w=2000");
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
