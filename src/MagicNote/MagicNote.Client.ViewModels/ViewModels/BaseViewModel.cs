using CommunityToolkit.Mvvm.ComponentModel;

namespace MagicNote.Client.ViewModels
{
	public partial class BaseViewModel : ObservableObject
	{
		#region Properties 
		[ObservableProperty]
		private bool _isBusy = false; 
		#endregion 
	}
}
