using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MagicNote.Client.Maui.Services
{

	public class B2CAuthenticationService
	{
		private IPublicClientApplication _pca;


		private static readonly Lazy<B2CAuthenticationService> lazy = new Lazy<B2CAuthenticationService>
		   (() => new B2CAuthenticationService());

		public static B2CAuthenticationService Instance { get { return lazy.Value; } }

		public B2CAuthenticationService()
		{
#if ANDROID
			_pca = PublicClientApplicationBuilder
				.Create(B2CConstants.ClientID)
				.WithAuthority(B2CConstants.AuthoritySignInSignUp)
				.WithRedirectUri($"msal{B2CConstants.ClientID}://auth")
				.WithParentActivityOrWindow(() => Platform.CurrentActivity)
				.Build();
#elif IOS
            _pca = PublicClientApplicationBuilder
                .Create(B2CConstants.ClientID)
                .WithAuthority(B2CConstants.AuthoritySignInSignUp)
                .WithIosKeychainSecurityGroup(B2CConstants.IOSKeyChainGroup)
                .WithRedirectUri($"msal{B2CConstants.ClientID}://auth")
                .Build();
#else
			_pca = PublicClientApplicationBuilder
				.Create(B2CConstants.ClientID)
				.WithAuthority(B2CConstants.AuthoritySignInSignUp)
				.WithRedirectUri("https://login.microsoftonline.com/common/oauth2/nativeclient")
				.Build();
#endif
		}

		public async Task<UserContext> SignInAsync()
		{
			var accounts = await _pca.GetAccountsAsync();
			AuthenticationResult result = null;

			UserContext newContext;
			try
			{
				result = await _pca
								.AcquireTokenSilent(B2CConstants.Scopes, accounts.FirstOrDefault())
								.ExecuteAsync();
				// acquire token silent
				newContext = await AcquireTokenSilent();
			}
			catch (MsalUiRequiredException)
			{
				// acquire token interactive
				return null;
			}
			catch (Exception ex)
			{
				// TODO: Catch the exception and handle it
				await Shell.Current.DisplayAlert("Error", ex.Message, "Close");
			}

			return UpdateUserInfo(result);
		}

		public async Task<UserContext> InteractiveSignInAsync()
		{
			AuthenticationResult result = null;
			try
			{
				result = await _pca
							.AcquireTokenInteractive(B2CConstants.Scopes)
							.ExecuteAsync();
			}
			catch (Exception ex)
			{
				// TODO: Catch the exception and handle it
				await Shell.Current.DisplayAlert("Error", ex.Message, "Close");
				return null;
			}

			return UpdateUserInfo(result);
		}

		private async Task<UserContext> AcquireTokenSilent()
		{
			IEnumerable<IAccount> accounts = await _pca.GetAccountsAsync();
			AuthenticationResult authResult = await _pca.AcquireTokenSilent(B2CConstants.Scopes, GetAccountByPolicy(accounts, B2CConstants.PolicySignUpSignIn))
			   .WithB2CAuthority(B2CConstants.AuthoritySignInSignUp)
			   .ExecuteAsync();

			var newContext = UpdateUserInfo(authResult);
			return newContext;
		}

		public async Task<UserContext> ResetPasswordAsync()
		{
			AuthenticationResult authResult = await _pca.AcquireTokenInteractive(B2CConstants.Scopes)
				.WithPrompt(Prompt.NoPrompt)
				.WithAuthority(B2CConstants.AuthorityPasswordReset)
				.ExecuteAsync();

			var userContext = UpdateUserInfo(authResult);

			return userContext;
		}

		//public async Task<UserContext> EditProfileAsync()
		//{
		//    IEnumerable<IAccount> accounts = await _pca.GetAccountsAsync();

		//    AuthenticationResult authResult = await _pca.AcquireTokenInteractive(B2CConstants.Scopes)
		//        .WithAccount(GetAccountByPolicy(accounts, B2CConstants.PolicyEditProfile))
		//        .WithPrompt(Prompt.NoPrompt)
		//        .WithAuthority(B2CConstants.AuthorityEditProfile)
		//        .ExecuteAsync();

		//    var userContext = UpdateUserInfo(authResult);

		//    return userContext;
		//}


		public async Task<UserContext> RegisterAsync()
		{
			AuthenticationResult authResult = await _pca.AcquireTokenInteractive(B2CConstants.Scopes)
				
				.WithAuthority(B2CConstants.AuthorityRegister)
				.ExecuteAsync();

			var userContext = UpdateUserInfo(authResult);

			return userContext;
		}

		private async Task<UserContext> SignInInteractively()
		{
			var windowLocatorService = DependencyService.Get<IParentWindowLocatorService>();
			AuthenticationResult authResult = await _pca.AcquireTokenInteractive(B2CConstants.Scopes)
				.WithParentActivityOrWindow(windowLocatorService?.GetCurrentParentWindow())
				.ExecuteAsync();

			var newContext = UpdateUserInfo(authResult);
			return newContext;
		}

		public async Task<UserContext> SignOutAsync(Func<Task> deregisterNotificationsAction)
		{

			IEnumerable<IAccount> accounts = await _pca.GetAccountsAsync();
			while (accounts.Any())
			{
				await _pca.RemoveAsync(accounts.FirstOrDefault());
				accounts = await _pca.GetAccountsAsync();
			}
			if (deregisterNotificationsAction != null)
				await deregisterNotificationsAction?.Invoke();
			var signedOutContext = new UserContext();
			signedOutContext.IsLoggedOn = false;
			return signedOutContext;
		}

		private IAccount GetAccountByPolicy(IEnumerable<IAccount> accounts, string policy)
		{
			foreach (var account in accounts)
			{
				string userIdentifier = account.HomeAccountId.ObjectId.Split('.')[0];
				if (userIdentifier.EndsWith(policy.ToLower())) return account;
			}

			return null;
		}

		private string Base64UrlDecode(string s)
		{
			s = s.Replace('-', '+').Replace('_', '/');
			s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
			var byteArray = Convert.FromBase64String(s);
			var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Count());
			return decoded;
		}

		public UserContext UpdateUserInfo(AuthenticationResult ar)
		{
			var newContext = new UserContext();
			newContext.IsLoggedOn = false;
			JObject user = ParseIdToken(ar.IdToken);

			newContext.AccessToken = ar.AccessToken;
			newContext.Name = user["name"]?.ToString();
			newContext.UserIdentifier = user["oid"]?.ToString();

			newContext.GivenName = user["given_name"]?.ToString();
			newContext.FamilyName = user["family_name"]?.ToString();

			newContext.StreetAddress = user["streetAddress"]?.ToString();
			newContext.City = user["city"]?.ToString();
			newContext.Province = user["state"]?.ToString();
			newContext.PostalCode = user["postalCode"]?.ToString();
			newContext.Country = user["country"]?.ToString();

			newContext.JobTitle = user["jobTitle"]?.ToString();

			var emails = user["emails"] as JArray;
			if (emails != null)
			{
				newContext.EmailAddress = emails[0].ToString();
			}
			newContext.IsLoggedOn = true;

			return newContext;
		}

		JObject ParseIdToken(string idToken)
		{
			// Get the piece with actual user info
			idToken = idToken.Split('.')[1];
			idToken = Base64UrlDecode(idToken);
			return JObject.Parse(idToken);
		}
	}

	public static class B2CConstants
	{
		// Azure AD B2C Coordinates
		public static string Tenant = "virtualaccounting.onmicrosoft.com";
		public static string AzureADB2CHostname = "virtualaccounting.b2clogin.com";
		public static string ClientID = "882f3004-d98f-4559-8fe1-3f86a56d126d";
		public static string PolicySignUpSignIn = "B2C_1_Sign_In";
		public static string PolicySignUp = "B2C_1_Sign_Up";
		//public static string PolicyEditProfile = "b2c_1_edit_profile";
		public static string PolicyResetPassword = "B2C_1_Reset_Password";

		public static string[] Scopes = { "https://virtualaccounting.onmicrosoft.com/a2c93f1f-ef5a-4259-92d3-e97346fa1bab/Api.ReadWrite", "offline_access" };

		public static string AuthorityBase = $"https://{AzureADB2CHostname}/tfp/{Tenant}/";
		public static string AuthoritySignInSignUp = $"{AuthorityBase}{PolicySignUpSignIn}";
		//public static string AuthorityEditProfile = $"{AuthorityBase}{PolicyEditProfile}";
		public static string AuthorityPasswordReset = $"{AuthorityBase}{PolicyResetPassword}";
		public static string AuthorityRegister = $"{AuthorityBase}{PolicySignUp}";
		public static string IOSKeyChainGroup = "com.dosarcusina.mobile";
	}
}
