Feature: Generation


Scenario Outline: Generates parser to a target language
	Given there is a complex grammar
	When the parser generation is performed using '<template>'
	Then then the generation should be successful
Examples: 
	| template     |
	| CSharp.razor |
	| Ruby.razor   |