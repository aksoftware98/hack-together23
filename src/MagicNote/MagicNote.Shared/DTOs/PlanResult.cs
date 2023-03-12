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

	public class PlanItem
	{

        public PlanItem()
        {
			Title = string.Empty;
        }

		public PlanEntityType Plan { get; set; }
        public string Title { get; set; }
		
		public DateTime? StartTime { get; set; }
		
	}

	public enum PlanEntityType
	{
		Event,
		Meeting,
		ToDoItem,
		None,
		// TODO: Support more types later
	}
}
