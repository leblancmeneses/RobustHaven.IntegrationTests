using System.Data;
using OpenQA.Selenium;

namespace RobustHaven.IntegrationTests.KendoExtensions
{
	public class KendoGrid : KendoWidget
	{
		public KendoGrid(IWebDriver driver) :base(driver)
		{
		}

		public KendoGrid(IWebDriver driver, IWebElement parentElement):base(driver, parentElement)
		{
		}


		public KendoGrid Select(int index)
		{
			ScriptExecute("$k.select('tr:eq(" + index + ")');");
			return this;
		}


		public KendoGrid InvokeCreate()
		{
			ScriptExecute("$k.addRow();");
			return this;
		}

		protected override string KendoName
		{
			get { return "kendoGrid"; }
		}

		public int Total()
		{
			var total = ScriptQuery<int>("return $k.dataSource.total();", (retrived, cycles) => retrived == 0 && cycles < 10);

			return total;
		}


		public int TotalPages()
		{
			return ScriptQuery<int>("return $k.dataSource.totalPages();", (retrived, cycles) => retrived == 0 && cycles < 10);
		}
		

		public void ShouldHaveIncreasedFromInit(int initialGridTotal)
		{
			var total = Total();
			if (total <= initialGridTotal)
			{
				throw new ConstraintException(string.Format("'grid paging total' should have increased from {0} but was {1}.", initialGridTotal, total));
			}
		}

		public void ShouldHaveDecreasedFromInit(int initialGridTotal)
		{
			var total = Total();
			if (total >= initialGridTotal)
			{
				throw new ConstraintException(string.Format("'grid paging total' should have decreased from {0} but was {1}.", initialGridTotal, total));
			}
		}
	}
}
