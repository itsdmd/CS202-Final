using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace FinalProject
{
	public class RawToRenamedConverter : IMultiValueConverter
	{
		const string conversionErrorFallback = "Conversion Error!";
		public RuleFactory Factory { get; set; } = new RuleFactory();

		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values.Length < 2) return conversionErrorFallback;

			var fileName = (string)values[0];
			var fileIndex = (int)values[1];
			
			Factory.FileName = fileName;
			Factory.FileIndex = fileIndex;
			return Factory.Parse();
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}