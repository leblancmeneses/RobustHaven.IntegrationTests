using System;
using RobustHaven.IntegrationTests.SeleniumExtensions.Core;

namespace RobustHaven.IntegrationTests.SeleniumExtensions.Extensions
{
	public static class ILogExtensions
	{
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



		public static void Info(this ILog logger, Action execute, string message, params object[] args)
		{
			execute();
			logger.Info(message, args);
		}


		public static void Verified(this ILog logger, Action execute, string message, params object[] args)
		{
			execute();
			logger.Verified(message, args);
		}
	}
}
