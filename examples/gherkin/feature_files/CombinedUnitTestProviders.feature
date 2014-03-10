Feature: CombinedUnitTestProviders

Scenario Outline: Should detect steps in SpecFun+NUnit and SpecRun+MsTest mode
	Given I have a feature file with a scenario as
		"""
			Feature: Simple Feature
			Scenario: Simple Scenario
				Given there is something
				When I do something
				Then something should happen
		"""
	And all steps are bound and pass
	And the SpecFlow unit test provider is configured to '<UnitTestProvider>'
	When I execute the tests
	Then the step results should contain
		| step                         | result    |
		| Given there is something     | Succeeded |
		| When I do something          | Succeeded |
		| Then something should happen | Succeeded |

Examples: 
	| UnitTestProvider |
	| SpecRun+NUnit    |
	| SpecRun+MsTest   |
