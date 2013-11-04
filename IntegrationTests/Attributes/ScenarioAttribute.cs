using System;

namespace RobustHaven.IntegrationTests.Attributes
{
	public class ScenarioAttribute : Attribute
	{
		public ScenarioAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; protected set; }
	}
}
