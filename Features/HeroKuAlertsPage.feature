Feature: HeroKuJSAlertsPage

A short summary of the feature

Scenario: JSAlert Auto accept
	Given user is navigated to the home page
	When Navigates to the "JavaScript Alerts" link from home page
	And clicks on "Click for JS Alert" button
	Then the alert is displayed with the message "I am a JS Alert"
	And the message "You successfully clicked an alert" is displayed in the page


Scenario: JSAlert confirm alert - accept
	Given user is navigated to the home page
	When Navigates to the "JavaScript Alerts" link from home page
	And clicks on "Click for JS Confirm" button
	Then the alert is displayed with the message "I am a JS Confirm" and "Accept" the alert
	And the message "You clicked: Ok" is displayed in the page


Scenario: JSAlert prompt alert
	Given user is navigated to the home page
	When Navigates to the "JavaScript Alerts" link from home page
	And clicks on "Click for JS Prompt" button
	Then the alert is displayed with the message "I am a JS prompt" input "input message" and "Accept" the alert
	And the message "You entered: input message" is displayed in the page
