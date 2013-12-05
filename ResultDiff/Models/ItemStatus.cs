using System.ComponentModel;

namespace ResultDiff.Models
{
	public enum ItemStatus
	{
		[Description("?")]
		XNone = 0,
		[Description("Not Found")]
		XNotFound = 2,
		[Description("Deleted")]
		XDeleted = 4,
		[Description("Modified")]
		XModified = 8,
		[Description("Okay")]
		XOkay = 64,
		[Description("Parsing Failed")]
		ParsingFailed = 128
	}

	public enum TestStatus
	{
		[Description("?")]
		XNone = 0,
		[Description("Passed")]
		XPassed = 2,
		[Description("Failed")]
		XFailed = 4,
	}
}
