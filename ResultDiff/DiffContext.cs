namespace ResultDiff
{
	public class DiffContext
	{
		public DiffContext(string bddFolder, string testOutputFile)
		{
			BddFolder = bddFolder;
			TestOutputFile = testOutputFile;
		}

		public string BddFolder { get; private set; }
		public string TestOutputFile { get; private set; }
	}
}
