Feature: Error Messages

Scenario: Expected tokens are listed in the error message
	Given the input source
		"""
		Grammar dummy
		//possible correct variations:
		// Grammar := ... (token: #Definition)
		// Grammar! ... (token: #Production)
		"""
	And the parser is set to stop at first error
	When the input source is parsed with the BerpGrammarParserForTest parser
	Then the parsing should fail
	And the error should contain the expected tokens
		| expected token |
		| #Definition    |
		| #Production    |

Scenario: Expected tokens are listed in the error message (distinct)
	Given the input source
		"""
		Grammar := 
		"""
	And the parser is set to stop at first error
	When the input source is parsed with the BerpGrammarParserForTest parser
	Then the parsing should fail
	And the error should contain the expected tokens
		| expected token |
		| #LParen        |
		| #Token         |
		| #Rule          |

Scenario: Received token is in the error message
	Given the input source
		"""
		Grammar |
		//possible correct variations:
		// Grammar := ... (token: #Definition)
		// Grammar! ... (token: #Production)
		"""
	And the parser is set to stop at first error
	When the input source is parsed with the BerpGrammarParserForTest parser
	Then the parsing should fail
	And the error should contain the received token #AlternateOp

Scenario: Should handle unexpected end of file
	Given the input source
		"""
		//there must be at least one grammar rule in the file
		[
		]
		"""
	And the parser is set to stop at first error
	When the input source is parsed with the BerpGrammarParserForTest parser
	Then the parsing should fail
	And the error should be an unexpected end of file error

Scenario: Should include line numbers into the parsing errors
	Given the input source
		"""
		// comment to increase the line number
		Grammar dummy
		//possible correct variations:
		// Grammar := ... (token: #Definition)
		// Grammar! ... (token: #Production)
		"""
	And the parser is set to stop at first error
	When the input source is parsed with the BerpGrammarParserForTest parser
	Then the parsing should fail
	And the error should contain the line number 2

Scenario: Should include line position numbers into the parsing errors
	Given the input source
		"""
		G dummy
		//possible correct variations:
		// G := ... (token: #Definition)
		// G! ... (token: #Production)
		"""
	And the parser is set to stop at first error
	When the input source is parsed with the BerpGrammarParserForTest parser
	Then the parsing should fail
	And the error should contain the line position number 3
