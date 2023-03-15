using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagicNote.Shared.DTOs;
using System.Collections.ObjectModel;

namespace MagicNote.Client.ViewModels
{
	public partial class PlanItemViewModel : ObservableObject
	{

		private PlanItem _item;

		[ObservableProperty]
		private string _id = string.Empty;

		[ObservableProperty]
		private DateTime? _startTime;

		[ObservableProperty]
		private DateTime? _endTime;

		[ObservableProperty]
		private ObservableCollection<ContactViewModel>? _contacts = new();

		[ObservableProperty]
		private PlanEntityType _type = PlanEntityType.None;

		[ObservableProperty]
		private bool _isEditMode = false;

		[ObservableProperty]
		private string _errorMessage = string.Empty;

		public Action<string> DeleteItemAction { get; set; }

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

		private string _existingTitle = string.Empty;
		private DateTime? _existingStartTime = null;
		private DateTime? _existingEndTime = null;
		

		[RelayCommand]
		private void SubmitChanges()
		{
			var validationResult = ValidateContent();
			if (!validationResult)
			{
				return;
			}
			ErrorMessage = string.Empty;
			IsEditMode = false;
		}

		[RelayCommand]
		private void CancelEdit()
		{
			IsEditMode = false;
			ResetConent();
		}

		[RelayCommand]
		private void Edit()
		{
			IsEditMode = true;
			_existingEndTime = EndTime;
			_existingStartTime = StartTime;
			_existingTitle = Title;
		}

		[RelayCommand]
		private void Delete()
		{
			DeleteItemAction(Id);
		}

		private void DeleteContact(ContactViewModel model)
		{
			if (Contacts.Count == 1)
			{
				var firstContact = Contacts.FirstOrDefault();
				firstContact.IsDeleteAllowed = false;
				return;
			}
				
			Contacts.Remove(model);
		}

		public PlanItemViewModel(PlanItem item, Action<string> deleteItemAction)
		{
			_item = item;
			DeleteItemAction = deleteItemAction;
			Id = item.Id;
			Title = item.Title;
			StartTime = item.StartTime;
			EndTime = item.EndTime;
			Contacts = item.People == null ? null : new(item.People.Select(p => new ContactViewModel(p, DeleteContact)));
			Type = item.Type;
		}

		private bool ValidateContent()
		{
			if (string.IsNullOrWhiteSpace(Title))
			{
				ErrorMessage = "Title is required";
				return false;
			}

			if (Type == PlanEntityType.Meeting || Type == PlanEntityType.Event)
			{
				var startTimeOnly = TimeOnly.FromDateTime(StartTime.Value);
				var endTimeOnly = TimeOnly.FromDateTime(EndTime.Value);
				if (endTimeOnly <= startTimeOnly)
				{
					ErrorMessage = "End time must be greater than start time";
					return false;
				}
			}

			if (Type == PlanEntityType.Meeting)
			{
				if (Contacts == null || Contacts.Count == 0)
				{
					ErrorMessage = "Meeting must have at least one attendee";
					return false;
				}

				var hasInvalidContact = Contacts.Any(c => string.IsNullOrWhiteSpace(c.DisplayName) && string.IsNullOrWhiteSpace(c.Email));
				if (hasInvalidContact)
				{
					ErrorMessage = "Contact must have a name or valid email address";
					return false;
				}
			}

			return true;
		}

		private void ResetConent()
		{
			Title = _existingTitle;
			StartTime = _existingStartTime;
			EndTime = _existingEndTime;
		}
	}
}
