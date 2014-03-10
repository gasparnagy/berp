@reality_check
Feature: Reality checks
	I want to be told the sum of two numbers

Scenario Outline: Can process existing Excel files
	Given the default feature language is <featureLang>
	Given the already existing Excel file '<file>'
	When the excel file is processed
	Then the generated feature should contain
		"""
		<gherkin>
		"""

Examples: 
	| scenaro                   | file                                                     | featureLang | gherkin                                                                                                                             |
	| tax calculation           | RealityCheckFiles\TaxCalculation.feature.xlsx            | en-US       | Scenario: Emplyee without children Given the monthly gross salary of the employee is                                                |
	| complex formulas          | RealityCheckFiles\TaxCalculation-ExcelMagic.feature.xlsx | en-US       | <SZJA>                                                                                                                              |
	| data type formatting (en) | RealityCheckFiles\CellFormats.feature.xlsx               | en-US       | * 12.3 text 1/15/2014 12:00:00 AM True 12.3 12/30/1899 2:34:00 AM * 12.3 text 1/15/2014 12:00:00 AM True 12.3 12/30/1899 2:34:00 AM |
	| data type formatting (de) | RealityCheckFiles\CellFormats.feature.xlsx               | de-AT       | * 12,3 text 15.01.2014 00:00:00 True 12.3 30.12.1899 02:34:00 * 12,3 text 15.01.2014 00:00:00 True 12.3 30.12.1899 02:34:00         |