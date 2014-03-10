@excel-examples @external-source
Feature: Tax Calculation (Excel Examples)

Scenario Outline: Emplyee without children
	Given the monthly gross salary of the employee is <gross>
	And there are no children in the family
	When the tax is calculated
	Then the monthly net salary of the employee should be <net>

@source:TaxCalculation_ExcelExamples.xlsx
Examples: 
	| gross  | net    |

Scenario Outline: Employee wo children - details
	Given the monthly gross salary of the employee is <gross>
	And there are no children in the family
	When the tax is calculated
	Then the monthly net salary of the employee should be <net>
	And the following taxes are paid by the employee
		| Social Security Contributions | Income Tax   |
		| <contributions>               | <income tax> |

@source:TaxCalculation_ExcelExamples.xlsx
Examples: 
	| gross  | net    | contributions | income tax |
	| 100000 | 65500  | 18000         | 16000      |

