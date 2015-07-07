using OpenQA.Selenium;

namespace RobustHaven.IntegrationTests.NgExtensions
{
	public static class NgBy
	{
		public static By Model(string expression)
		{
			return By.CssSelector($"[ng-model='{expression}']");
		}
	}
}
