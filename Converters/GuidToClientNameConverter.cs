using System;
using System.Globalization;
using Avalonia.Data.Converters;
using TreningAplikacija.ViewModels;

namespace TreningAplikacija.Converters;

public class GuidToClientNameConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Guid id && parameter is AdminViewModel vm)
        {
            return vm.GetClientName(id);
        }
        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}