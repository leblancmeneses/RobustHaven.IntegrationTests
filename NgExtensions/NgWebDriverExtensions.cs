using System;
using System.Threading;
using Newtonsoft.Json;
using OpenQA.Selenium;
using RobustHaven.IntegrationTests.SeleniumExtensions;

namespace RobustHaven.IntegrationTests.NgExtensions
{
	public static class NgWebDriverExtensions
	{
		public static T NgScope<T>(this IWebDriver driver, IWebElement element, string path, Func<T, bool> waitUntilTrue)
		{
			T model;
			int cnt = 0;
			do
			{
				if (cnt++ > 0)
				{
					Thread.Sleep(1000);
				}

				if (!string.IsNullOrEmpty(path))
				{
					path = string.Format(".{0}", path);
				}

				string script = string.Format("return JSON.stringify(angular.element(arguments[0]).scope(){0});", path);
				object result = ((IJavaScriptExecutor) driver).ExecuteScript(script, element);
				model = JsonConvert.DeserializeObject<T>((string) result);
			} while (waitUntilTrue(model) == false);

			Thread.Sleep(1000);
			return model;
		}

		public static IWebElement FindByNgController(this IWebDriver driver, ISearchContext searchContext,
		                                             string controllerName)
		{
			string css = string.Format("div[ng-controller='{0}']", controllerName);
			return driver.FindElement(searchContext, By.CssSelector(css));
		}
	}
}