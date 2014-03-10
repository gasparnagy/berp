Feature: Step Arguments

Scenario: Table arguments can be provided by skipping the first column
	Given there is an excel file 'TableArg.xlsx' with a spreadsheet 'Simple scenario'
		| When I do something with     |      |       |
		|                              | Name | Value |
		|                              | foo  | bar   |
		|                              | boo  | boz   |
		| Then something should happen |      |       |
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: TableArg
		Scenario: Simple scenario
			When I do something with
			| Name | Value |
			| foo  | bar   |
			| boo  | boz   |
			Then something should happen
		"""

Scenario: Multi-line text arguments can be provided as a single cell in the second column
	Given there is an excel file 'TableArg.xlsx' with a spreadsheet 'Simple scenario'
		| When I do something with     |                                             |
		|                              | this is a longer text[BR]with a second line |
		| Then something should happen |                                             |
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: TableArg
		Scenario: Simple scenario
			When I do something with
			'''
			this is a longer text
			with a second line
			'''
			Then something should happen
		"""
