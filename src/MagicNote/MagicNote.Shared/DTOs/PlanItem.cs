using System.Text.Json.Serialization;

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

		[JsonPropertyName("id")]
		public string Id { get; private set; }

		[JsonPropertyName("type")]
		public PlanEntityType Type { get; set; }

		[JsonPropertyName("title")]
		public string Title { get; set; }

		[JsonPropertyName("startTime")]
		public DateTime? StartTime { get; set; }

		[JsonPropertyName("endTime")]
		public DateTime? EndTime { get; set; }

		[JsonPropertyName("people")]
		public IEnumerable<MeetingPerson> People { get; set; }
	}
}
