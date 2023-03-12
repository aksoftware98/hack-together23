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
			
		[RelayCommand]
		private void SubmitChanges()
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
			DeleteItemAction(Id);
		}

		public PlanItemViewModel(PlanItem item, Action<string> deleteItemAction)
		{
			_item = item;
			DeleteItemAction = deleteItemAction;
			Id = item.Id;
			Title = item.Title;
			StartTime = item.StartTime;
			EndTime = item.EndTime;
			Contacts = item.People == null ? null : new(item.People.Select(p => new ContactViewModel(p)));
			Type = item.Type;
		}
	}
}
