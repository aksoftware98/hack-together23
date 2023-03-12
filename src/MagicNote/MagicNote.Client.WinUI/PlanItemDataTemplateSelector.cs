using MagicNote.Client.ViewModels;
using MagicNote.Shared.DTOs;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicNote.Client.WinUI
{
    public class PlanItemDataTemplateSelector : DataTemplateSelector
    {
		public DataTemplate ToDoItemPlanTemplate { get; set; }
		public DataTemplate EventPlanTemplate { get; set; }
		public DataTemplate MeetingPlanTemplate { get; set; }
		protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
		{
			var planItem = item as PlanItemViewModel;
			return planItem.Type switch
			{
				PlanEntityType.ToDoItem => ToDoItemPlanTemplate,
				PlanEntityType.Event => EventPlanTemplate,
				PlanEntityType.Meeting => MeetingPlanTemplate,
				_ => throw new NotImplementedException(),
			};
		}
	}
}
