using System.Collections.Generic;

namespace RobustHaven.IntegrationTests.Framework
{
	public abstract class Composite : Component
	{
		public abstract List<Component> Children { get; }

		public Sequence Sequence(Component other)
		{
			return new Sequence(this, other);
		}


		public override void Accept(AVisitor visitor)
		{
			visitor.VisitEnter(this);

			int i = 0;
			foreach (Component c in Children)
			{
				if (i++ != 0)
				{
					visitor.VisitExecute(this);
				}

				c.Accept(visitor);
			}

			visitor.VisitLeave(this);
		}
	}
}