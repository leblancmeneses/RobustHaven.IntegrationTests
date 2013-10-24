using System;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using RobustHaven.IntegrationTests.SeleniumExtensions;

namespace RobustHaven.IntegrationTests.KendoExtensions
{
	public static class KendoWebDriverExtensions
	{
		public static int kGridTotal(this IWebDriver driver, ISearchContext ctx)
		{
			const int limitTo = 10;
			int counter = 0;
			do
			{
				IWebElement pager = driver.FindElement(ctx, By.CssSelector(".k-pager-info"), 10);

				if (counter > (limitTo / 2) && pager.Text.Equals("No items to display"))
				{
					return 0;
				}

				Match match = Regex.Match(pager.Text, @"of\s*(?<total>\d+)\s*items", RegexOptions.IgnoreCase);

				if (match.Success)
				{
					return int.Parse(match.Groups["total"].Value);
				}

				Thread.Sleep(1000);
				counter++;
			} while (counter < limitTo);

			throw new TimeoutException("wait expired for fetching grid total");
		}

		public static int kGridTotal(this IWebDriver driver)
		{
			return driver.kGridTotal(driver);
		}
	}
}