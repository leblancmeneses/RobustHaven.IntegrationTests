using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ResultDiff.FeatureParser.Models;

namespace ResultDiff.Extensions
{
	public static class StringExtensions
	{
		public static string CodeLabel(this string name)
		{
			return Regex.Replace(name.Trim(), "[^a-zA-Z0-9 ]", string.Empty).Replace(" ", "_");
		}
		public static string Text(this string message)
		{
			return Regex.Replace(message.Trim(), "\"", @"\""");
		}
		public static string ToText(this List<Statement> gherkin)
		{
			var builder = new StringBuilder();

			foreach (var statement in gherkin)
			{
				builder.AppendFormat("{0} {1}", statement.Key, statement.Message);
				builder.Append(Environment.NewLine);
			}
			return builder.ToString();
		}
	}
}
