using CommunityToolkit.Mvvm.ComponentModel;
using MagicNote.Shared.DTOs;
using System.Collections.ObjectModel;

namespace MagicNote.Client.ViewModels.ViewModels
{
	public partial class PlanItemViewModel : ObservableObject
	{

		private PlanItem _item;
		
		[ObservableProperty]
		private string _id = string.Empty;

		[ObservableProperty]
		private string _title = string.Empty;

		[ObservableProperty]
		private DateTime? _startTime;

		[ObservableProperty]
		private DateTime? _endDate;

		[ObservableProperty]
		private ObservableCollection<ContactViewModel>? _contacts = new();

		[ObservableProperty]
		private PlanEntityType _type = PlanEntityType.None; 
		
		public PlanItemViewModel(PlanItem item)
		{
			_item = item;
			Id = item.Id;
			Title = item.Title;
			StartTime = item.StartTime;
			EndDate = item.EndTime;
			Contacts = item.People == null ? null : new(item.People.Select(p => new ContactViewModel(p)));
		}
	}
}
