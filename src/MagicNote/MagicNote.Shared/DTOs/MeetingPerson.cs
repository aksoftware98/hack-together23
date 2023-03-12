namespace MagicNote.Shared.DTOs
{
	public class MeetingPerson
	{
		public string? Id { get; set; }
		public string? Name { get; set; }
		public string? Email { get; set; }
		
		/// <summary>
		/// Add the email to the contact
		/// </summary>
		public bool AddEmailToContact { get; set; }
		/// <summary>
		/// Request the server to the add the contact to the user's contacts if it's not exist
		/// </summary>
		public bool AddContact { get; set; }

		public bool AddContactEnabled => string.IsNullOrEmpty(Id);
	}
}
