using RobustHaven.IntegrationTests.Framework;

namespace RobustHaven.IntegrationTests.SeleniumExtensions.Core
{
	public static class ComponentExtensions
	{
		public static void Execute(this Component component, BaseVisitor visitor)
		{
			component.Accept(visitor);
			visitor.Execute();
		}
	}
}