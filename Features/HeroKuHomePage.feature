Feature: HeroKuHomePage

A short summary of the feature

@smoketest
Scenario: Verify the home page links
	When user is navigated to the home page
	Then the following links should be available
	| LinkNames           |
	| Add/Remove Elements |
	| Frames              |
