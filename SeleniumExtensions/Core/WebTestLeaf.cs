using RobustHaven.IntegrationTests.Framework;

namespace RobustHaven.IntegrationTests.SeleniumExtensions.Core
{
	public abstract class WebTestLeaf<T> : ALeaf
	{
		public T Model { get; set; }

		public abstract void Execute(WebTestContext ctx);
	}
}