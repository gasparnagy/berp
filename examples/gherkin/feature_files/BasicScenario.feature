Feature: Basic Scenario

Scenario: Can transform an Excel file with a simple scenario
	Given there is an excel file 'Simple.xlsx' with a spreadsheet 'Simple scenario'
		| Given there is something     |
		| When I do something          |
		| Then something should happen |
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: Simple
		Scenario: Simple scenario
			Given there is something     
			When I do something          
			Then something should happen
		"""

Scenario: Should concatenate subsequent cells
	Given there is an excel file 'SimpleConcat.xlsx' with a spreadsheet 'Simple scenario'
		| When | I | do something |
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: SimpleConcat
		Scenario: Simple scenario
			When I do something          
		"""

Scenario: Should allow different cell types (number, date, time, bool)
	Given there is an excel file 'SimpleTypes.xlsx' with a spreadsheet 'Simple scenario'
		| Given I have entered      | [Number]50          | into the calculator |
		| And the current date is   | [DateTime]5/15/1978 | now                 |
		| And the the statement was | [Boolean]TRUE       | in fact             |
		| And it took               | [TimeSpan]5:15      | to answer           |
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: SimpleTypes
		Scenario: Simple scenario
			Given I have entered 50 into the calculator
			And the current date is 5/15/1978 12:00:00 AM now
			And the the statement was True in fact
			And it took 05:15:00 to answer
		"""

Scenario: Should support formulas
	Given there is an excel file 'Formulas.xlsx' with a spreadsheet 'Simple scenario'
		| Given I have entered      | 50     | into the calculator |
		| And I have entered        | 70     | into the calculator |
		| When I press add          |        |                     |
		| Then the result should be | =B1+B2 | on the screen       |
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: Formulas
		Scenario: Simple scenario
			Given I have entered 50 into the calculator
			And I have entered 70 into the calculator
			When I press add
			Then the result should be 120 on the screen
		"""
	
Scenario: Scenario tags can be specified by starting the first cell with '@'
	Given there is an excel file 'Tags.xlsx' with a spreadsheet 'Simple scenario'
		| @mytag                       |      |
		| @foo @bar                    | @boz |
		| Given there is something     |      |
		| When I do something          |      |
		| Then something should happen |      |
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: Tags
		@mytag @foo @bar @boz
		Scenario: Simple scenario
			Given there is something     
			When I do something          
			Then something should happen
		"""

Scenario: Should process numbers according to the specification language
	Given there is an excel file 'CultureSpecificValues.xlsx' with a spreadsheet 'Simple scenario'
		| Given I have entered      | 50.3   | into the calculator |
	When the excel file is processed in a machine using de-AT culture
	Then the generated feature is equivalent to
		"""
		Feature: CultureSpecificValues
		Scenario: Simple scenario
			Given I have entered 50.3 into the calculator
		"""
	
Scenario: Should process numbers according to the specification language (non-EN)
	Given the default feature language is de-AT
	Given there is an excel file 'CultureSpecificValues-de.xlsx'
	And it has a spreadsheet 'Simple scenario'
		| Angenommen | 50.3 | is | =B1 |
	When the excel file is processed in a machine using en-US culture
	Then the generated feature is equivalent to
		"""
		Funktionalität: CultureSpecificValues-de
		Szenario: Simple scenario
			Angenommen 50,3 is 50,3
		"""
	
