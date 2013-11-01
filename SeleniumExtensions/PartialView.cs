using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading;
using OpenQA.Selenium;
using RobustHaven.IntegrationTests.SeleniumExtensions.Core;

namespace RobustHaven.IntegrationTests.SeleniumExtensions
{
	public enum ViewModes
	{
		EditorTemplate,
		DisplayTemplate
	};

	public class PartialView<TViewModel> where TViewModel : class
	{
		public readonly WebTestContext WebTestContext;
		public readonly IWebDriver WebDriver;
		public readonly IWebElement PartialElement;

		public TViewModel ViewModel { get; set; }

		public ViewModes ViewMode { get; set; }

		public bool ClearInputFirst { get; set; }

		public PartialView(WebTestContext webTestContext, IWebDriver webDriver, IWebElement partialElement)
		{
			WebTestContext = webTestContext;
			WebDriver = webDriver;
			PartialElement = partialElement;
			ViewMode = ViewModes.EditorTemplate;
			ClearInputFirst = false;
		}


		public virtual void Write()
		{
			Thread.Sleep(1000);

			if (ViewModel == null)
				return;

			if (ViewModel is IEnumerable)
			{
				throw new NotSupportedException("Viewmodels that implement IEnumerable are not supported.");
			}

			var containerType = ViewModel.GetType();

			var properties = containerType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.CanWrite).OrderBy(x => x.MetadataToken);
			foreach (var property in properties)
			{
				bool canBeNull = property.PropertyType.IsValueType && (Nullable.GetUnderlyingType(property.PropertyType) != null);
				TypeCode typeCode = Type.GetTypeCode(canBeNull ? Nullable.GetUnderlyingType(property.PropertyType) : property.PropertyType);

				WriteForItem(property, typeCode);
			}

			Thread.Sleep(2000);
		}

		public virtual void Read()
		{
		}


		protected virtual void WriteForItem(PropertyInfo property, TypeCode typeCode)
		{
			var containerType = ViewModel.GetType();
			var expression = string.Format("{0}.{1}", containerType.FullName, property.Name);

			var expectedValue = property.GetValue(ViewModel, null);

			if (expectedValue == null)
			{
				return;
			}

			if (expectedValue.GetType().IsValueType)
			{
				if (Activator.CreateInstance(expectedValue.GetType()).ToString() == expectedValue.ToString())
				{
					return;
				}
			}

			if (!string.IsNullOrEmpty(expectedValue.ToString()) )
			{
				var textBox = PartialElement.FindElement(By.Id(expression));
				textBox.SendKeys(expectedValue.ToString());
			}
		}
	}
}
