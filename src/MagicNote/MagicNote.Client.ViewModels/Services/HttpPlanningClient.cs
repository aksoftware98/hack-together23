using MagicNote.Client.ViewModels.Exceptions;
using MagicNote.Client.ViewModels.Interfaces;
using MagicNote.Shared.DTOs;
using MagicNote.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MagicNote.Client.ViewModels.Services
{
	// TODO: Refactor the code of Http request and use a global user object instead of passing the token excplicityly as below
	public class HttpPlanningClient : IPlanningClient
	{

		/* COMMENT THE FOLLWOING LINE AND UNCOMMENT THE AFTER TO USE THE LOCAL API IN DEBUG MODE INSTEAD OF THE ONLINE VERSION */
		private const string BaseUrl = "https://magicnote.azurewebsites.net";
		
		/* UNCOMMENT THIS FOR LOCAL TESTING */
		//private const string BaseUrl = "https://localhost:7120";

		public async Task<PlanDetails> AnalyzeNoteAsync(string token, string note)
		{
			if (string.IsNullOrWhiteSpace(note))
				throw new ArgumentNullException(nameof(note));

			using var client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

			var response = await client.PostAsJsonAsync($"{BaseUrl}/analyze-note", new
			{
				query = note
			});

			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadFromJsonAsync<PlanDetails>();
				return result ?? new(); 
			}
			else
			{
				var content = await response.Content.ReadAsStringAsync();
				Debug.WriteLine(content);
				var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				throw new ApiException(error);
			}
		}

		public async Task SubmitPlanAsync(string? token, PlanDetails request)
		{

			using var client = new HttpClient();
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

			var response = await client.PostAsJsonAsync($"{BaseUrl}/submit-plan", request);

			if (!response.IsSuccessStatusCode)
			{ 
				var error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
				throw new ApiException(error);
			}
		}
	}
}
