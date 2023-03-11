using MagicNote.Core.Interfaces;
using MagicNote.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MagicNote.Core
{
	public static class DependencyInjectionExtensions
	{

		public static IServiceCollection AddLanguageUnderstandingService(this IServiceCollection services, string apiKey)
		{
			if (string.IsNullOrWhiteSpace(apiKey))
				throw new ArgumentNullException(nameof(apiKey));
			
			return services.AddSingleton<ILanguageUnderstnadingService>(sp => new ConversationalLanguageUnderstandingService(sp.GetRequiredService<HttpClient>(), apiKey));
		}

	}
}
