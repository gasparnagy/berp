Feature: Scenario Outline

Scenario: A cell with 'Examples' turns the sheet to a scenario outline
	Given there is an excel file 'SimpleScenarioOutline.xlsx' with a spreadsheet 'Simple scenario'
		| Given there is <what>     |         |           |
		| When I do <what>          |         |           |
		| Then <what> should happen |         |           |
		| Examples                  |         |           |
		|                           | case    | what      |
		|                           | default | something |
		|                           | special | anything  |
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: SimpleScenarioOutline
		Scenario Outline: Simple scenario
			Given there is <what>     
			When I do <what>
			Then <what> should happen
		Examples:
			| case    | what      |
			| default | something |
			| special | anything  |
		"""

Scenario: Example set title can be specified in the cell after the 'Examples'
	Given there is an excel file 'ScenarioOutline.xlsx' with a spreadsheet 'Simple scenario'
		| Given there is <what> |                      |
		| Examples              | Title of example set |
		|                       | what                 |
		|                       | something            |
		|                       | anything             |
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: ScenarioOutline
		Scenario Outline: Simple scenario
			Given there is <what>     
		Examples: Title of example set
			| what      |
			| something |
			| anything  |
		"""

Scenario: Multiple example sets can be defined with the 'Examples' cell
	Given there is an excel file 'ScenarioOutline.xlsx' with a spreadsheet 'Simple scenario'
		| Given there is <what> |                    |
		| Examples              | First example set  |
		|                       | what               |
		|                       | something          |
		|                       | anything           |
		| Examples              | Second example set |
		|                       | what               |
		|                       | foo                |
		|                       | bar                |
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: ScenarioOutline
		Scenario Outline: Simple scenario
			Given there is <what>     
		Examples: First example set
			| what      |
			| something |
			| anything  |
		Examples: Second example set
			| what |
			| foo  |
			| bar  |
		"""

Scenario: Example set tags can be specified by starting the first cell with '@'
	Given there is an excel file 'ExampleTags.xlsx' with a spreadsheet 'Simple scenario'
		| Given there is <what> |                    |
		| @mytag                |                    |
		| @foo @bar             | @boz               |
		| Examples              | First example set  |
		|                       | what               |
		|                       | something          |
		|                       | anything           |
		| @othertag             |                    |
		| Examples              | Second example set |
		|                       | what               |
		|                       | foo                |
		|                       | bar                |
	When the excel file is processed 
	Then the generated feature is equivalent to
		"""
		Feature: ExampleTags
		Scenario Outline: Simple scenario
			Given there is <what>     
		@mytag @foo @bar @boz
		Examples: First example set
			| what      |
			| something |
			| anything  |
		@othertag
		Examples: Second example set
			| what |
			| foo  |
			| bar  |
		"""
