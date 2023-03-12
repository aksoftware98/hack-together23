using System.Text.Json.Serialization;

namespace MagicNote.Shared.DTOs
{
	public class MeetingPerson
	{
		[JsonPropertyName("id")]
		public string? Id { get; set; }

		[JsonPropertyName("name")]
		public string? Name { get; set; }

		[JsonPropertyName("email")]
		public string? Email { get; set; }

		/// <summary>
		/// Add the email to the contact
		/// </summary>
		[JsonPropertyName("addEmailToContact")]
		public bool AddEmailToContact { get; set; }

		[JsonPropertyName("addContact")]
		/// <summary>
		/// Request the server to the add the contact to the user's contacts if it's not exist
		/// </summary>
		public bool AddContact { get; set; }

		public bool AddContactEnabled => string.IsNullOrEmpty(Id);
	}
}
