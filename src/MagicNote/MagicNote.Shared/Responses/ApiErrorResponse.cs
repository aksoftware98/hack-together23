using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicNote.Shared.Responses
{
	public class ApiErrorResponse
	{

        public ApiErrorResponse()
        {
            
        }

		public ApiErrorResponse(string? message)
		{
			Message = message;
		}

		public ApiErrorResponse(string? message, string[]? errors)
		{
			Message = message;
			Errors = errors;
		}

		public string? Message { get; set; }

		public string[]? Errors { get; set; }

	}
}
