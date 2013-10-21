using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace RobustHaven.IntegrationTests.SeleniumExtensions
{
	public static class WebDriverExtensions
	{
		public static IWebElement FocusedElement(this IWebDriver driver)
		{
			return (IWebElement) ((IJavaScriptExecutor) driver).ExecuteScript("return document.activeElement;");
		}

		public static IWebElement FindElement(this IWebDriver driver, ISearchContext searchContext, By by,
		                                      int timeoutInSeconds = 10)
		{
			var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
			return wait.Until(drv => searchContext.FindElement(by));
		}

		public static ReadOnlyCollection<IWebElement> FindElements(this IWebDriver driver, ISearchContext searchContext, By by,
		                                                           int timeoutInSeconds = 10)
		{
			var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
			return wait.Until(drv => (searchContext.FindElements(by).Count > 0) ? searchContext.FindElements(by) : null);
		}
	}
}