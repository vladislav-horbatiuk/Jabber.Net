using System;
using System.ComponentModel;
using System.Globalization;
using agsXMPP;

namespace Jabber.Net.Server.Utils
{
    class JidTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return new Jid((string)value);
        }
    }
}
