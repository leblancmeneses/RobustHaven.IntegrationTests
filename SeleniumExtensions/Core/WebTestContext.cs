using OpenQA.Selenium;

namespace RobustHaven.IntegrationTests.SeleniumExtensions.Core
{
	public class WebTestContext
	{
		public WebTestContext(Configuration configuration)
		{
			Configuration = configuration;
			Driver = configuration.WebDriver;
			Logger = new ConsoleLogger();
		}

		public IWebDriver Driver { get; private set; }

		public ILog Logger { get; set; }

		public Configuration Configuration { get; private set; }
	}
}