Feature: HomePage

Scenario: The title on the home page is showing
	Given I am on the home page
	Then the title should be showing

Scenario: Navigating from the home page to the second page
	Given I am on the home page
	When I click on the second page link
	Then I should be on the second page

Scenario: Create the something
	Given I am on the create page
	When I fill out the form with all kinds of characters
	| Label       | Value                              |
	| FullName    | Scott Reed                         |
	| FullAddress | 3350 Brennan Dr, Raleigh NC, 27613 |
	And I click Create
	Then I should see the new value in the list