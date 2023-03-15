using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicNote.Client.ViewModels.Interfaces
{
	public interface IMessageDialogService
	{

		Task ShowOkDialogAsync(string title, string body);

	}
}
