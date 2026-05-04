using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace SoftSkillsAML.Converters
{
    internal class BoolToBorderBrushConverter : IValueConverter
    {
        private static readonly SolidColorBrush CompletedBrush = new(Color.Parse("#44a08d"));
        private static readonly SolidColorBrush PendingBrush = new(Color.Parse("#764ba2"));

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is bool b && b ? CompletedBrush : PendingBrush;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return new Avalonia.Data.BindingNotification(value);
        }
    }
}
