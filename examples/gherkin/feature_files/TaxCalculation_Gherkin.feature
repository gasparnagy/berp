@gherkin
Feature: Tax Calculation (Gherkin)

Scenario: Emplyee without children
	Given the monthly gross salary of the employee is 200000
	And there are no children in the family
	When the tax is calculated
	Then the monthly net salary of the employee should be 131000


Scenario Outline: Emplyee without children (SO)
	Given the monthly gross salary of the employee is <gross>
	And there are no children in the family
	When the tax is calculated
	Then the monthly net salary of the employee should be <net>

Examples: 
	| gross  | net    |
	| 100000 | 65500  |
	| 200000 | 131000 |
	| 300000 | 196500 |

Scenario Outline: Employee wo children - details
	Given the monthly gross salary of the employee is <gross>
	And there are no children in the family
	When the tax is calculated
	Then the monthly net salary of the employee should be <net>
	And the following taxes are paid by the employee
		| Social Security Contributions | Income Tax   |
		| <contributions>               | <income tax> |

Examples: 
	| gross  | net    | contributions | income tax |
	| 100000 | 65500  | 18000         | 16000      |
	| 200000 | 131000 | 37000         | 32000      |
	| 300000 | 196500 | 55500         | 48000      |

