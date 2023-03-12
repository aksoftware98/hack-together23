using Microsoft.UI.Xaml.Data;
using System;

namespace MagicNote.Client.WinUI.Converters
{
	internal class InverseBoolToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var boolValue = (bool)value;
			return !boolValue ? Microsoft.UI.Xaml.Visibility.Visible : Microsoft.UI.Xaml.Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
