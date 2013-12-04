using System;
using System.Collections.Generic;
using System.Text;
using ResultDiff.Extensions;

namespace ResultDiff.FeatureParser.Models
{
	public class Scenario
	{
		public Scenario()
		{
			Issues = new List<int>();
			Tags = new List<string>();
			Gherkin = new List<Statement>();
		}
		public Feature Feature { get; set; }
		public string Title { get; set; }
		public List<int> Issues { get; set; }
		public List<string> Tags { get; set; }
		public string Notes { get; set; }
		public List<Statement> Gherkin { get; set; }

		public override string ToString()
		{
			var builder = new StringBuilder();
			
			foreach (var tag in Feature.Tags)
			{
				builder.AppendFormat("@{0} ", tag);
			}
			builder.Append(Environment.NewLine);
			builder.AppendFormat("Feature: {0}", Feature.Title);
			builder.Append(Environment.NewLine);
			if (Feature.Background != null)
			{
				builder.Append("Background:");
				builder.Append(Environment.NewLine);
				builder.Append(Feature.Background.Gherkin.ToText());
				builder.Append(Environment.NewLine);
			}
			

			foreach (var tag in Tags)
			{
				builder.AppendFormat("@{0} ", tag);
			}
			builder.Append(Environment.NewLine);
			builder.AppendFormat("Scenario: {0}", Title);
			builder.Append(Environment.NewLine);

			builder.Append(Gherkin.ToText());

			return builder.ToString();
		}
	}
}