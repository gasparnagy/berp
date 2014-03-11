Berp
====

A flexible cross-language parser generator with support for languages without explicit tokenization rules (like Gherkin).

Features:

* generates parser for it's own grammar (the "hello world" for parser generators), see [Berp Grammar](https://github.com/gasparnagy/berp/blob/master/src/Berp/BerpGrammar/BerpGrammar.berp)
* does not generate a lexer/tokenizer, so ideal for languages where tokenization is easy or anyway not really possible
* simple, BNF-like grammar definition
* supports multiple target languages (currently C# and Ruby) with the same grammar (the language generation is specified in template files)
* allows building AST, with AST-building hooks
* supports streamed token reading (tokens can be kept attached to the input stream to avoid unnecessary data transfer and object creation)
* supports context-sensitive tokens, also possible to change the tokenization rules during parsing (e.g. when a ``#language: no`` is encountered)
* supports a special "other" token, that matches to the "anything-else" case, when there is no better match
* support for recursive grammar rules is limited (it parses them up to a certain level only)
* simple, look-ahead rules can be specified
* rules can be marked as production rules to be represented in AST
* allows capturing ignored content tokens (e.g. comments)

Samples:

* [Berp Grammar](https://github.com/gasparnagy/berp/blob/master/src/Berp/BerpGrammar/BerpGrammar.berp)
* [Gherkin Grammar](https://github.com/gasparnagy/berp/blob/master/examples/gherkin/GherkinGrammar.berp)

State: alpha

TODO:

* improve error handling and error reporting
* keep and use original rule position (to support repeating sub-rules within the same rule)
* separating token scanner and token matcher for C# generation
* skip generating labels for non-production rules
* cleanup dependency injection (AST builder, token matcher, ...)
* report error lines for grammar parsing errors
* support recursive grammar rules


