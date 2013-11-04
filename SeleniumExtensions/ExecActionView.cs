using System;
using RobustHaven.IntegrationTests.SeleniumExtensions.Core;

namespace RobustHaven.IntegrationTests.SeleniumExtensions
{
	public class ExecActionView : WebTestLeaf<object>
	{
		private readonly Action<WebTestContext> _action;

		public ExecActionView(Action<WebTestContext> action)
		{
			_action = action;
		}

		public override void Execute(WebTestContext ctx)
		{
			_action(ctx);
		}
	}
}
