using System;
using RobustHaven.IntegrationTests.Framework;
using RobustHaven.IntegrationTests.SeleniumExtensions.Core;

namespace IntegrationTests
{
	[TestFixture]
	public class SampleFeatureTests : FeatureTestBase
	{
		[Test]
		public void Delete_Template()
		{
			var viewModel = new TemplateViewModel {Name = Guid.NewGuid().ToString()};

			var step1 = FlowFactory.TemplateFlows.CreateTemplate(viewModel);

			var step2 = FlowFactory.TemplateFlows.DeleteTemplate(viewModel);

			var workflow = new Sequence(step1, step2);

			workflow.Execute(_visitor);
		}
	}
}