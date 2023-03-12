using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using MagicNote.Shared;
using MagicNote.Core.Interfaces;
using Microsoft.Graph;
using System.Net.Http;

namespace MagicNote.Functions
{
    public class SubmitNote
    {

        private readonly ILanguageUnderstnadingService _languageService;
		public SubmitNote(ILanguageUnderstnadingService languageService)
		{
			_languageService = languageService;
		}

		[FunctionName("SubmitNote")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Run the test of submit a note function");
			
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var request = JsonSerializer.Deserialize<SubmitNoteRequest>(requestBody);

            var serviceClient = new GraphServiceClient(new HttpClient());

            var result = await _languageService.AnalyzeTextAsync(request.Query);

            return new OkObjectResult(result);
        }
    }
}
