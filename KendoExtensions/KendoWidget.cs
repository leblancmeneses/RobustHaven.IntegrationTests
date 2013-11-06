using System;
using System.Threading;
using OpenQA.Selenium;
using RobustHaven.IntegrationTests.SeleniumExtensions;

namespace RobustHaven.IntegrationTests.KendoExtensions
{
	public abstract class KendoWidget
	{
		private IWebElement _element;
		private IWebElement _dataRoleElement;
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
			var dataRole = KendoName.Replace("kendo", string.Empty).ToLowerInvariant();
			if (_element == null)
			{
				_dataRoleElement = Driver.ScriptQuery<IWebElement>("return $('[data-role=\"{0}\"]').get(0);".Replace("{0}", dataRole));
			}
			else
			{
				_dataRoleElement = Driver.ScriptQuery<IWebElement>("return ($(arguments[0]).attr('data-role') == \"{0}\")? arguments[0] : $('[data-role=\"{0}\"]', $(arguments[0])).get(0);".Replace("{0}", dataRole), _element);
			}

			var cmd = command.Replace("$k", "$(arguments[0]).data('" + KendoName + "')");

			Driver.WaitFor("$(arguments[0]).data('" + KendoName + "') != null", 15, _dataRoleElement);

			Driver.ScriptExecute(cmd, _dataRoleElement);
			Thread.Sleep(1000);
		}

		protected T ScriptQuery<T>(string command, Func<T, int, bool> doWhile = null)
		{
			var dataRole = KendoName.Replace("kendo", string.Empty).ToLowerInvariant();
			if (_element == null)
			{
				_dataRoleElement = Driver.ScriptQuery<IWebElement>("return $('[data-role=\"{0}\"]').get(0);".Replace("{0}", dataRole));
			}
			else
			{
				_dataRoleElement = Driver.ScriptQuery<IWebElement>("return ($(arguments[0]).attr('data-role') == \"{0}\")? arguments[0] : $('[data-role=\"{0}\"]', $(arguments[0])).get(0);".Replace("{0}", dataRole), _element);
			}

			var cmd = command.Replace("$k", "$(arguments[0]).data('" + KendoName + "')");


			Driver.WaitFor("$(arguments[0]).data('" + KendoName + "') != null", 15, _dataRoleElement);

			T result;
			var cnt = 0;
			do
			{
				if (cnt != 0)
				{
					Thread.Sleep(1000);
				}

				if (typeof(T).IsValueType)
				{
					var browserResult = ((IJavaScriptExecutor)Driver).ExecuteScript(cmd, _dataRoleElement);
					result = (T)Convert.ChangeType(browserResult, typeof(T));
				}
				else
				{
					result = Driver.ScriptQuery<T>(cmd, _dataRoleElement);
				}
				cnt++;
			} while (doWhile != null && doWhile(result, cnt));

			return result;
		}
	}
}
