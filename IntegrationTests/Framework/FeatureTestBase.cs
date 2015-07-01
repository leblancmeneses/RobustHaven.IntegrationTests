using System;
using System.Linq;
using NUnit.Framework;
using RobustHaven.IntegrationTests.Attributes;

namespace RobustHaven.IntegrationTests.Framework
{
	public abstract class FeatureTestBase
	{
		protected ILog Log;

		[SetUp]
		public virtual void Setup()
		{
			var feature = (FeatureAttribute)Attribute.GetCustomAttribute(GetType(), typeof(FeatureAttribute));
			var categories = ((CategoryAttribute[])Attribute.GetCustomAttributes(GetType(), typeof(CategoryAttribute))).Select(x => x.Name).ToList();
			var name = TestContext.CurrentContext.Test.Name;
			var index = name.IndexOf('(');
			if (index != -1)
			{
				name = name.Substring(0, index);
			}
			var scenario = GetType()
				.GetMethod(name)
				.GetCustomAttributes(true)
				.OfType<ScenarioAttribute>()
				.Single();

			Log = IntegrationTestsServiceFactory.Logger();

			Log.Info("***** Test: {0}:{1}", name, scenario.Name);
			
			var i = 0;
			foreach (var category in categories)
			{
				if (i++ > 0)
				{
					Log.GherkinAppend(" ");
				}
				Log.GherkinAppend("@{0}", category);
			}

			Log.Gherkin("", "");
			Log.Gherkin("", "Feature:\t{0}", feature.Name);

			Log.Gherkin("", "Background:", "");

			Background();

			Log.Gherkin("", "");
			Log.Gherkin("", "Scenario:\t{0}", scenario.Name);
		}

		public virtual void Background()
		{
		}


		[TearDown]
		public virtual void TearDown()
		{
			if (TestContext.CurrentContext.Result.Status == TestStatus.Passed)
			{
				Assert.Pass(Log.GherkinLog);
			}
		}
	}
}
