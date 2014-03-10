@ignore
Feature: SpecFlowFeature1

Scenario: Should be able to execute a simple passing scenario
	Given I have a feature file with a scenario as
		"""
		Feature: Simple Feature
		Scenario: Simple Scenario
			Given there is something
			When I do something
			Then something should happen
		"""
	And all steps are bound and pass
	When I execute the tests
	Then the execution summary should contain
		| Total | Succeeded |
		| 1     | 1         |

Scenario: Test1
	Given the feature file 'C:\Temp\Dummy.feature' added to the project
	And all steps are bound and pass
	When I execute the tests
	Then the execution summary should contain
		| Total | Succeeded |
		| 1     | 1         |

Scenario: Test2
	Given the feature file 'C:\Temp\Dummy.feature.xlsx' added to the project
	And the Excel-Gherkin plugin is configured
	And all steps are bound and pass
	When I execute the tests
	Then the execution summary should contain
		| Total | Succeeded |
		| 1     | 1         |
