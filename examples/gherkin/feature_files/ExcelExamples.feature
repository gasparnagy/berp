@external-source
Feature: ExcelExamples

Scenario Outline: Add two numbers
	Given I have entered <a> into the calculator
	And I have entered <b> into the calculator
	When I press add
	Then the result should be <result> on the screen

Examples: 
	| title        | a  | b  | result |
	| default case | 50 | 70 | 120    |

@source:ExternalExamples.xlsx
Examples: 
	| title        | a  | b  | result |
