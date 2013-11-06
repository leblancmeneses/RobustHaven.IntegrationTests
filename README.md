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
	var templateDiv = browser.FindElementByNgController("TemplateController");
	Browser.NgWaitFor(templateDiv, "scope.Data.Id != 0");
	var result = browser.NgScope<TemplateViewModel>(templateDiv, "Data");
	
	
[Kendo Extensions](http://www.nuget.org/packages/RobustHaven.IntegrationTests.KendoExtensions/)  

	Install-Package RobustHaven.IntegrationTests.KendoExtensions 
	
Usage Samples:

	// there are constructor overloads for search context
	var grid = new KendoGrid(browser);
	int initialGridTotal = grid.Total();
	grid.Select(4);
	
	templateGrid.DoPerPage(i =>
	{		
		ctx.Delay();
	});
			
	var tableRow = templateGrid.GetTableRowByViewModelId(Model.Id);
	var actionsDropDown = new KendoDropDownList(ctx.Browser, tableRow);
	actionsDropDown.Select("Delete");
	
	

	
# How do you start?
	
###  Step 1: Gherkin + balsamiq mockups

	@Development
	Feature:	Template
	Background:
		Given an authenticated user
		 And the user is at the Templates home page

	Scenario:	User creates a new template with a name
		When user clicks on 'New Template' button
		Then template creation is started
		 And user can save template with a name
		 
	Scenario:	User creates a new template with case information
		When user adds valid partial case information to a template
		Then 'required' fields should not fail validation
		  
	Scenario:	User creates a new template with case information error
		When user adds invalid partial case information to a template
		Then validation errors should prevent case information from being saved


	Scenario:	User edits an existing template's name
		When user selects on 'View' under the Actions menu
		Then user can change the template's name
			And user is notified of modification success
		  
	Scenario:	User edits an existing template's case information 
		When user changes valid partial case information on the template
		Then 'required' fields should not fail validation

	Scenario:	User edits an existing template's case information with errors
		When user changes invalid partial case information on the template
		Then validation errors should prevent case information from being saved


	Scenario:	User deletes an existing template
		When user selects Delete under the Actions menu
		Then user is notified of deletion
		 And template should no longer appear in the templates grid


	Scenario:	User invokes a new envelope using a template from action menu
		When user selects on 'File' under the Actions menu
		Then a new envelope is started
		 And fields are populated with default values from the template
		 
		 
	Scenario:	Discoverable favorite feature for templates
		Given there are zero favorited templates for user
		 And user is at My Filings page
		When user selects the dropdown for new filings
		Then they will get a prompt 'You have not favorited any templates.'
		 And 'templates' text is link to My Templates page
		 
		 
	Scenario:	User marks a template as a favorite
		 Given a template not currently marked as a favorite
		 When user clicks the favorite icon
		 Then the template is marked as a favorite
		 And user can select the favorite when starting a new case from My Filings
		 
	Scenario:	User unmarks a template as a favorite
		Given a template currently marked as a favorite
		 When user clicks favorite icon
		 Then the template is unmarked as a favorite
		 And user cannot select the favorite when starting a new case from My Filings



![grid](https://raw.github.com/leblancmeneses/RobustHaven.IntegrationTests/master/Docs/grid.jpg)
![form](https://raw.github.com/leblancmeneses/RobustHaven.IntegrationTests/master/Docs/form.jpg)


### Step 2: nunit - no additional pluggins required for visual studio


	namespace IntegrationTests.Flows
	{
		[TestFixture]
		[Category("template")]
		[Feature("Template")]
		public class TemplateFeatureTests : FeatureTestBase
		{
			public override void Background()
			{
				Visitor.Context.Given("an authenticated user");
				Visitor.Context.And(() =>
				{
					var startingUrl = Visitor.Context.ToAbsoluteUrl<TemplateController>(t => t.Index());
					Visitor.Context.Browser.Navigate().GoToUrl(startingUrl);
				}, "the user is at the Templates home page");
			}


			[Test]
			[Scenario("User creates a new template with a name")]
			public void User_creates_a_new_template_with_a_name()
			{
				var viewModel = new TemplateViewModel { Name = Guid.NewGuid().ToString() };

				var createFlow = new CreateTemplate { Model = viewModel};

				createFlow.Execute(Visitor);
			}

			[Test]
			[Scenario("User creates a new template with case information")]
			public void User_creates_a_new_template_with_case_information()
			{
				var templateViewModel = new TemplateViewModel { Name = Guid.NewGuid().ToString() };

				var caseViewModel = new CaseViewModel() { Location_Id = 1, CaseNumber = "12345" };

				var createFlow = new CreateTemplate
				{
					Model = templateViewModel,
					CaseView = new CaseViewEditorTemplate()
					{
						IsValidInput = true,
						IsPartiallyFilled = true,
						ActivityMode = CaseActivityMode.ModifyTemplate,
						Model = caseViewModel
					}
				};

				createFlow.Execute(Visitor);
			}

			[Test]
			[Scenario("User creates a new template with case information error")]
			public void User_creates_a_new_template_with_case_information_error()
			{
				var templateViewModel = new TemplateViewModel { Name = Guid.NewGuid().ToString() };

				var caseViewModel = new CaseViewModel() { Location_Id = 1, CaseNumber = "abc"};

				var createFlow = new CreateTemplate
				{
					Model = templateViewModel,
					CaseView = new CaseViewEditorTemplate()
					{
						IsValidInput = false,
						IsPartiallyFilled = true,
						ActivityMode = CaseActivityMode.ModifyTemplate,
						Model = caseViewModel
					}
				};

				createFlow.Execute(Visitor);
			}

			[Test]
			[Scenario("User edits an existing template's name")]
			public void User_edits_an_existing_templates_name()
			{
				var templateViewModel = new TemplateViewModel { Name = Guid.NewGuid().ToString() };
				
				var create = new CreateTemplate{Model = templateViewModel};

				var change = new ExecActionView(context =>
				{ 
					templateViewModel.Name = string.Format("changed {0}", templateViewModel.Name); 
				});

				var edit = new EditTemplate{Model = templateViewModel};

				var workflow = new Sequence(create, change)
								.Sequence(edit);

				workflow.Execute(Visitor);
			}

			[Test]
			[Scenario("User edits an existing template's case information")]
			public void User_edits_an_existing_templates_case_information()
			{
				var templateViewModel = new TemplateViewModel { Name = Guid.NewGuid().ToString() };
				var caseViewModel = new CaseViewModel() { Location_Id = 1, CaseNumber = "12345" };
				var caseView = new CaseViewEditorTemplate()
				{
					IsValidInput = true,
					IsPartiallyFilled = true,
					ActivityMode = CaseActivityMode.ModifyTemplate,
					Model = caseViewModel
				};
				var create = new CreateTemplate
				{
					Model = templateViewModel,
					CaseView = caseView
				};
				var change = new ExecActionView(context =>
				{
					templateViewModel.Name = string.Format("changed {0}", templateViewModel.Name);
					caseViewModel.Title = "changed abc";
				});
				var edit = new EditTemplate
				{
					Model = templateViewModel,
					CaseView = caseView
				};
				var workflow = new Sequence(create, change)
								.Sequence(edit);
				workflow.Execute(Visitor);
			}

			[Test]
			[Scenario("User edits an existing template's case information with errors")]
			public void User_edits_an_existing_templates_case_information_with_errors()
			{
				var templateViewModel = new TemplateViewModel { Name = Guid.NewGuid().ToString() };
				var caseViewModel = new CaseViewModel() { Location_Id = 1, CaseNumber = "12345" };
				var caseView = new CaseViewEditorTemplate()
				{
					IsValidInput = true,
					IsPartiallyFilled = true,
					ActivityMode = CaseActivityMode.ModifyTemplate,
					Model = caseViewModel
				};
				var create = new CreateTemplate
				{
					Model = templateViewModel,
					CaseView = caseView
				};
				var change = new ExecActionView(context =>
				{
					templateViewModel.Name = string.Format("changed {0}", templateViewModel.Name);
					caseViewModel.CaseNumber = "abc";
					caseView.IsValidInput = false;
				});
				var edit = new EditTemplate
				{
					Model = templateViewModel,
					CaseView = caseView
				};
				var workflow = new Sequence(create, change)
								.Sequence(edit);
				workflow.Execute(Visitor);
			}

			[Test]
			[Scenario("User deletes an existing template")]
			public void User_deletes_an_existing_template()
			{
				var viewModel = new TemplateViewModel {Name = Guid.NewGuid().ToString()};

				var step1 = new CreateTemplate { Model = viewModel };
				var step2 = new DeleteTemplate { Model = viewModel };

				var workflow = new Sequence(step1, step2);

				workflow.Execute(Visitor);
			}

			[Test]
			[Scenario("User invokes a new envelope using a template from action menu")]
			public void User_invokes_a_new_envelope_using_a_template_from_action_menu()
			{
				var templateViewModel = new TemplateViewModel { Name = Guid.NewGuid().ToString() };
				var caseViewModel = new CaseViewModel() { Location_Id = 1, CaseNumber = "12345" };
				var caseView = new CaseViewEditorTemplate()
				{
					IsValidInput = true,
					IsPartiallyFilled = true,
					ActivityMode = CaseActivityMode.ModifyTemplate,
					Model = caseViewModel
				};

				var create = new CreateTemplate
				{
					Model = templateViewModel,
					CaseView = caseView
				};

				var step2 = new UseTemplate { Model = templateViewModel, CaseViewModel = caseViewModel };
				var workflow = new Sequence(create, step2);

				workflow.Execute(Visitor);
			}

			[Test]
			[Scenario("Discoverable favorite feature for templates")]
			public void Discoverable_favorite_feature_for_templates()
			{
				var step = new DiscoverableTemplateFeature();
				step.Execute(Visitor);
			}

			[Test]
			[Scenario("User marks a template as a favorite")]
			public void User_marks_a_template_as_a_favorite()
			{
				var viewModel = new TemplateViewModel { Name = Guid.NewGuid().ToString() };

				// Create a template
				var step1 = new CreateTemplate { Model = viewModel };

				// Mark above template as favorite
				var step2 = new FavoriteTemplate { Model = viewModel, MarkAsFavorite = true };

				var workflow = new Sequence(step1, step2);
				workflow.Execute(Visitor);
			}

			[Test]
			[Scenario("User unmarks a template as a favorite")]
			public void User_unmarks_a_template_as_a_favorite()
			{
				var viewModel = new TemplateViewModel { Name = Guid.NewGuid().ToString() };

				var create = new CreateTemplate { Model = viewModel };

				var mark = new FavoriteTemplate { Model = viewModel, MarkAsFavorite = true };

				var unmark = new FavoriteTemplate { Model = viewModel, MarkAsFavorite = false };

				var workflow = new Sequence(create, mark).Sequence(unmark);
				workflow.Execute(Visitor);
			}
		}
	}

	
	
### Step 3: (real work sample) DiscoverableTemplateFeature.cs

	public class DiscoverableTemplateFeature : WebTestLeaf<object>
	{
		public override void Execute(WebTestContext ctx)
		{
			ctx.Given(() =>
			{
				var templateGrid = new KendoGrid(ctx.Browser);
				ctx.Browser.WaitFor("isBrowserBusy() === false");

				templateGrid.DoPerPage(i =>
				{
					IWebElement item;
					do
					{
						var items = ctx.Browser.FindElements(By.CssSelector("a.IsFavorite i.icon-star"));
						item  = items.FirstOrDefault();
						if (item != null)
						{
							item.Click();
						}

						ctx.Delay();

					} while (item != null);
				});
			}, "there are zero favorited templates for user");


			ctx.And(() =>
			{
				var myFilingsHomePageUrl = ctx.ToAbsoluteUrl<EnvelopeController>(t => t.Index());
				ctx.Browser.Navigate().GoToUrl(myFilingsHomePageUrl);
				ctx.Delay();
			}, "user is at My Filings page");


			ctx.When(() =>
			{
				var favoriteTemplatesBtn = ctx.Browser.FindElement(By.CssSelector("div#NewCaseBtnGroup button.btn.dropdown-toggle"));
				favoriteTemplatesBtn.Click();

				ctx.Delay();
			}, "user selects the dropdown for new filings");


			var msg = "You have not favorited any templates.";
			IWebElement li = null;
			ctx.Then(() =>
				{
					var favoriteTemplatesList = ctx.Browser.FindElements(By.CssSelector("div#NewCaseBtnGroup ul.dropdown-menu li"));
					
					ctx.Verified(() => li = favoriteTemplatesList.Single(), "only one li element existed");

					var text = ctx.Browser.ScriptQuery<string>("return $(arguments[0]).text();", li);

					var verified = string.Format("prompt text should be '{0}'", msg);
					ctx.Verified(() => Assert.AreEqual(text.Trim(), msg), verified);

				}, "they will get a prompt '{0}'", msg);


			ctx.And(() =>
			{
				var href = ctx.Browser.ScriptQuery<string>("return $('a', arguments[0]).attr('href');", li);
				href = href.StartsWith("/") ? string.Format("{0}{1}", ctx.BaseUrl, href) : href;
				var templateHomePage = ctx.ToAbsoluteUrl<TemplateController>(t => t.Index());
				var verified = string.Format("template link is should be '{0}'", templateHomePage);
				ctx.Verified(() => Assert.AreEqual(href, templateHomePage), verified);

			}, "'templates' text is link to My Templates page");
		}
	}

	
### Step 4: red/green/refactor
![form](https://raw.github.com/leblancmeneses/RobustHaven.IntegrationTests/master/Docs/nunit.jpg)
