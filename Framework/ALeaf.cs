namespace RobustHaven.IntegrationTests.Framework
{
    public class ALeaf : Component
    {
        public override void Accept(AVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
