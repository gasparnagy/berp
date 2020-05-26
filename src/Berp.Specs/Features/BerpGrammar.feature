Feature: Berp Grammar

Scenario: The Berp grammar parser can parse itself
	Given the input source from 'BerpGrammarParserForTest\BerpGrammar.berp'
	When the input source is parsed with the BerpGrammarParserForTest parser 
	Then the parsing should be successful

Scenario Outline: Lookahead with no skip tokens
	Given the input source
		"""
		[
			Tokens -> #A,#B1,#B2,#C
		]

		Grammar := (B | C)+
		B [-><expected>]:= (#A | #B1 | #B2)
		C := (#A | #C)
		"""
	When the input source is compiled with the BerpGrammarParserForTest parser
	Then the parsing should be successful
Examples: 
	| description       | expected   |
	| single expected   | #B1        |
	| multiple expected | #B1 \| #B2 |
