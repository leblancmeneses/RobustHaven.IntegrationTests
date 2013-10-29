using OpenQA.Selenium;

namespace RobustHaven.IntegrationTests.SeleniumExtensions.Core
{
	public class WebTestContext
	{
		public WebTestContext(string baseUrl)
		{
			BaseUrl = baseUrl;
		}

		public IWebDriver Driver { get; set; }

		public ILog Logger { get; set; }

		public string BaseUrl { get; private set; }
	}
}