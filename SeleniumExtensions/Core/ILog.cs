namespace RobustHaven.IntegrationTests.SeleniumExtensions.Core
{
	public interface ILog
	{
		int IndentBy { get; set; }
		
		void Background(string info);
		void Scenario(string name);

		void Given(string input, params object[] values);
		void When(string input, params object[] values);
		void Then(string input, params object[] values);
		void And(string input, params object[] values);
		void But(string input, params object[] values);

		void Verified(string input, params object[] values);
		void Info(string input, params object[] values);
	}
}
