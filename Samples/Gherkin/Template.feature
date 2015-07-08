@template @InTesting
Feature:	Template
Background:
	Given an authenticated user
	 And the user is at the Templates home page

Scenario:	User creates a new template with a name
	When user clicks on 'New Template' button
	Then template creation is started
	 And user can save template with a name

Scenario:	User marks a template as a favorite
	Given a template not currently marked as a favorite
	When user clicks the favorite icon
	Then the template is marked as a favorite
	 And user can select the favorite when starting a new xyz.

Scenario:	User unmarks a template as a favorite
	Given a template currently marked as a favorite
	When user clicks favorite icon
	Then the template is unmarked as a favorite
	 And user cannot select the favorite when starting a new xyz.