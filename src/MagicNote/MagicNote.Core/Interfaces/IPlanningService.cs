using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagicNote.Shared;
using MagicNote.Shared.DTOs;
namespace MagicNote.Core.Interfaces
{
	public interface IPlanningService
	{

		/// <summary>
		/// Understand a note using AI and build a productivty plan out of it for user to review and fill the remaining data
		/// </summary>
		/// <param name="note">Note query request</param>
		/// <returns></returns>
		Task<PlanDetails> AnalyzeNoteAsync(SubmitNoteRequest note);

		/// <summary>
		/// Submit the final plan to be added to create the events and the meetings in the calendar, and the to-do items
		/// </summary>
		/// <param name="plan"></param>
		/// <returns></returns>
		Task SubmitPlanAsync(PlanDetails plan);
	}
}
