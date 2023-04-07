Feature: HeroKuCheckBoxPage

A short summary of the feature

Scenario Outline: Verify the checkbox initial status
	Given user is navigated to the home page
	When Navigates to the "checkboxes" link from home page
	Then verify the status "<checkboxStatus>" of the checkbox "<checkboxName>"
Examples: 
	| checkboxName | checkboxStatus |
	| checkbox 1   | unchecked      |
	| checkbox 2   | checked        |

Scenario Outline: Checkbox functionalities
	Given user is navigated to the home page
	When Navigates to the "checkboxes" link from home page
	And set the status of the checkbox "<checkboxName>" as "<checkboxStatus>"
	Then verify the status "<checkboxStatus>" of the checkbox "<checkboxName>"
Examples: 
	| checkboxName | checkboxStatus |
	| checkbox 1   | checked        |
	| checkbox 2   | unchecked      |