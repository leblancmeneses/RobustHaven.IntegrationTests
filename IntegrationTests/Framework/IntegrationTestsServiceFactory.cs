using System;

namespace RobustHaven.IntegrationTests.Framework
{
	public static class IntegrationTestsServiceFactory
	{
		public static Func<ILog> Logger = () => new Log();
	}
}
