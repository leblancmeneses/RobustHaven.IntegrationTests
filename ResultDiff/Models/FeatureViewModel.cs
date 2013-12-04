using System.Collections.Generic;

namespace ResultDiff.Models
{
	public class FeatureViewModel
	{
		public FeatureViewModel()
		{
			Scenarios = new List<ScenarioViewModel>();
			Status = ItemStatus.XNone; 
		}

		public string Name { get; set; }
		public ItemStatus Status { get; set; }
		public List<ScenarioViewModel> Scenarios { get; set; }
	}
}
