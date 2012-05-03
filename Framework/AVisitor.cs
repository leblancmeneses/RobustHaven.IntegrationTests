using System;
using System.Reflection;

namespace RobustHaven.IntegrationTests.Framework
{
	public abstract class AVisitor
	{
		#region composite nodes

		public void VisitEnter(Component component)
		{
			var types = new[] {component.GetType()};
			MethodInfo methodInfo = GetType().GetMethod("VisitEnter", types);
			if (methodInfo != null)
			{
				methodInfo.Invoke(this, new object[] {component});
			}
			else
			{
				throw new Exception("Visitor does not implement the Visit method for the type: " + component.GetType());
			}
		}

		public void VisitExecute(Component component)
		{
			var types = new[] {component.GetType()};
			MethodInfo methodInfo = GetType().GetMethod("VisitExecute", types);
			if (methodInfo != null)
			{
				methodInfo.Invoke(this, new object[] {component});
			}
			else
			{
				throw new Exception("Visitor does not implement the Visit method for the type: " + component.GetType());
			}
		}

		public void VisitLeave(Component component)
		{
			var types = new[] {component.GetType()};
			MethodInfo methodInfo = GetType().GetMethod("VisitLeave", types);
			if (methodInfo != null)
			{
				methodInfo.Invoke(this, new object[] {component});
			}
			else
			{
				throw new Exception("Visitor does not implement the Visit method for the type: " + component.GetType());
			}
		}

		#endregion

		#region leaf node

		public void Visit(Component component)
		{
			// Use reflection to find and invoke the correct Visit method
			var types = new[] {component.GetType()};
			MethodInfo methodInfo = GetType().GetMethod("Visit", types);
			if (methodInfo != null)
			{
				methodInfo.Invoke(this, new object[] {component});
			}
			else
			{
				throw new Exception("Visitor does not implement the Visit method for the type: " + component.GetType());
			}
		}

		#endregion
	}
}