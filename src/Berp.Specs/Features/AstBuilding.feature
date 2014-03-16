Feature: AST Building

Scenario: Build a simple AST (wihtout repeating elements)
	Given the input source
		"""
		Grammar := RuleA RuleB
		"""
	When the input source is parsed with the BerpGrammarParserForTest parser
	Then the parsing should be successful
	And the created AST should be
		"""
		[Grammar
			[RuleDefinition
				#Rule:'Grammar'
				#Definition:':='
				[RuleDefinitionElement
					#Rule:'RuleA'
				]
				[RuleDefinitionElement
					#Rule:'RuleB'
				]
				#EOL
			]
			#EOF
		]
		"""
