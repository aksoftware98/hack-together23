using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicNote.Shared.DTOs
{

	public class PlanResult
	{

        public PlanResult()
        {
			Items = Enumerable.Empty<PlanItem>(); 
        }

        public IEnumerable<PlanItem> Items { get; set; } 
		
	}
}
