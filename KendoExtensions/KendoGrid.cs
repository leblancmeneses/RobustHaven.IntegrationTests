using System;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using OpenQA.Selenium;
using RobustHaven.IntegrationTests.SeleniumExtensions;

namespace RobustHaven.IntegrationTests.KendoExtensions
{
	public class KendoGrid : KendoWidget
	{
		public KendoGrid(IWebDriver driver)
			: base(driver)
		{
		}


		public KendoGrid(IWebDriver driver, IWebElement parentElement)
			: base(driver, parentElement)
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

		// http://docs.telerik.com/kendo-ui/api/framework/datasource#methods-get
		// can be number or string
		public IWebElement GetTableRowByModelId(int id)
		{
			var modelId = string.Format("{0}", id);
			Func<bool> dataItemExistsOnCurrentPage = () =>
			{
				var dataItemExists = ScriptQuery<bool>("return $k.dataSource.get(" + modelId + ") != null;");
				return dataItemExists;
			};

			Func<IWebElement> getTableRow = () =>
			{
				var dataItemUid = ScriptQuery<string>("return $k.dataSource.get(" + modelId + ").uid;");
				var row = ScriptQuery<IWebElement>("return $k.tbody.find(\"tr[data-uid='" + dataItemUid + "']\").get(0);");
				return row;
			};

			if (dataItemExistsOnCurrentPage())
			{
				return getTableRow();
			}

			IWebElement foundItem = null;

			// Item not found on current page, start searching from page 1
			DoPerPage(x =>
			{
				// Attempt to find item on this page
				if (!dataItemExistsOnCurrentPage()) return;

				x.Cancel = true;
				foundItem = getTableRow();
			});

			if (foundItem != null)
				return foundItem;

			return null;
		}

		public IWebElement GetTableRowByModelId(string id)
		{
			var modelId = string.Format("'{0}'", id);
			Func<bool> dataItemExistsOnCurrentPage = () =>
			{
				var dataItemExists = ScriptQuery<bool>("return $k.dataSource.get(" + modelId + ") != null;");
				return dataItemExists;
			};

			Func<IWebElement> getTableRow = () =>
			{
				var dataItemUid = ScriptQuery<string>("return $k.dataSource.get(" + modelId + ").uid;");
				var row = ScriptQuery<IWebElement>("return $k.tbody.find(\"tr[data-uid='" + dataItemUid + "']\").get(0);");
				return row;
			};

			if (dataItemExistsOnCurrentPage())
			{
				return getTableRow();
			}

			IWebElement foundItem = null;

			// Item not found on current page, start searching from page 1
			DoPerPage(x =>
			{
				// Attempt to find item on this page
				if (!dataItemExistsOnCurrentPage()) return;

				x.Cancel = true;
				foundItem = getTableRow();
			});

			if (foundItem != null)
				return foundItem;

			return null;
		}


		public void DoPerPage(Action<DoPerPageContext> doWork)
		{
			// Item not found on current page, start searching from page 1
			for (int page = 1; page <= TotalPages(); page++)
			{
				// Go to page 
				ScriptExecute("$k.dataSource.page(" + page + ")");

				ScriptExecute("isBrowserBusy() == false");

				var ctx = new DoPerPageContext { PageNumber = page, TotalItems = Total() };

				doWork(ctx);
				if (ctx.Cancel)
				{
					break;
				}
			}
		}


		public T DataItem<T>(IWebElement tr)
		{
			var result = Driver.ScriptQuery<string>("return JSON.stringify(  $(arguments[0]).data('" + KendoName + "').dataItem($(arguments[1]))  );", KendoWidgetHtmlElement(), tr);
			T model = JsonConvert.DeserializeObject<T>(result);
			return model;
		}
	}
}