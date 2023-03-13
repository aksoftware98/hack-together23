using MagicNote.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicNote.Client.ViewModels.Exceptions
{
	public class ApiException : Exception
	{

		public ApiErrorResponse Error { get; set; }

        public ApiException(ApiErrorResponse error) : base(error.Message)
        {
            Error = error;
        }

    }
}
