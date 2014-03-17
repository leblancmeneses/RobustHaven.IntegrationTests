using OpenQA.Selenium;

namespace RobustHaven.IntegrationTests.KendoExtensions
{
	public class KendoDropDownList : KendoWidget
	{
		public KendoDropDownList(IWebDriver driver) : base(driver)
		{
		}

		public KendoDropDownList(IWebDriver driver, IWebElement element) : base(driver, element)
		{
		}


		/// <summary>
		/// Selects item from the Kendo Down List by index and triggers changes event.
		/// </summary>
		/// <param name="index">Index of item in drop down.</param>
		/// <returns>KendoDropDownList</returns>
		public KendoDropDownList Select(int index)
		{
			ScriptExecute(string.Format("$k.select({0});$k.trigger('change');", index));
			return this;
		}

		public T Value<T>() where T : struct
		{
			return ScriptQuery<T>("return $k.value();");
		}

		public bool IsEnabled 
		{
			get
			{
				return !ScriptQuery<bool>("return $('select', $k.wrapper).prop('disabled');");
			}
		}

		/// <summary>
		/// Selects item from the Kendo Down List by text and triggers change event.
		/// </summary>
		/// <param name="dataTextFieldName">Name of the Data Text Field.</param>
		/// <param name="valueOfDataTextField">Value of Data Text Field.</param>
		/// <returns>KendoDropDownList</returns>
		public KendoDropDownList Select(string dataTextFieldName, string valueOfDataTextField)
		{
			ScriptExecute(string.Format("$k.select(function(dataItem) {{ return dataItem.{0}.trim() === '{1}';}});$k.trigger('change');", dataTextFieldName, valueOfDataTextField));
			return this;
		}


		/// <summary>
		/// Selects item from the Kendo Down List by text and triggers change event.
		/// </summary>
		/// <param name="valueOfDataTextField">Value of Data Text Field. Searches a Data Text Field of "Text" by default. See overloaf it using different Data Text Field.</param>
		/// <returns></returns>
		public KendoDropDownList Select(string valueOfDataTextField)
		{
			return Select("Text", valueOfDataTextField);
		}


		protected override string KendoName
		{
			get { return "kendoDropDownList"; }
		}
	}
}
