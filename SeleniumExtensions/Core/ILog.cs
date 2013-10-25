namespace RobustHaven.IntegrationTests.SeleniumExtensions.Core
{
	public interface ILog
	{
		void Given(string input, params object[] values);
		void When(string input, params object[] values);
		void Then(string input, params object[] values);
		void And(string input, params object[] values);
		void But(string input, params object[] values);
	}
}
