namespace ResultDiff.Models
{
	public class ScenarioViewModel
	{
		public ScenarioViewModel()
		{
			Status = ItemStatus.XNone;
			Diff = new Diff<string>(); 
		}

		public string Name { get; set; }
		
		public ItemStatus Status { get; set; }

		public Diff<string> Diff { get; set; }

		public string ErrorText { get; set; }

		public bool DidPass { get; set; }
	}
}