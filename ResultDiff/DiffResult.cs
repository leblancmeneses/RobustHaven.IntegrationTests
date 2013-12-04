using System;
using System.Collections.Generic;
using ResultDiff.Models;

namespace ResultDiff
{
	public class DiffResult
	{
		public DiffResult()
		{
			Features = new List<FeatureViewModel>();
		}

		public List<FeatureViewModel> Features { get; set; }
		public List<String> Messages { get; set; } 
	}
}
