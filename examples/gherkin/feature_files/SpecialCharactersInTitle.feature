Feature: Special characters in title

Scenario: Should support quotes in the title
	Given I have a feature file with a scenario as
         """
			Feature: Simple Feature

			Scenario: Quotes test single ‘, double “, back ` end-of-title
				When I do something
         
			Scenario: Quotes test single: Deselecting the ‘Show’ option end-of-title
				When I do something
         """
	And all steps are bound and pass
	When I execute the tests
	Then the execution summary should contain
		| Total | Succeeded |
		| 2     | 2         |
