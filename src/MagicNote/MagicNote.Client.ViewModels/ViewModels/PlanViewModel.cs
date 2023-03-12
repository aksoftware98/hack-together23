using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace MagicNote.Client.ViewModels
{
	public partial class PlanViewModel : ObservableObject
	{
		[ObservableProperty]
		private ObservableCollection<PlanItemViewModel> _items = new();

		[RelayCommand]
		private void RemoveItem(string id)
		{
			var item = Items.SingleOrDefault(i => i.Id == id);
			if (item != null)
				Items.Remove(item);
		}
	}
}
