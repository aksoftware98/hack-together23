using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MagicNote.Shared
{
	public class SubmitNoteRequest
	{

		public SubmitNoteRequest()
		{
			Query = string.Empty;
		}

		[JsonPropertyName("query")]
		public string Query { get; set; }

	}
}
