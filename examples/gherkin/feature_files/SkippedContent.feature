Feature: Skipped content

Scenario: Comments can be added by starting the first cell with '#'
	Given there is an excel file 'Comments.xlsx' with a spreadsheet 'Simple scenario'
		| Given there is something     |                            |
		| # comment                    |                            |
		| When I do something          |                            |
		| #                            | this line is a comment too |
		| Then something should happen |                            |
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: Comments
		Scenario: Simple scenario
			Given there is something     
			When I do something          
			Then something should happen
		"""

Scenario: Empty rows are ignored
	Given there is an excel file 'EmptyRows.xlsx' with a spreadsheet 'Simple scenario'
		| Given there is something     |  
		|                              |  
		| When I do something          |  
		|                              |  
		| Then something should happen |  
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: EmptyRows
		Scenario: Simple scenario
			Given there is something     
			When I do something          
			Then something should happen
		"""

Scenario: Helper calculations are ignored if the first two cell is empty
	Given there is an excel file 'Helpers.xlsx' with a spreadsheet 'Simple scenario'
		| Given there is something     |  |                                                           |               |
		|                              |  | this cell is ignored, can be used for helper calculations |               |
		| When I do something          |  |                                                           |               |
		|                              |  | ignored too                                               | and also this |
		| Then something should happen |  |                                                           |               |
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: Helpers
		Scenario: Simple scenario
			Given there is something     
			When I do something          
			Then something should happen
		"""

Scenario: Spreadsheets with name starting with underscore are ignored
	Given there is an excel file 'IgnoredSheet.xlsx'
	And it has a spreadsheet 'Simple scenario'
		| Given there is something     |  
		| When I do something          |  
		| Then something should happen |  
	And it has a spreadsheet '_helper sheet'
		| this content | is  |                                     |
		| ignored      | and | can be used for helper calculations |
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: IgnoredSheet
		Scenario: Simple scenario
			Given there is something     
			When I do something          
			Then something should happen
		"""

Scenario: Hidden spreadsheets are ignored
	Given there is an excel file 'HiddenSheet.xlsx'
	And it has a spreadsheet 'Simple scenario'
		| Given there is something     |  
		| When I do something          |  
		| Then something should happen |  
	And it has a hidden spreadsheet 'Helper sheet'
		| this content | is  |                                     |
		| ignored      | and | can be used for helper calculations |
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: HiddenSheet
		Scenario: Simple scenario
			Given there is something     
			When I do something          
			Then something should happen
		"""

Scenario: Helper calculations are ignored if they are separated from the content by two empty cells
	Given there is an excel file 'Helpers.xlsx' with a spreadsheet 'Simple scenario'
		| Given there is something     |      |       | this cell is ignored, can be used for helper calculations |  |             |               |
		| When I do something with     |      |       |                                                           |  |             |               |
		|                              | Name | Value |                                                           |  | ignored too |               |
		|                              | foo  | bar   |                                                           |  |             | and also this |
		|                              | boo  | boz   |                                                           |  |             |               |
		| Then something should happen |      |       |                                                           |  |             |               |
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: Helpers
		Scenario: Simple scenario
			Given there is something     
			When I do something with
			| Name | Value |
			| foo  | bar   |
			| boo  | boz   |
			Then something should happen
		"""

Scenario: Merged cells are not treated as empty cells
	Given there is an excel file 'Helpers.xlsx' with a spreadsheet 'Simple scenario'
		| Given there is something     |      |        |        |        |  
		| When I do something with     |      |        |        |        |  
		|                              | Name | Value1 | Value2 | Value3 |  
		|                              | foo  | bar    | boz    | bot    |  
		|                              | boo  |        |        | bor    |  
		| Then something should happen |      |        |        |        |  
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: Helpers
		Scenario: Simple scenario
			Given there is something     
			When I do something with
			| Name | Value1 | Value2 | Value3 |
			| foo  | bar    | boz    | bot    |
			| boo  |        |        | bor    |
			Then something should happen
		"""

Scenario: Merged cells (in first column) are not treated as empty cells
	Given there is an excel file 'Helpers.xlsx' with a spreadsheet 'Simple scenario'
		| Given there is something     |      |        |        |
		| When I do something with     |      |        |        |
		|                              | Name | Value1 | Value2 |
		|                              | foo  | bar    | boz    |
		|                              |      | boo    | bor    |
		| Then something should happen |      |        |        |
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: Helpers
		Scenario: Simple scenario
			Given there is something     
			When I do something with
			| Name | Value1 | Value2 | 
			| foo  | bar    | boz    | 
			|      | boo    | bor    | 
			Then something should happen
		"""

Scenario: Merged cells (in first and second column) are not treated as empty cells
	Given there is an excel file 'Helpers.xlsx' with a spreadsheet 'Simple scenario'
		| Given there is something     |      |        |        |
		| When I do something with     |      |        |        |
		|                              | Name | Value1 | Value2 |
		|                              | foo  | bar    | boz    |
		|                              |      |        | bor    |
		| Then something should happen |      |        |        |
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: Helpers
		Scenario: Simple scenario
			Given there is something     
			When I do something with
			| Name | Value1 | Value2 |
			| foo  | bar    | boz    |
			|      |        | bor    |
			Then something should happen
		"""
