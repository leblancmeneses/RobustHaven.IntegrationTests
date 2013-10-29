using System;
using System.Reflection;
using RobustHaven.IntegrationTests.Framework;

namespace RobustHaven.IntegrationTests.SeleniumExtensions.Core
{
	public class WebTestVisitor : BaseVisitor
	{
		private readonly WebTestContext _testContext;

		public WebTestVisitor(WebTestContext testContext)
		{
			_testContext = testContext;
		}

		public override void VisitEnter(Component component)
		{
			if (IsSubclassOfRawGeneric(typeof(WebTestComposite<>), component.GetType()))
			{
				Stack.Push(() =>
				{
					_testContext.Logger.IndentBy++;
					MethodInfo methodInfo = component.GetType().GetMethod("ExecuteOnEnter");
					methodInfo.Invoke(component, new object[] { _testContext });
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
					MethodInfo methodInfo = component.GetType().GetMethod("ExecuteOnLeave");
					methodInfo.Invoke(component, new object[] { _testContext });
					_testContext.Logger.IndentBy--;
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
					_testContext.Logger.IndentBy++;
					MethodInfo methodInfo = component.GetType().GetMethod("Execute");
					methodInfo.Invoke(component, new object[] { _testContext });
					_testContext.Logger.IndentBy--;
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