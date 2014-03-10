Feature: Specifying feature background

Scenario: Spreadsheets with name 'Background' is processed as feature background
	Given there is an excel file 'Background.xlsx'
	And it has a spreadsheet 'Background'
		| Given there is something from background |
	And it has a spreadsheet 'Simple scenario'
		| Given there is something     |  
		| When I do something          |  
		| Then something should happen |  
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: Background

		Background:
			Given there is something from background

		Scenario: Simple scenario
			Given there is something     
			When I do something          
			Then something should happen
		"""

Scenario: The spreadsheet name for backgound depends on the feature language
	Given there is an excel file 'Background-de.xlsx'
	And the following category is specified: 'language:de-DE'
	And it has a spreadsheet 'Grundlage'
		| Angenommen there is something from background |
	And it has a spreadsheet 'Simple scenario'
		| Angenommen there is something     |  
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		#language:de
		Funktionalität: Background-de

		Grundlage:
			Angenommen there is something from background

		Szenario: Simple scenario
			Angenommen there is something     
		"""
