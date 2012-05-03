namespace RobustHaven.IntegrationTests.Framework
{
    public abstract class Component
    {
        abstract public void Accept(AVisitor visitor);
    }
}
