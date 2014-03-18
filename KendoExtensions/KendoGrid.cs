using System;
using System.Collections.ObjectModel;
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


		public ReadOnlyCollection<IWebElement> Select()
		{
			return ScriptQuery<ReadOnlyCollection<IWebElement>>(@"
var selected = $k.select();
var result = [];
for(var i =0; i<selected.length; i++)
{
	result.push(selected[i]);
}
return result;");
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


		public IWebElement GetTableRowByViewModelId(int id)
		{
			if (DataItemExistsOnCurrentPage(id))
			{
				return GetTableRow(id);
			}

			// Item not found on current page, start searching from page 1
			for (int page = 1; page <= TotalPages(); page++)
			{
				// Go to page 
				ScriptExecute("$k.dataSource.page(" + page + ")");
				
				// Attempt to find item on this page
				if (DataItemExistsOnCurrentPage(id))
				{
					return GetTableRow(id);
				}
			}

			return null;
		}


		public void DoPerPage(Action<int> doWork)
		{
			// Item not found on current page, start searching from page 1
			for (int page = 1; page <= TotalPages(); page++)
			{
				// Go to page 
				ScriptExecute("$k.dataSource.page(" + page + ")");

				ScriptExecute("isBrowserBusy() == false");

				doWork(page);
			}
		}

		private bool DataItemExistsOnCurrentPage(int id)
		{
			var dataItemExists = ScriptQuery<bool>("return $k.dataSource.get(" + id + ") != null;");
			return dataItemExists;
		}


		private IWebElement GetTableRow(int id)
		{
			var dataItemUid = ScriptQuery<string>("return $k.dataSource.get(" + id + ").uid;");
			var row = ScriptQuery<IWebElement>("return $k.tbody.find(\"tr[data-uid='" + dataItemUid + "']\").get(0);");
			return row;
		}

	}
}
