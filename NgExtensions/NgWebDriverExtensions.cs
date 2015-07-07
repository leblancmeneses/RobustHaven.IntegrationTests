using System;
using System.Threading;
using Newtonsoft.Json;
using OpenQA.Selenium;
using RobustHaven.IntegrationTests.SeleniumExtensions;

namespace RobustHaven.IntegrationTests.NgExtensions
{
	public static class NgWebDriverExtensions
	{
		public static T NgFetch<T>(this IWebDriver driver, IWebElement element, string expression)
		{
			string script = string.Format(@"
var scope = angular.element(arguments[0]).scope();
var isolateScope = angular.element(arguments[0]).isolateScope();
return JSON.stringify({0});", expression);
			object result = ((IJavaScriptExecutor)driver).ExecuteScript(script, element);
			T model = JsonConvert.DeserializeObject<T>((string)result);

			return model;
		}

		
		public static void NgWaitFor(this IWebDriver driver, IWebElement element, string conditionExpression, int seconds = 15)
		{
			int cnt = 0;
			bool result;
			do
			{
				if (cnt >= seconds)
				{
					throw new TimeoutException("Wait until true exceeded wait limit.");
				}

				if (cnt++ > 0)
				{
					Thread.Sleep(1000);
				}

				string script = string.Format(@"
var scope = angular.element(arguments[0]).scope();
var isolateScope = angular.element(arguments[0]).isolateScope();
return {0};", conditionExpression);
				result = driver.ScriptQuery<bool>(script, element);
			} while (result == false);
		}
	}
}