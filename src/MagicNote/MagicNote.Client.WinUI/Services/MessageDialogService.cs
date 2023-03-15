using MagicNote.Client.ViewModels.Interfaces;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinRT.Interop;

namespace MagicNote.Client.WinUI.Services
{
	public class MessageDialogService : IMessageDialogService
	{
		private readonly Page _page;
		public MessageDialogService(Page page)
		{
			_page = page;
		}

		public async Task ShowOkDialogAsync(string title, string body)
		{
			var contentDialgo = new ContentDialog()
			{
				Title = title,
				Content = body,
				CloseButtonText = "Ok",
				XamlRoot = _page.Content.XamlRoot,
				RequestedTheme = _page.ActualTheme,
			};

			await contentDialgo.ShowAsync();
		}
	}
}
