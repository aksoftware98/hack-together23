using MagicNote.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicNote.Client.ViewModels.Interfaces
{
	public interface IPlanningClient
	{

		Task<PlanDetails> AnalyzeNoteAsync(string? token, string? note);

		Task SubmitPlanAsync(string? token, PlanDetails request);

	}
}
