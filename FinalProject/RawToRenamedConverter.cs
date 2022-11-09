using System;
using System.Windows.Data;

namespace FinalProject
{
	public class RawToRenamedConverter : IValueConverter
	{
		public RuleFactory Factory { get; set; } = new RuleFactory();

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Factory.FileName = (string)value;

			return Factory.Parse();
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}