using MagicNote.Client.ViewModels.Interfaces;
using MagicNote.Client.WinUI.Pages;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicNote.Client.WinUI.Services
{
	public class NavigationService : INavigationService
	{

		static IReadOnlyDictionary<string, Type> _pagesByKey = new Dictionary<string, Type>
		{
			{ "PlanningPage", typeof(PlanningPage) }
		};

        public NavigationService(Frame frame)
        {
			Frame = frame ?? new Frame();
        }

        public static Frame Frame { get; private set; }

		public void NavigateTo(string pageKey)
		{
			Frame.Navigate(_pagesByKey[pageKey]);
		}
	}
}
