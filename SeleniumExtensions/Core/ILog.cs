namespace RobustHaven.IntegrationTests.SeleniumExtensions.Core
{
	public interface ILog
	{
		int IndentBy { get; set; }

		void Gherkin(string prefix, string input, params object[] values);
		void Verified(string input, params object[] values);
		void Info(string input, params object[] values);

		void NewLine();
	}
}
