using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagicNote.Shared.DTOs;

namespace MagicNote.Client.ViewModels
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

		[ObservableProperty]
		private bool _isEditMode = false;

		[ObservableProperty]
		private string _errorMessage = string.Empty;

		public Action<ContactViewModel> DeleteItemAction { get; set; }

		private string _title = string.Empty;
		public string Title
		{
			get => _title;
			set
			{
				_title = value;
				OnPropertyChanged(nameof(Title));
			}
		}

		[RelayCommand]
		private void SaveChanges()
		{
			if (string.IsNullOrWhiteSpace(Title))
				ErrorMessage = "Title is required";

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
			AddContact = person.AddContact;
			UpdateEmail = person.AddEmailToContact;
		}
	}
}
