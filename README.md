RobustHaven.IntegrationTests
============================

RobustHaven.IntegrationTests is used for integration tests with WatiN or Selenium.

One of the bigest problems with automated integration tests is a way to seperate data from actual workflow that needs to execute.

This framework allows you to create a composite of your workflow and execute the workflow in the correct order while sharing strongly typed state.

Extensions contained in these packages allow you to interact with components found in AngularJs (NgExtensions), Kendo (KendoExtensions), and Bootstrap.


##Nuget Packages
[Selenium Extensions](http://www.nuget.org/packages/RobustHaven.IntegrationTests.SeleniumExtensions/)  

	Install-Package RobustHaven.IntegrationTests.SeleniumExtensions 

	
[AngularJs Extensions](http://www.nuget.org/packages/RobustHaven.IntegrationTests.NgExtensions/)  

	Install-Package RobustHaven.IntegrationTests.NgExtensions 


[Kendo Extensions](http://www.nuget.org/packages/RobustHaven.IntegrationTests.KendoExtensions/)  

	Install-Package RobustHaven.IntegrationTests.KendoExtensions 
	
	


Selenium Extensions:

	webDriver.FocusedElement(); // used to implemented ShouldBeFocused(myInputCtrl)
	driver.ScriptQuery<bool>(script, params);
	driver.ScriptExecute(script, params);
	driver.FindElement(ctx, By.CssSelector(".k-pager-info"), 10);
	driver.FindElements(ctx, By.CssSelector(".k-pager-info"), 10);



	
AngularJs Extensions: 
	
	IWebElement templateDiv = browser.FindElementByNgController("TemplateController");
	var result = browser.NgScope<TemplateViewModel>(templateDiv, "Data");
	webDriver.NgWaitFor(partialElement, "scope.IsBusyLoadingTemplate == false");


Kendo Extensions: (	work in progress )

	// there are constructor overloads for search context
	var grid = new KendoGrid(browser);
	int initialGridTotal = grid.Total();
	grid.Select(4);
	// do work
	new KendoGrid(browser).ShouldHaveIncreasedFromInit(initialGridTotal); 
	
	


### Sample 001: viewmodel referenced by multiple flows allows disconnected flows to interact as expected

flows can be composites (nested flows) - logins, partial views

	[TestFixture]
	public class TemplateFeatureTests : FeatureTestBase
	{
		[Test]
		public void Create_Template()
		{
			var viewModel = new TemplateViewModel { Name = Guid.NewGuid().ToString() };

			var createFlow = new CreateTemplate { Model = viewModel };

			createFlow.Execute(Context);
		}

		[Test]
		public void Create_Template_With_Case_Info()
		{
			var templateViewModel = new TemplateViewModel { Name = Guid.NewGuid().ToString() };

			var caseViewModel = new CaseViewModel() {Location_Id = 1, CaseNumber = "12345"};

			var createFlow = new CreateTemplate { Model = templateViewModel, CaseViewModel = caseViewModel };

			createFlow.Execute(Context);
		}

		[Test]
		public void Delete_Template()
		{
			var viewModel = new TemplateViewModel {Name = Guid.NewGuid().ToString()};

			var step1 = new CreateTemplate {Model = viewModel};

			var step2 = new DeleteTemplate {Model = viewModel};

			var workflow = new Sequence(step1, step2);

			workflow.Execute(Context);
		}
	}
	
	
### Sample 001: CreateTemplate.cs

Reference Images as you read the user code below. 
The goal was to make the developer api minimal and push waits and context setup into lower levels.
Currently we use strongly typed viewmodels used on server to generate views, used by angularjs on client $http, and hence can easily be reused in testing!

![grid](https://raw.github.com/leblancmeneses/RobustHaven.IntegrationTests/master/Docs/grid.jpg)
![form](https://raw.github.com/leblancmeneses/RobustHaven.IntegrationTests/master/Docs/form.jpg)


	namespace IntegrationTests.TemplateFlows
	{
		public class CreateTemplate : WebTestLeaf<TemplateViewModel>
		{
			public CaseViewModel CaseViewModel { get; set; }

			public override void Execute(WebTestContext ctx)
			{
				ConsoleLogger logger = ctx.Logger;
				IWebDriver browser = ctx.Driver; 

				string startingUrl = ctx.ToAbsoluteUrl<TemplateController>(t => t.Index());
				logger.Given("the user is at {0}", startingUrl);
				browser.Navigate().GoToUrl(startingUrl);

				int initialGridTotal = new KendoGrid(browser).Total();

				logger.When("the user clicks on 'New Template' button.");
				IWebElement newTemplateBtn = browser.bButton("New Template");
				newTemplateBtn.Click();



				
				IWebElement templateDiv = browser.FindElementByNgController("TemplateController");
				var templateView = new MyPartialView<TemplateViewModel>(ctx, browser, templateDiv)
				{
					ViewModel = Model,
					ViewMode = ViewModes.EditorTemplate
				};
				templateView.ShouldBeFocused(t => t.Name);
				templateView.SaveChangesShouldBeDisabled();

				logger.And("the user types the template name '{0}'", Model.Name);
				templateView.Write();
				templateView.SaveChanges();


				browser.NgWaitFor(templateDiv, "scope.Data.Id != 0");
				var result = browser.NgScope<TemplateViewModel>(templateDiv, "Data");
				Model.InjectFrom(result);
				logger.Then("the user can save a template (Id == {0}).", Model.Id);





				if (CaseViewModel != null)
				{
					var partialElement = browser.FindElementByNgController("CaseController");
					var caseView = new MyPartialView<CaseViewModel>(ctx, browser, partialElement)
					{
						ViewModel = CaseViewModel, 
						ViewMode = ViewModes.EditorTemplate
					};
					caseView.SaveChangesShouldBeDisabled();
					caseView.Write();
					caseView.SaveChanges();
				}



				browser.Navigate().GoToUrl(startingUrl);
				new KendoGrid(browser).ShouldHaveIncreasedFromInit(initialGridTotal);
			}
		}
	}
