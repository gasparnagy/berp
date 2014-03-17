Feature: Handle Multiple Errors

Scenario: Should handle provide multiple errors by skipping failing token
	Given the input source
		"""
		Grammar | := Dummy
		Dummy := | Dummy
		"""
	When the input source is parsed with the BerpGrammarParserForTest parser
	Then the parsing should fail
	And there should be 2 parsing errors

