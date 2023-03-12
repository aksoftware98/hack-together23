using MagicNote.Core;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(MagicNote.Functions.Startup))]
namespace MagicNote.Functions
{
	public class Startup : FunctionsStartup
	{
		public override void Configure(IFunctionsHostBuilder builder)
		{
			var configuration = builder.GetContext().Configuration;
			builder.Services.AddSingleton(sp => new HttpClient());
			builder.Services.AddLanguageUnderstandingService(configuration["LanguageServiceApiKey"]);
		}
	}
}
