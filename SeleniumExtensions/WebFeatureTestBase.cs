using System;
using System.Collections.Generic;
using System.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Safari;
using RobustHaven.IntegrationTests.Framework;

namespace RobustHaven.IntegrationTests.SeleniumExtensions
{
	public class WebFeatureTestBase : FeatureTestBase
	{
		protected WebScenarioContext Context;

		public override void Setup()
		{
			var selectedBrowser = ConfigurationManager.AppSettings["browser"];
			var browsers = new Dictionary<string, IWebDriver>(){
				//{ "chrome", new ChromeDriver() },
				{ "firefox", new FirefoxDriver() },
				//{ "phantom", new PhantomJSDriver() },
				//{ "ie", new InternetExplorerDriver() },
				//{ "safari", new SafariDriver() },
				//new EventFiringWebDriver(),
				//new RemoteWebDriver(new Uri(), new DesiredCapabilities())
			};

			if (!browsers.ContainsKey(selectedBrowser))
			{
				throw new NotImplementedException("Browser configured not supported.");
			}

			var baseUrl = ConfigurationManager.AppSettings["baseUrl"];
			Context = new WebScenarioContext(baseUrl) { Logger = Log, Browser = browsers[selectedBrowser] };

			base.Setup();
		}



		public override void TearDown()
		{
			Context.Execute();

			Context.Browser.Close();

			try
			{
				Context.Browser.SwitchTo().Alert().Accept();
			}
			catch (NoAlertPresentException)
			{
				// That's fine.
			}
			catch (WebDriverException)
			{
				// no confirm dialog
			}
			catch (InvalidOperationException)
			{
				// no confirm dialog
			}

			if (TestContext.CurrentContext.Result.Status == TestStatus.Passed)
			{
				Assert.Pass(Log.GherkinLog);
			}
		}
	}
}
