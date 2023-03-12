namespace MagicNote.Shared.DTOs
{
	public class PlanItem
	{

        public PlanItem()
        {
			Title = string.Empty;
			People = Enumerable.Empty<MeetingPerson>();
			Id = Guid.NewGuid().ToString(); 
		}

		public string Id { get; private set; }

		public PlanEntityType Plan { get; set; }
        public string Title { get; set; }
		
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }

		public IEnumerable<MeetingPerson> People { get; set; }
	}
}
