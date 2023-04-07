Feature: HeroKuIFramePage

A short summary of the feature

Scenario: Input values in the iFrame
	Given user is navigated to the home page
	When Navigates to the "Frames" link from home page
	And Navigates to the "iFrame" link from home page
	And Selects new document from the File menu
	And inputs the document string in the editor
	"""
	This is a document string
	that is being entered newly
	"""

