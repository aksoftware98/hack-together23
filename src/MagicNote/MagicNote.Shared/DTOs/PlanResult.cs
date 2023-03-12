using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MagicNote.Shared.DTOs
{

	/// <summary>
	/// DTO represents the plan of tasks that the user is up to for the specific date.
	/// <see cref="PlanDetails"/> is used to retrieve the result of the AI analysis for the note submitted by the user, also it is used to instruct the server on how to send the tasks (To-do items, events, meetings, etc.) to Microsoft Graph
	/// </summary>
	public class PlanDetails
	{

        public PlanDetails()
        {
			Items = Enumerable.Empty<PlanItem>(); 
        }

		[JsonPropertyName("items")]
        public IEnumerable<PlanItem> Items { get; set; } 
		
	}
}
