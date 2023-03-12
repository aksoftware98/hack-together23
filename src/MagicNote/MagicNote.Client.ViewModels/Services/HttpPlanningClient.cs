﻿using MagicNote.Client.ViewModels.Interfaces;
using MagicNote.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MagicNote.Client.ViewModels.Services
{
	public class HttpPlanningClient : IPlanningClient
	{
		
		public Task<PlanResult> AnalyzeNoteAsync(string token, string note)
		{
			if (string.IsNullOrWhiteSpace(note))
				throw new ArgumentNullException(nameof(note));

			var jsonText = $@"""
				{{
    ""items"": [
        {{
            ""plan"": 0,
            ""title"": ""buy some stationary"",
            ""startTime"": ""2023-03-13T21:00:00"",
            ""endTime"": ""2023-03-13T22:00:00"",
            ""people"": []
        }},
        {{
            ""plan"": 0,
            ""title"": ""play football"",
            ""startTime"": ""2023-03-13T19:00:00"",
            ""endTime"": ""2023-03-13T20:00:00"",
            ""people"": []
        }},
        {{
            ""plan"": 2,
            ""title"": ""study some Azure"",
            ""startTime"": null,
            ""endTime"": null,
            ""people"": []
        }},
        {{
            ""plan"": 1,
            ""title"": ""to"",
            ""startTime"": ""2023-03-13T20:30:00"",
            ""endTime"": ""2023-03-13T21:30:00"",
            ""people"": [
                {{
                    ""id"": null,
                    ""name"": ""Julio"",
                    ""email"": null,
                    ""addEmailToContact"": true,
                    ""addContact"": true,
                    ""addContactEnabled"": true
                }},
                {{
                    ""id"": ""AQMkADAwATY0MDABLWU3ZTItMDY5NC0wMAItMDAKAEYAAANJ3szxihH5SrT8gftvFZ7MBwCdIsSQqxPRTpRxjiXL8C6UAAACAQ4AAACdIsSQqxPRTpRxjiXL8C6UAAY21TqcAAAA"",
                    ""name"": ""John Smith"",
                    ""email"": ""aksoftware19981998@gmail.com"",
                    ""addEmailToContact"": false,
                    ""addContact"": false,
                    ""addContactEnabled"": false
                }}
            ]
        }}
    ]
}}
""";

            return Task.FromResult(JsonSerializer.Deserialize<PlanResult>(jsonText));
		}

		public Task SubmitPlanAsync(PlanResult request)
		{
			throw new NotImplementedException();
		}
	}
}
