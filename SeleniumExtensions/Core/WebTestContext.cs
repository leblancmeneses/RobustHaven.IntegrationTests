using System;
using OpenQA.Selenium;

namespace RobustHaven.IntegrationTests.SeleniumExtensions.Core
{
	public abstract class WebTestContext
	{
		protected WebTestContext(string baseUrl)
		{
			BaseUrl = baseUrl;
		}

		public IWebDriver Browser { get; set; }

		public ILog Logger { get; set; }

		public string BaseUrl { get; private set; }


		public abstract void Scenario(string name, params object[] values);
		public abstract void Given(string input, params object[] values);
		public abstract void When(string input, params object[] values);
		public abstract void Then(string input, params object[] values);
		public abstract void And(string input, params object[] values);
		public abstract void But(string input, params object[] values);
		public abstract void Verified(string input, params object[] values);
		public abstract void Info(string input, params object[] values);

		public void Given(Action execute, string message, params object[] args)
		{
			execute();
			Given(message, args);
		}
		public void When(Action execute, string message, params object[] args)
		{
			execute();
			When(message, args);
		}
		public void Then(Action execute, string message, params object[] args)
		{
			execute();
			Then(message, args);
		}
		public void And(Action execute, string message, params object[] args)
		{
			execute();
			And(message, args);
		}

		public void Info(Action execute, string message, params object[] args)
		{
			execute();
			Info(message, args);
		}

		public void Verified(Action execute, string message, params object[] args)
		{
			execute();
			Verified(message, args);
		}
	}
}