using System;
using System.ComponentModel;

namespace ResultDiff.Extensions
{
	public static class EnumExtensions
	{
		public static string GetCustomDescription(object objEnum)
		{
			var fi = objEnum.GetType().GetField(objEnum.ToString());
			var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
			return (attributes.Length > 0) ? attributes[0].Description : objEnum.ToString();
		}
		 
		public static string ToDescription(this Enum value)
		{
			return GetCustomDescription(value);
		}
	}
}
