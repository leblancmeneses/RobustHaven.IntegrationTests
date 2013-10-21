using OpenQA.Selenium.Firefox;
using RobustHaven.IntegrationTests.SeleniumExtensions.Core;

namespace IntegrationTests
{
	public class FeatureTestBase
	{
		private Configuration _configuration;
		protected WebTestVisitor _visitor;

		[SetUp]
		public void Setup()
		{
			_configuration = new Configuration {BaseUrl = "http://localhost:54646", WebDriver = new FirefoxDriver()};
			_visitor = new WebTestVisitor(new WebTestContext(_configuration));
		}


		[TearDown]
		public void TearDown()
		{
			_configuration.WebDriver.Close();
			_configuration.WebDriver.Dispose();
		}
	}
}