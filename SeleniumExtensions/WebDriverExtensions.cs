﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace RobustHaven.IntegrationTests.SeleniumExtensions
{
	public static class WebDriverExtensions
	{
		public static IWebElement FocusedElement(this IWebDriver driver)
		{
			return driver.ScriptQuery<IWebElement>("return document.activeElement;");
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

		public static void ScriptExecute(this IWebDriver driver, string script, params object[] args)
		{
			((IJavaScriptExecutor)driver).ExecuteScript(script, args);
		}

		public static T ScriptQuery<T>(this IWebDriver driver, string script, params object[] args)
		{
			return (T)((IJavaScriptExecutor)driver).ExecuteScript(script, args);
		}

		public static void WaitFor(this IWebDriver driver, string conditionExpression, int seconds = 15, params object[] args)
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

				string script = string.Format(@"return {0};", conditionExpression);
				result = driver.ScriptQuery<bool>(script, args);
			} while (result == false);
		}

		public static void InjectSeleniumExt(this IWebDriver driver)
		{
			var type = typeof(WebDriverExtensions);
			var assembly = type.Assembly;
			string[] resourceNames = assembly.GetManifestResourceNames().Where(x=>Regex.IsMatch(x, @"RobustHaven\.IntegrationTests\.SeleniumExtensions\.js")).OrderBy(x=>x).ToArray();
			foreach (string resourceName in resourceNames)
			{
				using (Stream stream = assembly.GetManifestResourceStream(resourceName))
				using (StreamReader reader = new StreamReader(stream))
				{
					string result = reader.ReadToEnd();
					((IJavaScriptExecutor)driver).ExecuteScript(result);
				}
			}
		}
	}
}