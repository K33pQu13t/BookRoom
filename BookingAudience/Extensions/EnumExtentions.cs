using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BookingAudience.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription<TEnum>(this TEnum enumValue) where TEnum : struct
        {
            FieldInfo fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            DescriptionAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return enumValue.ToString();
        }
    }
}
