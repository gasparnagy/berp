Feature: TeamCityOutput

Scenario: Ignored test after failing test causing problem in the emitted TeamCity test results
	Given I have a test project 'SpecRun.TestProject'
	And I have a feature file with a scenario as
		"""
			Feature: Simple Feature

			Scenario: Scenario1
				When I do something

			@ignore
			Scenario: Scenario2
				When I do something

			Scenario: Scenario3
				When I do something
		"""
	And all steps are bound and fail
	And there is a specrun configuration file 'Default.srprofile' as
		"""
		<?xml version="1.0" encoding="utf-16"?>
		<TestProfile xmlns="http://www.specrun.com/schemas/2011/09/TestProfile">
		  <TestAssemblyPaths>
			<TestAssemblyPath>SpecRun.TestProject.dll</TestAssemblyPath>
		  </TestAssemblyPaths>
		  <Execution retryFor="None" />
		</TestProfile>
		"""
	When I execute the tests through the console runner with
         | Setting     | Value             |
         | ConfigFile  | Default.srprofile |
         | BuildServer | TeamCity          |
	Then the console runner output should contain "##teamcity[testFinished name='Scenario1 (in Simple Feature)'" before "##teamcity[testStarted name='Scenario2 (in Simple Feature)'"

Scenario: Ignored test after failing test causing problem in the emitted TeamCity test results (only ignores)
	Given I have a test project 'SpecRun.TestProject'
	And I have a feature file with a scenario as
		"""
			Feature: Simple Feature

			@ignore
			Scenario: Scenario1
				When I do something

			@ignore
			Scenario: Scenario2
				When I do something
		"""
	And all steps are bound and fail
	And there is a specrun configuration file 'Default.srprofile' as
		"""
		<?xml version="1.0" encoding="utf-16"?>
		<TestProfile xmlns="http://www.specrun.com/schemas/2011/09/TestProfile">
		  <TestAssemblyPaths>
			<TestAssemblyPath>SpecRun.TestProject.dll</TestAssemblyPath>
		  </TestAssemblyPaths>
		  <Execution retryFor="None" />
		</TestProfile>
		"""
	When I execute the tests through the console runner with
         | Setting     | Value             |
         | ConfigFile  | Default.srprofile |
         | BuildServer | TeamCity          |
	Then the console runner output should contain "##teamcity[testFinished name='Scenario1 (in Simple Feature)'" before "##teamcity[testStarted name='Scenario2 (in Simple Feature)'"