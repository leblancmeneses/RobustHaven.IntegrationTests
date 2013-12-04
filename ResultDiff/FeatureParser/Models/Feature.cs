using System.Collections.Generic;

namespace ResultDiff.FeatureParser.Models
{
	public class Feature
	{
		public Feature()
		{
			Tags = new List<string>();
			Scenarios = new List<Scenario>();
		}

		public string Title { get; set; }
		public List<string> Tags { get; set; }
		public IEnumerable<Scenario> Scenarios { get; private set; }
		public Scenario Background { get; set; }

		public void AddScenario(Scenario scenario)
		{
			var items = (List<Scenario>)Scenarios;
			scenario.Feature = this;
			items.Add(scenario);
		}
	}
}
