RobustHaven.IntegrationTests
============================

RobustHaven.IntegrationTests is used for integration tests with WatiN or Selenium.

One of the bigest problems with automated integration tests is a way to seperate data from actual workflow that needs to execute.

This framework allows you to create a composite of your workflow and execute the workflow in the correct order while sharing strongly typed state.

Behind the scenes is a fluent interface using composition and a hierarchical visitor pattern.

Extensions contained in these packages allow you to interact with components found in AngularJs (NgExtensions), Kendo (KendoExtensions), and Bootstrap.


##Nuget Packages
[Selenium Init](http://www.nuget.org/packages/RobustHaven.IntegrationTests.SeleniumExtensions/)  

	Install-Package RobustHaven.IntegrationTests.SeleniumExtensions 


[Kendo Extensions](http://www.nuget.org/packages/RobustHaven.IntegrationTests.KendoExtensions/)  

	Install-Package RobustHaven.IntegrationTests.KendoExtensions 


[AngularJs Extensions](http://www.nuget.org/packages/RobustHaven.IntegrationTests.NgExtensions/)  

	Install-Package RobustHaven.IntegrationTests.NgExtensions 



### Sample 001: viewmodel referenced by multiple flows allows disconnected flows to interact as expected

flows can be composites (nested flows) - logins, partial views

	[Test]
	public void Delete_Template()
	{
		var viewModel = new TemplateViewModel {Name = Guid.NewGuid().ToString()};

		ALeaf step1 = FlowFactory.TemplateFlows.CreateTemplate(viewModel);

		ALeaf step2 = FlowFactory.TemplateFlows.DeleteTemplate(viewModel);

		var workflow = new Sequence(step1, step2);

		workflow.Execute(_visitor);
	}
	
	
	
### Sample 001: CreateTemplate.cs

	namespace IntegrationTests.TemplateFlows
	{
		public class CreateTemplate : WebTestLeaf<TemplateViewModel>
		{
			public override void Execute(WebTestContext ctx)
			{
				ConsoleLogger logger = ctx.Logger;
				IWebDriver browser = ctx.Driver; 

				string startingUrl = ctx.ToAbsoluteUrl<TemplateController>(t => t.Index());
				logger.Given("the user is at {0}", startingUrl);
				browser.Navigate().GoToUrl(startingUrl);

				int initialGridTotal = browser.kGridTotal();

				logger.When("the user clicks on 'New Template' button.");
				IWebElement newTemplateBtn = browser.bButton("New Template");
				newTemplateBtn.Click();

				IWebElement templateDiv = browser.FindByNgController(browser, "TemplateController");

				IWebElement nameTxtBx = browser.FindElement(templateDiv, ctx.ById<TemplateViewModel>(t => t.Name));
				browser.NgWaitForFalse(templateDiv, "IsBusyLoadingTemplate");
				nameTxtBx.ShouldBeFocused(browser);


				IWebElement saveChangesBtn = templateDiv.bButton("Save Changes");
				saveChangesBtn.ShouldBeDisabled();


				logger.And("the user types the template name '{0}'", Model.Name);
				nameTxtBx.SendKeys(Model.Name);
				saveChangesBtn.ShouldBeEnabled();
				saveChangesBtn.Click();


				var result = browser.NgScope<TemplateViewModel>(templateDiv, "Data", t => t.Id != 0);
				Mapper.Map(result, Model);
				logger.Then("the user can save a template (Id == {0}).", Model.Id);

		
				browser.NgWaitForFalse(templateDiv, "Data.TemplateForm.$dirty");
				browser.Navigate().GoToUrl(startingUrl);

				int finalGridTotal = browser.kGridTotal();
				finalGridTotal.kGridTotalShouldHaveIncreased(initialGridTotal);
			}
		}
	}

