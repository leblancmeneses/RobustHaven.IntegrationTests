namespace RobustHaven.IntegrationTests.Framework
{
	public abstract class Component
	{
		public abstract void Accept(AVisitor visitor);
	}
}