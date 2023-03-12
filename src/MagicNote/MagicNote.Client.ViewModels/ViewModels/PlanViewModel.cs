using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagicNote.Shared.DTOs;
using System.Collections.ObjectModel;

namespace MagicNote.Client.ViewModels
{
	public partial class PlanViewModel : ObservableObject
	{
		[ObservableProperty]
		private ObservableCollection<PlanItemViewModel> _items = new();

        public PlanViewModel(PlanResult planResult)
        {
			Items = new(planResult.Items.Select(p => new PlanItemViewModel(p, RemoveItem)));
		}

        private void RemoveItem(string id)
		{
			var item = Items.SingleOrDefault(i => i.Id == id);
			if (item != null)
				Items.Remove(item);
		}
	}
}
