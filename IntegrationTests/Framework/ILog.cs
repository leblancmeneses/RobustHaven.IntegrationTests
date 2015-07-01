namespace RobustHaven.IntegrationTests.Framework
{
	public interface ILog
	{
		int IndentBy { get; set; }

		void Gherkin(string prefix, string input, params object[] values);
		void GherkinAppend(string input, params object[] values);

		void Verified(string input, params object[] values);
		void Info(string input, params object[] values);

		void NewLine();

		string GherkinLog { get; }
	}
}
