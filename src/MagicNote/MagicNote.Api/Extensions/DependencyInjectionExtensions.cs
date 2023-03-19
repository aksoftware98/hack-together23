using Microsoft.Kiota.Abstractions.Authentication;
using System.Net.Http.Headers;

namespace MagicNote.Api.Extensions
{
	public static class DependencyInjectionExtensions
	{

		public static IServiceCollection AddAuthorizedHttpClient(this IServiceCollection services)
		{
			return services.AddScoped(sp =>
			{
				var context = sp.GetRequiredService<IHttpContextAccessor>();
				var token = context.ExtractToken();

				var client = new HttpClient();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				return client;
			});
		}

		public static IServiceCollection AddGraphServiceClient(this IServiceCollection services)
		{
			return services.AddScoped(sp =>
			{
				var context = sp.GetRequiredService<IHttpContextAccessor>();
				var token = context.ExtractToken();

				return new Microsoft.Graph.GraphServiceClient(new BaseBearerTokenAuthenticationProvider(new TokenProvider(() => Task.FromResult(token))));
			});
		}

	}
}
