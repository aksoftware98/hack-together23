using MagicNote.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicNote.Core.Interfaces
{
	public interface ILanguageUnderstnadingService
	{

		Task<AITextAnalyzeResponse> AnalyzeTextAsync(string query);

		
	}
}
