using System;
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

		public override void Visit(Component template)
		{
			if (IsSubclassOfRawGeneric(typeof (WebTestLeaf<>), template.GetType()))
			{
				Stack.Push(() =>
				{
					var methodInfo = template.GetType().GetMethod("Execute");
					methodInfo.Invoke(template, new object[]{_testContext});
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
		static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
		{
			while (toCheck != null && toCheck != typeof(object))
			{
				var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
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