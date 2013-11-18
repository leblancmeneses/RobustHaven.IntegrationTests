RobustHaven.IntegrationTests
============================

RobustHaven.IntegrationTests is used for integration tests with WatiN or Selenium.

One of the bigest problems with automated integration tests is a way to seperate data from actual workflow that needs to execute.

Strongly typed viewmodels are used on the server to generate views, used by angularjs on client $http, and hence can easily be reused in testing!
This framework allows you to create a composite of your workflow and execute the workflow in the correct order while sharing strongly typed state.
The goal was to make the developer api minimal and push waits and context setup into lower levels.


Extensions contained in these packages allow you to interact with components found in AngularJs (NgExtensions), Kendo (KendoExtensions), and Bootstrap.


##Nuget Packages
[Selenium Extensions](http://www.nuget.org/packages/RobustHaven.IntegrationTests.SeleniumExtensions/)  

	Install-Package RobustHaven.IntegrationTests.SeleniumExtensions 

Usage Samples:

	Browser.FocusedElement(); // used to implemented ShouldBeFocused(myInputCtrl)
	Browser.WaitFor("isBrowserBusy() === false");
	var text = Browser.ScriptQuery<string>("return $(arguments[0]).text();", li);
	var row = Browser.ScriptQuery<IWebElement>("return $k.tbody.find(\"tr[data-uid='" + dataItemUid + "']\").get(0);");
	Browser.ScriptExecute(string.Format("$k.select({0});$k.trigger('change');", index));
	driver.FindElement(ctx, By.CssSelector(".k-pager-info"), 10);
	driver.FindElements(ctx, By.CssSelector(".k-pager-info"), 10);

	
	
[AngularJs Extensions](http://www.nuget.org/packages/RobustHaven.IntegrationTests.NgExtensions/)  

	Install-Package RobustHaven.IntegrationTests.NgExtensions 


Usage Samples: 
	
	var items = browser.FindElementsByNgController("ItemController");
	var productDiv = browser.FindElementByNgController("ProductController");
	Browser.NgWaitFor(productDiv, "scope.Data.Id != 0");
	var result = browser.NgScope<ProductViewModel>(productDiv, "Data");
	
	
[Kendo Extensions](http://www.nuget.org/packages/RobustHaven.IntegrationTests.KendoExtensions/)  

	Install-Package RobustHaven.IntegrationTests.KendoExtensions 
	
Usage Samples:

	// there are constructor overloads for search context
	var grid = new KendoGrid(browser);
	int initialGridTotal = grid.Total();
	grid.Select(4);
	
	grid.DoPerPage(i =>
	{	
		// do work for each page in grid (unfavorite/favorite items)
		ctx.Delay();
	});
			
	var tableRow = productGrid.GetTableRowByViewModelId(Model.Id);
	var actionsDropDown = new KendoDropDownList(ctx.Browser, tableRow);
	actionsDropDown.Select("Delete");
	
	

	
[T4WithNUnit Extensions](http://www.nuget.org/packages/RobustHaven.IntegrationTests.T4WithNUnit/)  

	Install-Package RobustHaven.IntegrationTests.T4WithNUnit
	

Usage Sample:

	#used to init tests
	Scaffold T4WithNUnit
	
	#used to generate an nunit test given a feature file
	Scaffold T4WithNUnitFeature "Gherkin\Template.feature" -Force
	
	#used to generate an nunit test given a scenario string
	Scaffold T4WithNUnitScenario -Force -IsLeaf:$true "Scenario:	Discoverable favorite feature for products `
	Given there are zero favorited products for user `
	 And user is at My XX page `
	When user selects the dropdown for new XX `
	Then they will get a prompt 'You have not favorited any products.' `
	 And 'products' text is link to My Products page" 
	 
	
	
	

	
# How do you start?
	
###  Step 1: Gherkin + balsamiq mockups

	@Development
	Feature:	Product
	Background:
		Given an authenticated user
		 And the user is at the Products home page

	... more Scenario ...
		 
		 
	Scenario:	Discoverable favorite feature for products
		Given there are zero favorited products for user
		 And user is at My XX page
		When user selects the dropdown for new XX
		Then they will get a prompt 'You have not favorited any products.'
		 And 'products' text is link to My Products page
		 
		 
	... more Scenario ...




### Step 2: nunit - no additional pluggins required for visual studio

Install the T4WithNUnitTest package to help init your test project.
Run following command when you plan to implement the next scenario.
	
	
	Scaffold T4WithNUnitTest -Force -IsLeaf:$true "Scenario: Your Gherkin Scenario"
	

Here is how a multi-step test will look like:
	
	namespace IntegrationTests.Flows
	{
		[TestFixture]
		[Category("product")]
		[Feature("Product")]
		public class ProductFeatureTests : FeatureTestBase
		{
			public override void Background()
			{
				Visitor.Context.Given("an authenticated user");
				Visitor.Context.And(() =>
				{
					var startingUrl = Visitor.Context.ToAbsoluteUrl<ProductController>(t => t.Index());
					Visitor.Context.Browser.Navigate().GoToUrl(startingUrl);
				}, "the user is at the Products home page");
			}


			... more tests ...

			[Test]
			[Scenario("User edits an existing product's item information")]
			public void User_edits_an_existing_products_item_information()
			{
				var productViewModel = new ProductViewModel { Name = Guid.NewGuid().ToString() };
				var itemViewModel = new ItemViewModel() { Location_Id = 1, ItemNumber = "12345" };
				var itemView = new ItemViewEditorProduct()
				{
					IsValidInput = true,
					IsPartiallyFilled = true,
					ActivityMode = ItemActivityMode.ModifyProduct,
					Model = itemViewModel
				};
				var create = new CreateProduct
				{
					Model = productViewModel,
					ItemView = itemView
				};
				var change = new ExecActionView(context =>
				{
					productViewModel.Name = string.Format("changed {0}", productViewModel.Name);
					itemViewModel.Title = "changed abc";
				});
				var edit = new EditProduct
				{
					Model = productViewModel,
					ItemView = itemView
				};
				var workflow = new Sequence(create, change)
								.Sequence(edit);
				workflow.Execute(Visitor);
			}

			... more tests ...
		}
	}

	
	
### Step 3:  DiscoverableProductFeature.cs

	public class DiscoverableProductFeature : WebTestLeaf<object>
	{
		public override void Execute(WebTestContext ctx)
		{
			ctx.Given(() =>
			{
				// your code to do this work with ctx.Browser
			}, "there are zero favorited products for user");


			ctx.And(() =>
			{
				// your code to do this work with ctx.Browser
			}, "user is at My XX page");


			ctx.When(() =>
			{
				// your code to do this work with ctx.Browser
			}, "user selects the dropdown for new xx");

 
			ctx.Then(() =>
				{ 
					// your code to do this work with ctx.Browser
				
					ctx.Verified(() => li = favoriteProductList.Single(), "only one li element existed");
  
				}, "they will get a prompt '{0}'", msg);


			ctx.And(() =>
			{ 
				// your code to do this work with ctx.Browser
			
				ctx.Verified(() => Assert.AreEqual(href, productHomePage), verified);

			}, "'products' text is link to My Products page");
		}
	}

	
### Step 4: red/green/refactor
![nunit](https://raw.github.com/leblancmeneses/RobustHaven.IntegrationTests/master/Docs/nunit.JPG)
