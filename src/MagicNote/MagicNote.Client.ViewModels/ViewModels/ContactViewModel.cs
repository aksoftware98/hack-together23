using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagicNote.Shared.DTOs;

namespace MagicNote.Client.ViewModels
{
	// TODO: Support the save state of the edit before save (when cancel the edits) 
	// TODO: Remove the property of AddEmail to update
	// TODO: Support a better error message and validations
	// TODO: Show the Save Contact only if actual changes happens 
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

		[ObservableProperty]
		private bool _isEditMode = false;

		[ObservableProperty]
		private string _errorMessage = string.Empty;

		[ObservableProperty]
		private bool _isDeleteAllowed = true;

		[ObservableProperty]
		private bool _isValidEmail = false; 

		public Action<ContactViewModel> DeleteItemAction { get; set; }
		
		[RelayCommand]
		private void SaveChanges()
		{
			IsValidEmail = !string.IsNullOrWhiteSpace(Email);
			IsEditMode = false;
		}

		[RelayCommand]
		private void CancelEdit()
		{
			IsEditMode = false;
		}

		[RelayCommand]
		private void Edit()
		{
			IsEditMode = true;
		}

		[RelayCommand]
		private void Delete()
		{
			DeleteItemAction(this);
		}
		
		public ContactViewModel(MeetingPerson person, Action<ContactViewModel> deleteAction)
		{
			_person = person;
			DeleteItemAction = deleteAction;
			Id = person.Id;
			Email = person.Email;
			DisplayName = person.Name;
			AddContact = true;
			UpdateEmail = person.AddEmailToContact;
			IsValidEmail = !string.IsNullOrWhiteSpace(Email);
		}
	}
}
