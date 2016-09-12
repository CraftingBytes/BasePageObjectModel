Feature: HomePage

Scenario: The title on the home page is showing
	Given I am on the home page
	Then the title should be showing

Scenario: Navigating from the home page to the second page
	Given I am on the home page
	When I click on the second page link
	Then I should be on the second page
