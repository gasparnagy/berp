Feature: Retry Tests
As a developer
I want to see multiple run results of failing tests
So that I can distinguish between failure types 

Background: 
	Given I have a feature file with a scenario as
		"""
		Feature: Simple Feature
		Scenario: Simple Scenario
			When I do something
		"""

Scenario Outline: Should be able to retry falining tests
	Given all steps are bound and fail
	And the retry mode is configured to '<retry mode>' with repeat count 3
	When I execute the tests
	Then the test 'Simple Scenario' is executed 3 times

Examples: 
	| retry mode |
	| Failing    |
	| All        |

@config
Scenario: Should be able to detect undeterministic failures
	Given the following bindings
		"""
		static int counter = 0;
		[When("I do something")]public void Do()
		{
			if ((counter++ % 2) == 0) throw new Exception("simulated undeterministic error");
		}
		"""
	And the retry mode is configured to 'All' with repeat count 3
	When I execute the tests
	Then the execution summary should contain
		| Total | Randomly failed |
		| 1     | 1               |

