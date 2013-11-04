using System;

namespace RobustHaven.IntegrationTests.Attributes
{
	public class FeatureAttribute : Attribute
	{
		public FeatureAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; protected set; }
	}
}
