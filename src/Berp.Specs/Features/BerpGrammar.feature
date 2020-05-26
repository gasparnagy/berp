Feature: Berp Grammar

Scenario: The Berp grammar parser can parse itself
	Given the input source from 'BerpGrammarParserForTest\BerpGrammar.berp'
	When the input source is parsed with the BerpGrammarParserForTest parser 
	Then the parsing should be successful

Scenario: Lookahead with no skip tokens
	Given the input source
		"""
		[
			Tokens -> #A,#B,#C
		]

		Grammar := (B | C)+
		B [->#B]:= (#A | #B)
		C := (#A | #C)
		"""
	When the input source is compiled with the BerpGrammarParserForTest parser
	Then the parsing should be successful
