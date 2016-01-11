using System;
using System.ComponentModel;
using System.Globalization;

namespace JustCli
{
    public class ValueConverter
    {
        public object ConvertFromString(string stringValue, Type toType)
        {
            var typeConverter = TypeDescriptor.GetConverter(toType);
            var value = typeConverter.ConvertFrom(null, CultureInfo.InvariantCulture, stringValue);
            return value;
        }
    }
}