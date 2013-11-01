using System;
using System.Reflection;
using RobustHaven.IntegrationTests.Framework;

namespace RobustHaven.IntegrationTests.SeleniumExtensions.Core
{
	public class WebTestVisitor : BaseVisitor
	{
		public WebTestContext Context { get; private set; }

		public WebTestVisitor(WebTestContext testContext)
		{
			Context = testContext;
		}

		public override void VisitEnter(Component component)
		{
			if (IsSubclassOfRawGeneric(typeof(WebTestComposite<>), component.GetType()))
			{
				Stack.Push(() =>
				{
					Context.Logger.NewLine();
					Context.Logger.IndentBy++;
					MethodInfo methodInfo = component.GetType().GetMethod("ExecuteOnEnter");
					methodInfo.Invoke(component, new object[] { Context });
				});
			}
			else
			{
				if (component is Sequence)
				{
					base.VisitEnter(component);
				}
			}
		}

		public override void VisitExecute(Component component)
		{
			if (component is Sequence)
			{
				base.VisitExecute(component);
			}
		}
		
		public override void VisitLeave(Component component)
		{
			if (IsSubclassOfRawGeneric(typeof(WebTestComposite<>), component.GetType()))
			{
				Stack.Push(() =>
				{
					Context.Logger.NewLine();
					MethodInfo methodInfo = component.GetType().GetMethod("ExecuteOnLeave");
					methodInfo.Invoke(component, new object[] { Context });
					Context.Logger.IndentBy--;
				});
			}
			else
			{
				if (component is Sequence)
				{
					base.VisitLeave(component);
				}
			}
		}


		public override void Visit(Component component)
		{
			if (IsSubclassOfRawGeneric(typeof (WebTestLeaf<>), component.GetType()))
			{
				Stack.Push(() =>
				{
					Context.Logger.NewLine();
					Context.Logger.IndentBy++;
					MethodInfo methodInfo = component.GetType().GetMethod("Execute");
					methodInfo.Invoke(component, new object[] { Context });
					Context.Logger.IndentBy--;
				});
			}
			else
			{
				throw new NotImplementedException("visitor expects only WebTestLeaf<> types.");
			}
		}

		public override void Dispose()
		{
		}

		//http://stackoverflow.com/questions/457676/check-if-a-class-is-derived-from-a-generic-class
		private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
		{
			while (toCheck != null && toCheck != typeof (object))
			{
				Type cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
				if (generic == cur)
				{
					return true;
				}
				toCheck = toCheck.BaseType;
			}
			return false;
		}
	}
}