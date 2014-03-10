Feature: DogFood

Scenario: Eat your own dogfood
	Given all feature files in folder 'C:\git\ExcelGherkin\ExcelGherkin.Specs\Features' are converted to Excel
	And the converted Excel feature files are added to the project
	And the Excel-Gherkin plugin is configured
	And the test project targets .NET 4.5
	And the bindings are taken from 'C:\git\ExcelGherkin\ExcelGherkin.Specs\bin\Debug\ExcelGherkin.Specs.dll'
	When I execute the tests
	Then the execution summary should contain
		| Total | Succeeded |
		| 20    | 20        |
