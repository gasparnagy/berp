Feature: Capture test step results in SpecFun+NUnit/MsTest mode

Background: 
	Given I have a feature file with a scenario as
		"""
		Feature: Simple Feature
		Scenario: Simple Scenario
			Given there is something
			When I do something
			Then something should happen
		"""

Scenario: Should detect steps in SpecFun+NUnit mode
	Given all steps are bound and pass
	And the SpecFlow unit test provider is configured to 'SpecRun+NUnit'
	When I execute the tests
	Then the step results should contain
		| step                         | result    |
		| Given there is something     | Succeeded |
		| When I do something          | Succeeded |
		| Then something should happen | Succeeded |

Scenario: Should detect steps in SpecFun+MsTest mode
	Given all steps are bound and pass
	And the SpecFlow unit test provider is configured to 'SpecRun+MsTest'
	When I execute the tests
	Then the step results should contain
		| step                         | result    |
		| Given there is something     | Succeeded |
		| When I do something          | Succeeded |
		| Then something should happen | Succeeded |

