using System;
using System.Threading;
using OpenQA.Selenium;
using RobustHaven.IntegrationTests.SeleniumExtensions;

namespace RobustHaven.IntegrationTests.KendoExtensions
{
	public abstract class KendoWidget
	{
		private IWebElement _element;
		protected IWebDriver Driver;

		protected abstract string KendoName { get; }

		
		protected KendoWidget(IWebDriver driver)
		{
			Driver = driver;
		}

		protected KendoWidget(IWebDriver driver, IWebElement node)
		{
			Driver = driver;
			_element = node;
		}


		protected void ScriptExecute(string command)
		{
			if (_element == null)
			{
				var dataRole = KendoName.Replace("kendo", string.Empty).ToLowerInvariant();
				_element = Driver.ScriptQuery<IWebElement>("return $('[data-role=\"{0}\"]').get(0);".Replace("{0}", dataRole));
			}

			var cmd = command.Replace("$k", "$(arguments[0]).data('" + KendoName + "')");
			Driver.ScriptExecute(cmd, _element);
			Thread.Sleep(1000);
		}

		protected T ScriptQuery<T>(string command, Func<T, int, bool> doWhile = null)
		{
			if (_element == null)
			{
				var dataRole = KendoName.Replace("kendo", string.Empty).ToLowerInvariant();
				_element = Driver.ScriptQuery<IWebElement>("return $('[data-role=\"{0}\"]').get(0);".Replace("{0}", dataRole));
			}

			var cmd = command.Replace("$k", "$(arguments[0]).data('" + KendoName + "')");

			T result;
			var cnt = 0;
			do
			{
				if (cnt != 0)
				{
					Thread.Sleep(1000);
				}

				var browserResult = ((IJavaScriptExecutor)Driver).ExecuteScript(cmd, _element);
				result = (T)Convert.ChangeType(browserResult, typeof(T));
				cnt++;
			} while (doWhile != null && doWhile(result, cnt));

			return result;
		}
	}
}
