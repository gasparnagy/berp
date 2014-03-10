Feature: SkippingTests issue

Scenario: Repro for SkippingTests issue with 3 threads
	Given I have a feature file with a scenario as
		"""
			Feature: Simple Feature
			Scenario Outline: Simple Scenario
				When I do something

			@tag2
			Examples: Tag2
				| case |
				| x    |
				| x    |
				| x    |
				| x    |
				| x    |
				| x    |
				| x    |
				| x    |
				| x    |

			@tag1
			Examples: Tag1
				| case |
				| x    |
				| x    |
				| x    |
				| x    |
				| x    |
				| x    |
				| x    |
				| x    |
				| x    |

			Examples: Other
				| case |
				| x    |
				| x    |
		"""
	And the following bindings
		"""
			[When("I do something")]public void Do()
			{
				System.Threading.Thread.Sleep(100);
			}
		"""
	And the test thread count is configured to 3
	And test thread 0 is configured to run tests tagged with @tag1
	And test thread 1 is configured to run tests tagged with @tag2
	When I execute the tests
	Then the execution summary should contain
		| Total | Succeeded |
		| 20    | 20        |

Scenario: Special case for SkippingTests issue - affinity set that is not compatible with the thread count
	Given I have a feature file with a scenario as
		"""
			Feature: Simple Feature
			@mytag
			Scenario: Simple Scenario
				When I do something
		"""
	And all steps are bound and pass
	And test thread 1 is configured to run tests tagged with @mytag
	When I execute the tests expecting a tool error
	Then the tool should report a configuration error

