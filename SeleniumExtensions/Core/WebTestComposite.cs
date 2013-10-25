using System.Collections.Generic;
using RobustHaven.IntegrationTests.Framework;

namespace RobustHaven.IntegrationTests.SeleniumExtensions.Core
{
	public abstract class WebTestComposite<T> : Component
	{
		readonly List<Component> _children = new List<Component>();
		
		public abstract void ExecuteOnEnter(WebTestContext ctx);

		public abstract void ExecuteOnLeave(WebTestContext ctx);

		public T Model 
		{ 
			get; 
			set; 
		}

		public void AddChild(ALeaf leaf)
		{
			_children.Add(leaf);
		}

		public List<Component> Children { get { return _children; } }


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
