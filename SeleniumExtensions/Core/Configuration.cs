using OpenQA.Selenium;

namespace RobustHaven.IntegrationTests.SeleniumExtensions.Core
{
	public class Configuration
	{
		public IWebDriver WebDriver { get; set; }

		public string BaseUrl { get; set; }
	}
}