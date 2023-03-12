using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicNote.Client.WinUI.Converters
{
	public class DateTimeToTimeSpanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			var datetime = (DateTime?)value;
			if (datetime == null)
				return null;

			return new TimeSpan(datetime.Value.Hour, datetime.Value.Minute, datetime.Value.Second);
		}

		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			var timespan = (TimeSpan)value;

			return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, timespan.Hours, timespan.Minutes, timespan.Seconds);
		}
	}
}
