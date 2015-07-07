using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using RobustHaven.IntegrationTests.Attributes;
using RobustHaven.IntegrationTests.SeleniumExtensions;
using RobustHaven.IntegrationTests.NgExtensions;

namespace Samples
{
	[TestFixture]
	[Category("samples")]
	[Feature("Protractor Tutorial")]
	public class ProtractorTutorialTests : WebFeatureTestBase
	{
		private IWebElement firstNumber;
		private IWebElement secondNumber;
		private IWebElement goButton;
		private IWebElement latestResult;

		public override void Background()
		{
			var task = Context.NavigateTo("http://juliemr.github.io/protractor-demo/");
			task.RunSynchronously();

            firstNumber = Context.Browser.FindElement(NgBy.Model("first"));
			secondNumber = Context.Browser.FindElement(NgBy.Model("second"));
			goButton = Context.Browser.FindElement(By.Id("gobutton"));
			latestResult = Context.Browser.FindElement(By.CssSelector("form h2.ng-binding"));
		}


		[Test]
		[Scenario("should have a title")]
		public void should_have_a_title()
		{
			Assert.IsTrue(Context.Browser.Title.Equals("Super Calculator"));
		}

		[Test]
		[Scenario("should add one and two")]
		public void should_add_one_and_two()
		{
			firstNumber.SendKeys("1");
			secondNumber.SendKeys("2");
			goButton.Click();

			Context.Browser.NgWaitFor(latestResult, @"/\.\s/.test(scope.latest) == false");

			Assert.IsTrue(latestResult.Text.Equals("3"));
		}

		[Test]
		[Scenario("should add four and six")]
		public void should_add_four_and_six()
		{
			firstNumber.SendKeys("4");
			secondNumber.SendKeys("6");
			goButton.Click();

			Context.Browser.NgWaitFor(latestResult, @"/\.\s/.test(scope.latest) == false");
			Assert.IsTrue(latestResult.Text.Equals("10"));
		}


		// All helpers should return Task.
		// The caller can then arrange the helper task into the control flow.
		public Task<double> Add(int num1, int num2)
		{
			return new Task<double>(() =>
			{
				firstNumber.SendKeys(num1.ToString());
				secondNumber.SendKeys(num2.ToString());
				goButton.Click();

				Context.Browser.NgWaitFor(latestResult, @"/\.\s/.test(scope.latest) == false");

				Assert.IsTrue(latestResult.Text.Equals((num1+num2).ToString()));
				return double.Parse(latestResult.Text);
			});
		}

		[Test]
		[Scenario("should have a history")]
		public void should_have_a_history()
		{
			var bodyElement = Context.Browser.FindElement(By.TagName("body"));

			Func<int> getCount = () => Context.Browser.NgFetch<int>(bodyElement,
				@"window.document.querySelectorAll(""tr[ng-repeat='result in memory']"").length");

			var tasks = new[] { Add(1, 2), Add(3, 4) }.ToList();
			tasks.ForEach(x => {
				x.RunSynchronously();
			});

			Assert.IsTrue(getCount() == 2);

			var task = Add(5, 6);
			task.RunSynchronously();

			Assert.IsTrue(getCount() == 3);
		}
	}
}
