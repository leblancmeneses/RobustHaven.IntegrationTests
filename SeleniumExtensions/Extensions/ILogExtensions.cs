using System;
using RobustHaven.IntegrationTests.SeleniumExtensions.Core;

namespace RobustHaven.IntegrationTests.SeleniumExtensions.Extensions
{
	public static class ILogExtensions
	{
		public static void Given(this ILog logger, Func<object[]> execute, string message)
		{
			logger.Given(message, execute());
		}
		public static void When(this ILog logger, Func<object[]> execute, string message)
		{
			logger.When(message, execute());
		}
		public static void Then(this ILog logger, Func<object[]> execute, string message)
		{
			logger.Then(message, execute());
		}
		public static void And(this ILog logger, Func<object[]> execute, string message)
		{
			logger.And(message, execute());
		}


		public static void Given(this ILog logger, Action execute, string message, params object[] args)
		{
			execute();
			logger.Given(message, args);
		}
		public static void When(this ILog logger, Action execute, string message, params object[] args)
		{
			execute();
			logger.When(message, args);
		}
		public static void Then(this ILog logger, Action execute, string message, params object[] args)
		{
			execute();
			logger.Then(message, args);
		}
		public static void And(this ILog logger, Action execute, string message, params object[] args)
		{
			execute();
			logger.And(message, args);
		}
	}
}
