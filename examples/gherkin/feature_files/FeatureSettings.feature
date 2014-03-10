Feature: Feature settings

Scenario: Workbook tags are used as feature tags (keywords)
	Given there is an excel file 'FeatureTags.xlsx'
	And the following keywords are specified: 'foo, bar @boz'
	And it has a spreadsheet 'Simple scenario'
		| Given there is something     |  
		| When I do something          |  
		| Then something should happen |  
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		@foo @bar @boz
		Feature: FeatureTags
		Scenario: Simple scenario
			Given there is something     
			When I do something          
			Then something should happen
		"""

Scenario: Workbook title is used as feature title
	Given there is an excel file 'CustomTitle.xlsx'
	And the following workbook title is specified: 'Custom feature title'
	And it has a spreadsheet 'Simple scenario'
		| Given there is something     |  
		| When I do something          |  
		| Then something should happen |  
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: Custom feature title
		Scenario: Simple scenario
			Given there is something     
			When I do something          
			Then something should happen
		"""

Scenario: Gherkin language can be set as a workbook category 'language:en-US'
	Given there is an excel file 'Language.xlsx'
	And the following category is specified: 'language:de-DE'
	And it has a spreadsheet 'Simple scenario'
		| Angenommen there is something |
		| Und something else too        |
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		#language:de
		Funktionalität: Language
		Szenario: Simple scenario
			Angenommen there is something     
			Und something else too
		"""
