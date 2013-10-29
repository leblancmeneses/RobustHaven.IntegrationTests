using System.Collections.Generic;
using RobustHaven.IntegrationTests.Framework;

namespace RobustHaven.IntegrationTests.SeleniumExtensions.Core
{
	public abstract class WebTestComposite<T> : Component
	{
		public abstract void ExecuteOnEnter(WebTestContext ctx);

		public abstract void ExecuteOnLeave(WebTestContext ctx);

		public T Model 
		{ 
			get; 
			set; 
		}

		public abstract IEnumerable<Component> Children { get; }


		public override void Accept(AVisitor visitor)
		{
			visitor.VisitEnter(this);

			foreach (Component c in Children)
			{
				c.Accept(visitor);
			}

			visitor.VisitLeave(this);
		}
	}
}
