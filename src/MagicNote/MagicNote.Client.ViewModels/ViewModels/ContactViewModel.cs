using CommunityToolkit.Mvvm.ComponentModel;
using MagicNote.Shared.DTOs;

namespace MagicNote.Client.ViewModels.ViewModels
{
	public partial class ContactViewModel : ObservableObject
	{

		private MeetingPerson _person; 

		[ObservableProperty]
		private string? _id;

		[ObservableProperty]
		private string? _email;

		[ObservableProperty]
		private string? _displayName;

		[ObservableProperty]
		private bool _updateEmail = false;

		[ObservableProperty]
		private bool _addContact = false;

		public ContactViewModel(MeetingPerson person)
		{
			_person = person;
			Id = person.Id;
			Email = person.Email;
			DisplayName = person.Name;
			AddContact = person.AddContact;
			UpdateEmail = person.AddEmailToContact;
		}
	}
}
