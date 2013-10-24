using System.Threading;
using OpenQA.Selenium;

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


		public void Run(string command)
		{
			if (_element == null)
			{
				var dataRole = KendoName.Replace("kendo", string.Empty).ToLowerInvariant();
				_element = (IWebElement)((IJavaScriptExecutor)Driver).ExecuteScript("return $('[data-role=\"{0}\"]').get(0);".Replace("{0}", dataRole));
			}

			var cmd = command.Replace("$k", "$(arguments[0]).data('" + KendoName + "')");
			((IJavaScriptExecutor)Driver).ExecuteScript(cmd, _element);
			Thread.Sleep(1000);
		}

	}
}
