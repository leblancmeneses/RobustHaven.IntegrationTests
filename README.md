RobustHaven.IntegrationTests
============================

RobustHaven.IntegrationTests will help you communicate and manage the full cycle of a feature across your whole organization using Gherkin syntax.

RobustHaven.IntegrationTests is used for integration tests with Selenium. 
Extensions contained in these packages allow you to interact with components found in AngularJs (NgExtensions), Kendo (KendoExtensions), and Bootstrap.


Most applications have viewmodels *used* on the server to generate views, *used* by angularjs on client $http, and hence can easily be *reused* in testing!
This framework uses a Queue of C# [Task][1], similar to [protractor](https://angular.github.io/protractor/), to create a workflow while sharing strongly typed viewModel state with C# closures.

[1]: https://msdn.microsoft.com/en-us/library/system.threading.tasks.task(v=vs.110).aspx


| Feature  | Protractor  | RobustHaven.IntegrationTests |
|------------- | ------------- | ------------- |
| Language  | JavaScript | C# |
| Client Framework  | AngularJS Only | Any |
| Test Framework | Jasmine | NUnit (custom Gherkin logger) |
| Promise API  | Web Driver Promise (protractor.promise) | Task |
| Debugging | browser.pause() Or node debugger | Visual Studio breakpoints |
| Run specific tests | --spec fileLevel.js | NUnit runners with granularity to a specific test |
| Run specific suite | --suite | NUnit categories |
| Configuration | Conf.js | App.config |

Protractor maintains the [control flow](https://github.com/angular/protractor/blob/master/docs/control-flow.md) with a queue of promises that are dequeued and executed synchronously. External async tasks must be wrapped with a protractor.promise to ensure it is appended to the protractor queue.

RobustHaven.IntegrationTests encourages all helper methods to return a C# Task so the caller, NUnit [Test] or composite helper methods, can arrange the resulting task into the control flow. 
We do provide a Context.AddTask(...); queue similar to protractor but not necessarily needed as shown in the converted "Protractor Demo" below.




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
	

Package Manager Console Usage Sample:
	Install-Package RobustHaven.IntegrationTests.T4WithNUnit

	#used to generate an nunit test given a feature file
	Scaffold T4WithNUnitFeature "Gherkin\Template.feature" -Force
	
	#used to generate an nunit test given a scenario string
	Scaffold T4WithNUnitScenario -Force -Scenario ("Scenario:	Discoverable favorite feature for products `
	Given there are zero favorited products for user `
	 And user is at My XX page `
	When user selects the dropdown for new XX `
	Then they will get a prompt 'You have not favorited any products.' `
	 And 'products' text is link to My Products page")
	 
	
	
	

	
# How do you start?

Use the next five steps as a guide or view the [quickstart sample](https://github.com/leblancmeneses/RobustHaven.IntegrationTests/tree/master/Samples).

###  Step 1: Gherkin + mockups

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

Initialize your test project.

	Install-Package RobustHaven.IntegrationTests.SeleniumExtensions 
	
Optional Packages:

	Install-Package RobustHaven.IntegrationTests.NgExtensions 
	Install-Package RobustHaven.IntegrationTests.KendoExtensions 
	Install-Package RobustHaven.IntegrationTests.T4WithNUnit
	

Scaffold your test scenarios individually or specify a feature file.

	#see T4WithNUnit Extensions usage samples above
	Scaffold T4WithNUnitFeature "Gherkin\Template.feature" -Force
	


Replace the stubbed comments with actual code.  [Sample from Protractor's Demo](https://angular.github.io/protractor/#/tutorial "Protractor Demo Sample") 
			
	[TestFixture]
	[Category("samples")]
	[Feature("Protractor Tutorial")]
	public class ProtractorTutorialTests : WebFeatureTestBase
	{
		private IWebElement firstNumber;
		private IWebElement secondNumber;
		private IWebElement goButton;
		private IWebElement latestResult;

		public override void Background()
		{
			var task = Context.NavigateTo("http://juliemr.github.io/protractor-demo/");
			task.RunSynchronously();

			firstNumber = Context.Browser.FindElement(NgBy.Model("first"));
			secondNumber = Context.Browser.FindElement(NgBy.Model("second"));
			goButton = Context.Browser.FindElement(By.Id("gobutton"));
			latestResult = Context.Browser.FindElement(By.CssSelector("form h2.ng-binding"));
		}


		[Test]
		[Scenario("should have a title")]
		public void should_have_a_title()
		{
			Assert.IsTrue(Context.Browser.Title.Equals("Super Calculator"));
		}

		[Test]
		[Scenario("should add one and two")]
		public void should_add_one_and_two()
		{
			firstNumber.SendKeys("1");
			secondNumber.SendKeys("2");
			goButton.Click();

			Context.Browser.NgWaitFor(latestResult, @"/\.\s/.test(scope.latest) == false");

			Assert.IsTrue(latestResult.Text.Equals("3"));
		}

		[Test]
		[Scenario("should add four and six")]
		public void should_add_four_and_six()
		{
			firstNumber.SendKeys("4");
			secondNumber.SendKeys("6");
			goButton.Click();

			Context.Browser.NgWaitFor(latestResult, @"/\.\s/.test(scope.latest) == false");
			Assert.IsTrue(latestResult.Text.Equals("10"));
		}


		// All helpers should return a Task.
		// The caller can then arrange the helper task into the control flow.
		public Task<double> Add(int num1, int num2)
		{
			return new Task<double>(() =>
			{
				firstNumber.SendKeys(num1.ToString());
				secondNumber.SendKeys(num2.ToString());
				goButton.Click();

				Context.Browser.NgWaitFor(latestResult, @"/\.\s/.test(scope.latest) == false");

				Assert.IsTrue(latestResult.Text.Equals((num1+num2).ToString()));
				return double.Parse(latestResult.Text);
			});
		}

		[Test]
		[Scenario("should have a history")]
		public void should_have_a_history()
		{
			Func<int> getCount = () => Context.Browser.NgFetch<int>(Context.Browser.FindElement(By.TagName("body")),
				@"window.document.querySelectorAll(""tr[ng-repeat='result in memory']"").length");

			var tasks = new[] { Add(1, 2), Add(3, 4) }.ToList();
			tasks.ForEach(x => {
				x.RunSynchronously();
			});

			Assert.IsTrue(getCount() == 2);

			var task = Add(5, 6);
			task.RunSynchronously();

			Assert.IsTrue(getCount() == 3);
		}
	}

	
	
### Step 4: red/green/refactor
![nunit](https://raw.github.com/leblancmeneses/RobustHaven.IntegrationTests/master/Docs/nunit.JPG)


### Step 5: CI setup ResultDiff

	ResultDiff.exe -t="C:\ResultDiff\TestResult.xml" -bdd="C:\FolderContaining\BddFiles" -j="C:\ResultDiff\out.json"

![ResultDiff](https://raw.github.com/leblancmeneses/RobustHaven.IntegrationTests/master/Docs/ResultDiff.png)
