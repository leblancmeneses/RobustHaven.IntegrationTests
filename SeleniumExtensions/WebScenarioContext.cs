using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OpenQA.Selenium;
using RobustHaven.IntegrationTests.Framework;

namespace RobustHaven.IntegrationTests.SeleniumExtensions
{
	public class WebScenarioContext
	{
		public string BaseUrl { get; private set; }

		private readonly Queue<Task> _controlFlow = new Queue<Task>();

		public ILog Logger { get; set; }
		public IWebDriver Browser { get; set; }

		public WebScenarioContext(string baseUrl)
		{
			BaseUrl = baseUrl;
		}

		public Task NavigateTo(string url)
		{
			var task = new Task(() =>
			{
				Browser.Navigate().GoToUrl(url);
				Browser.InjectSeleniumExt();
			});
			return task;
		}

		public virtual void Given(Task execute, string message, params object[] args)
		{
			_controlFlow.Enqueue(execute);
			Logger.Gherkin("Given", message, args);
		}

		public virtual void When(Task execute, string message, params object[] args)
		{
			_controlFlow.Enqueue(execute);
			Logger.Gherkin("When", message, args);
		}

		public virtual void Then(Task execute, string message, params object[] args)
		{
			_controlFlow.Enqueue(execute);
			Logger.Gherkin("Then", message, args);
		}

		public virtual void And(Task execute, string message, params object[] args)
		{
			_controlFlow.Enqueue(execute);
			Logger.Gherkin(" And", message, args);
		}

		public virtual void But(Task execute, string message, params object[] args)
		{
			_controlFlow.Enqueue(execute.ContinueWith((task) => Logger.Gherkin(" But", message, args)));
		}

		public virtual void Info(Task execute, string message, params object[] args)
		{
			_controlFlow.Enqueue(execute);
			Logger.Info(message, args);
		}

		public virtual void Verified(Task execute, string message, params object[] args)
		{
			_controlFlow.Enqueue(execute);
			Logger.Verified(message, args);
		}


		public virtual void AddTask(Task execute)
		{
			_controlFlow.Enqueue(execute);
		}
		
		public virtual void Execute()
		{
			while (_controlFlow.Count > 0)
			{
				var task = _controlFlow.Dequeue();
				task.RunSynchronously();
			}
		}
	}
}