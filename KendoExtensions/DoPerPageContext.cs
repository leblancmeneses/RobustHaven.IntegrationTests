namespace RobustHaven.IntegrationTests.KendoExtensions
{
	public class DoPerPageContext
	{
		public DoPerPageContext()
		{
			PageNumber = 0;
			Cancel = false;
			TotalItems = 0;
		}
		public int PageNumber { get; set; }
		public bool Cancel { get; set; }

		public int TotalItems { get; set; }
	}
}
