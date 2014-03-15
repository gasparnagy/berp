Feature: Self Parsing

Scenario: The Berp grammar parser can parse itself
	Given the input source from 'BerpGrammarParserForTest\BerpGrammar.berp'
	When the input source is parsed with the BerpGrammarParserForTest parser 
	Then the parsing should be successful
