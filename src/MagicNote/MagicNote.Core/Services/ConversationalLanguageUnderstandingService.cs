using MagicNote.Core.Interfaces;
using MagicNote.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MagicNote.Core.Services
{
	public class ConversationalLanguageUnderstandingService : ILanguageUnderstnadingService
	{

		private readonly HttpClient _httpClient;
		private readonly string _apiKey;
		public ConversationalLanguageUnderstandingService(HttpClient httpClient, 
														  string apiKey)
		{
			_httpClient = httpClient;
			_apiKey = apiKey;
		}

		public async Task<AITextAnalyzeResponse> AnalyzeTextAsync(string query)
		{
			_httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _apiKey);
			var response = await _httpClient.PostAsJsonAsync("https://cls-magicnoteai.cognitiveservices.azure.com/language/:analyze-conversations?api-version=2022-10-01-preview", new
			{
				analysisInput = new
				{
					conversationItem = new
					{
						text = query,
						id = "1",
						participantId = "1",
						modality = "Text"
					}
				},
				parameters = new
				{
					projectName = "magicnote",
					deploymentName = "FivthModel",
					stringIndexType = "Utf16CodeUnit",
				},
				kind = "Conversation",
			});

			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadFromJsonAsync<AITextAnalyzeResponse>();
				return result  ?? new();
			}
			else
			{
				throw new Exception("Error");
			}
		}
	}
}
