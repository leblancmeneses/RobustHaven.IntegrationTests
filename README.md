RobustHaven.IntegrationTests
============================

RobustHaven.IntegrationTests will help you communicate and manage the full cycle of a feature across your whole organization using Gherkin syntax.

RobustHaven.IntegrationTests is used for integration tests with Selenium. 
Extensions contained in these packages allow you to interact with components found in AngularJs (NgExtensions), Kendo (KendoExtensions), and Bootstrap.


Most applications have viewmodels *used* on the server to generate views, *used* by angularjs on client $http, and hence can easily be *reused* in testing!
This framework uses a Queue of C# [Task][1], similar to [protractor](https://angular.github.io/protractor/), to create a workflow while sharing strongly typed viewModel state with C# closures.

[1]: https://msdn.microsoft.com/en-us/library/system.threading.tasks.task(v=vs.110).aspx

Feature  | Protractor  | RobustHaven.IntegrationTests
------------- | -------------
Language  | JavaScript | C#
Test Framework | Jasmine | NUnit (custom Gherkin logger)
Promise API  | Web Driver Promise | Task
Configuration | Conf.js | App.config



##Nuget Packages
[Selenium Extensions](http://www.nuget.org/packages/RobustHaven.IntegrationTests.SeleniumExtensions/)  

	Install-Package RobustHaven.IntegrationTests.SeleniumExtensions 

Usage Samples:

	Browser.FocusedElement(); // used to implement ShouldBeFocused(myInputCtrl)
	Browser.WaitFor("isBrowserBusy() === false"); // ensure you have installed js-test-integration in SeleniumExtension package.
	var text = Browser.ScriptQuery<string>("return $(arguments[0]).text();", li);
	var row = Browser.ScriptQuery<IWebElement>("return $k.tbody.find(\"tr[data-uid='" + dataItemUid + "']\").get(0);");
	Browser.ScriptExecute(string.Format("$k.select({0});$k.trigger('change');", index));
	driver.FindElement(ctx, By.CssSelector(".k-pager-info"), 10);
	driver.FindElements(ctx, By.CssSelector(".k-pager-info"), 10);

	
	
[AngularJs Extensions](http://www.nuget.org/packages/RobustHaven.IntegrationTests.NgExtensions/)  

	Install-Package RobustHaven.IntegrationTests.NgExtensions 


Usage Samples: 
	
	var productDiv = browser.FindElement(browser, By.CssSelector("div[ng-controller='ProductController']"));
	browser.NgWaitFor(productDiv, "scope.Data.Id != 0");
	var result = browser.NgFetch<ProductViewModel>(productDiv, "scope.Data");
		// FYI: "isolateScope" like the "scope" variable are predefined and available in your expression with ngFetch. 
	
	
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
	var isdisabled = !actionsDropDown.IsEnabled;
	
	

	
[T4WithNUnit Extensions](http://www.nuget.org/packages/RobustHaven.IntegrationTests.T4WithNUnit/)  

	Install-Package RobustHaven.IntegrationTests.T4WithNUnit
	

Usage Sample:

	#used to init tests
	Scaffold T4WithNUnit
	
	#used to generate an nunit test given a feature file
	Scaffold T4WithNUnitFeature "Gherkin\Template.feature" -Force
	
	#used to generate an nunit test given a scenario string
	Scaffold T4WithNUnitScenario -Force -HowToGenerate:"test|leaf|composite" "Scenario:	Discoverable favorite feature for products `
	Given there are zero favorited products for user `
	 And user is at My XX page `
	When user selects the dropdown for new XX `
	Then they will get a prompt 'You have not favorited any products.' `
	 And 'products' text is link to My Products page" 
	 
	
	
	

	
# How do you start?
	
###  Step 1: Gherkin + balsamiq mockups

Create a Gherkin describing the feature you plan to build.  [Gherkin Language](http://docs.behat.org/guides/1.gherkin.html "Gherkin Language") 

	@Development
	Feature:	Product
	Background:
		Given an authenticated user
		 And the user is at the Products home page
		 
	Scenario:	Discoverable favorite feature for products
		Given there are zero favorited products for user
		 And user is at My XX page
		When user selects the dropdown for new XX
		Then they will get a prompt 'You have not favorited any products.'
		 And 'products' text is link to My Products page
		 
		 
	... more Scenario ...




### Step 2: nunit - no additional pluggins required for visual studio

Install the T4WithNUnitTest package to help init your test project.

	Install-Package RobustHaven.IntegrationTests.SeleniumExtensions 
	Install-Package RobustHaven.IntegrationTests.T4WithNUnit

Scaffold your test scenarios individually or specify a feature file.


	#init the integration test framework
	Scaffold T4WithNUnit

	#see T4WithNUnit Extensions usage samples above
	Scaffold T4WithNUnitFeature "Gherkin\Template.feature" -Force
	


Replace the stubbed comments with actual code.
			
	using System.Linq;
	using IntegrationTests.Core;
	using IntegrationTests.Extensions;
	using IntegrationTests.Kendo;
	using NUnit.Framework;
	using OpenQA.Selenium;
	using RobustHaven.IntegrationTests.Attributes;
	using RobustHaven.IntegrationTests.NgExtensions;
	using RobustHaven.IntegrationTests.SeleniumExtensions;
	using RobustHaven.IntegrationTests.SeleniumExtensions.Extensions;

	namespace IntegrationTests.Flows
	{
		[TestFixture]
		[Category("New Case")]
		[Category("filing")]
		[Feature("Filing - Required A")]
		public class Filing_RequiredAFeatureTests : FeatureTestBase
		{
			private FolderViewModel folderViewModel;

			#region test helpers

			private IWebElement GetADiv()
			{
				var partialElement = Visitor.Context.Browser.FindElementByNgController("AController");
				return partialElement;
			}

			private KendoGrid GetAGrid()
			{
				return new KendoGrid(Visitor.Context.Browser, GetADiv());
			}

			#endregion

			public override void Setup()
			{
				folderViewModel = new FolderViewModel()
				{
					Category_Id = 2,
					LowerCourt_Id = 3,
					OtherCaseNumber = "3223",
					Title = "dfdf",
					Attorney_Id = 3,
				};

				base.Setup();
			}

			public override void Background()
			{
				Visitor.Context.Given(() =>
				{
					var startingUrl = Visitor.Context.ToAbsoluteUrl(t => t.Action("AddOrEdit", "C", new{area="SomeModule"}));
					Visitor.Context.Browser.Navigate().GoToUrl(startingUrl);
				}, "an authenticated user is on the create a new case screen");

				Visitor.Context.And(() =>
				{
					folderViewModel.Location_Id = 6; // washington
				}, "user selects a location");

				Visitor.Context.And(() =>
				{
					folderViewModel.CaseType_Id = 1; // adult motor vehicle offenses
				}, "user selects a case type");

				Visitor.Context.Verified(() =>
				{
					Assert.IsTrue(GetAGrid().Total() == 0);
				}, "A listing has zero items");

				Visitor.Context.When(() =>
				{
					var partialElement = Visitor.Context.Browser.FindElementByNgController("CaseController");
					var caseView = new MyPartialView<FolderViewModel>(Visitor.Context, Visitor.Context.Browser, partialElement)
					{
						ViewModel = folderViewModel,
						ViewMode = ViewModes.EditorTemplate
					};
					caseView.Write();
					caseView.SaveChanges();
				}, "user submits the new case");
			}
		

			[Test]
			[Scenario("Required B are created")]
			public void Required_B_are_created()
			{
				var action = new ExecActionView(ctx =>
				{
					ctx.Then(() =>
					{
						var count = GetAGrid().Total();
						ctx.Verified(() => { Assert.IsTrue(count == 2); }, " {0} required B created for the washington location", count);
					}, "the A listing will show required B that are based on the location and case type");
				});

				action.Execute(Visitor);
			}


			[Test]
			[Scenario("Required B A type is read only when editing")]
			public void Required_B_A_type_is_read_only_when_editing()
			{
				var action = new ExecActionView(ctx =>
				{
					ctx.And(() =>
					{
						var grid = GetAGrid();
						grid.Select(1);
					}, "user selects a required A");

					ctx.Then(() =>
					{
						var element = GetADiv().FindElement(By.Id(ctx.ClientId<AViewModel>(x => x.AType_Id)));
						var dropdown = new KendoDropDownList(ctx.Browser, element);

						ctx.Verified(() => { Assert.IsFalse(dropdown.IsEnabled); }, "dropdown is disabled");

					}, "the user will not be able to change the A type");
				});

				action.Execute(Visitor);
			}



			[Test]
			[Scenario("Required A cannot be deleted")]
			public void Required_A_cannot_be_deleted()
			{
				var action = new ExecActionView(ctx =>
				{
					ctx.Then(() =>
					{
						var grid = GetAGrid();
						grid.Select(1);
						var row = grid.Select().First();
						var lastColumn = row.FindElement(By.CssSelector("td:last-of-type"));

						ctx.Verified(() =>
							{
								Assert.Throws<NoSuchElementException>(() => lastColumn.FindElement(By.TagName("select")));
							}, "drop down does not exist so user does not have the option to delete");
						
					}, "the user WILL NOT be able to delete the required B created");
				});

				action.Execute(Visitor);
			}

		}
	}

	
	
### Step 4: red/green/refactor
![nunit](https://raw.github.com/leblancmeneses/RobustHaven.IntegrationTests/master/Docs/nunit.JPG)


### Step 5: CI setup ResultDiff

	ResultDiff.exe -t="C:\ResultDiff\TestResult.xml" -bdd="C:\FolderContaining\BddFiles" -j="C:\ResultDiff\out.json"

![ResultDiff](https://raw.github.com/leblancmeneses/RobustHaven.IntegrationTests/master/Docs/ResultDiff.png)
