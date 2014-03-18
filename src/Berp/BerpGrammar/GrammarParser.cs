using System;
using System.Collections.Generic;
namespace Berp.BerpGrammar
{
	public enum TokenType
	{
		None,
EOF,
Rule,
Token,
EOL,
Production,
Definition,
LParen,
RParen,
LBracket,
RBracket,
Arrow,
AlternateOp,
AnyMultiplier,
OneOrMoreMultiplier,
OneOrZeroMultiplier,
Comma,
Number,
Other,
	}

	public enum RuleType
	{
		None,
_EOF, // #EOF
_Rule, // #Rule
_Token, // #Token
_EOL, // #EOL
_Production, // #Production
_Definition, // #Definition
_LParen, // #LParen
_RParen, // #RParen
_LBracket, // #LBracket
_RBracket, // #RBracket
_Arrow, // #Arrow
_AlternateOp, // #AlternateOp
_AnyMultiplier, // #AnyMultiplier
_OneOrMoreMultiplier, // #OneOrMoreMultiplier
_OneOrZeroMultiplier, // #OneOrZeroMultiplier
_Comma, // #Comma
_Number, // #Number
_Other, // #Other
Grammar, // Grammar! := Settings? RuleDefinition+
RuleDefinition, // RuleDefinition! := #Rule #Production? #Definition RuleDefinitionElement+ #EOL
RuleDefinitionElement, // RuleDefinitionElement! := RuleDefinitionElement_Core LookAhead? RuleDefinitionElement_Multiplier?
RuleDefinitionElement_Core, // RuleDefinitionElement_Core := (AlternateElement | TokenElement | RuleElement | GroupElement)
RuleDefinitionElement_Multiplier, // RuleDefinitionElement_Multiplier := (#AnyMultiplier | #OneOrMoreMultiplier | #OneOrZeroMultiplier)
AlternateElement, // AlternateElement! := #LParen[#Token|#Rule-&gt;#AlternateOp] AlternateElementBody #RParen
AlternateElementBody, // AlternateElementBody := AlternateElementItem (#AlternateOp AlternateElementItem)*
AlternateElementItem, // AlternateElementItem := (#Rule | #Token)
GroupElement, // GroupElement! := #LParen RuleDefinitionElement+ #RParen
TokenElement, // TokenElement := #Token
RuleElement, // RuleElement := #Rule
LookAhead, // LookAhead! := #LBracket LookAheadTokenList? #Arrow LookAheadTokenList #RBracket
LookAheadTokenList, // LookAheadTokenList! := #Token (#AlternateOp #Token)*
Settings, // Settings! := (#LBracket #EOL) Parameter* (#RBracket #EOL)
Parameter, // Parameter! := #Rule #Arrow ParameterValue (#Comma ParameterValue)* #EOL
ParameterValue, // ParameterValue := (#Rule | #Token)
	}

    public partial class ParserError
    {
        public string StateComment { get; private set; }

        public Token ReceivedToken { get; private set; }
        public string[] ExpectedTokenTypes { get; private set; }

        public ParserError(Token receivedToken, string[] expectedTokenTypes, string stateComment)
        {
            this.ReceivedToken = receivedToken;
            this.ExpectedTokenTypes = expectedTokenTypes;
            this.StateComment = stateComment;
        }
    }

    public partial class ParserException : Exception
    {
        private ParserError[] errors = new ParserError[0];

        public ParserError[] Errors { get { return errors; } }

        public ParserException() { }
        public ParserException(string message) : base(message) { }
        public ParserException(string message, Exception inner) : base(message, inner) { }

        public ParserException(ParserMessageProvider messageProvider, params ParserError[] errors) 
			: base(messageProvider.GetDefaultExceptionMessage(errors))
        {
            if (errors != null)
                this.errors = errors;
        }
    }

    public class Parser
    {
		public bool StopAtFirstError { get; set;}
		public TokenMatcher TokenMatcher { get; private set; }
		public ParserMessageProvider ParserMessageProvider { get; private set; }

		class ParserContext
		{
			public TokenScanner TokenScanner { get; set; }
			public AstBuilder Builder { get; set; }
			public Queue<Token> TokenQueue { get; set; }
			public List<ParserError> Errors { get; set; }
		}

		public Parser() : this(new TokenMatcher(), new ParserMessageProvider())
		{
		}

		public object Parse(TokenScanner tokenScanner)
		{
			return Parse(tokenScanner, new AstBuilder());
		}

		public Parser(TokenMatcher tokenMatcher, ParserMessageProvider parserMessageProvider)
		{
			this.TokenMatcher = tokenMatcher;
			this.ParserMessageProvider = parserMessageProvider;
		}

        public object Parse(TokenScanner tokenScanner, AstBuilder astBuilder)
		{
			var context = new ParserContext
			{
				TokenScanner = tokenScanner,
				Builder = astBuilder,
				TokenQueue = new Queue<Token>(),
				Errors = new List<ParserError>()
			};

			StartRule(context, RuleType.Grammar);
            int state = 0;
            Token token;
            do
			{
				token = ReadToken(context);
				state = MatchToken(state, token, context);
            } while(!token.IsEOF);

			if (context.Errors.Count > 0)
			{
				throw new ParserException(ParserMessageProvider, context.Errors.ToArray());
			}

			if (state != 60)
			{
				throw new InvalidOperationException("One of the grammar rules expected #EOF explicitly.");
			}

			EndRule(context, RuleType.Grammar);
			return GetResult(context);
		}

		void Build(ParserContext context, Token token)
		{
			context.Builder.Build(token);
		}

		void StartRule(ParserContext context, RuleType ruleType)
		{
			context.Builder.StartRule(ruleType);
		}

		void EndRule(ParserContext context, RuleType ruleType)
		{
			context.Builder.EndRule(ruleType);
		}

		object GetResult(ParserContext context)
		{
			return context.Builder.GetResult();
		}

		Token ReadToken(ParserContext context)
		{
			return context.TokenQueue.Count > 0 ? context.TokenQueue.Dequeue() : context.TokenScanner.Read();
		}

		int MatchToken(int state, Token token, ParserContext context)
		{
			int newState;
			switch(state)
			{
				case 0:
					newState = MatchTokenAt_0(token, context);
					break;
				case 1:
					newState = MatchTokenAt_1(token, context);
					break;
				case 2:
					newState = MatchTokenAt_2(token, context);
					break;
				case 3:
					newState = MatchTokenAt_3(token, context);
					break;
				case 4:
					newState = MatchTokenAt_4(token, context);
					break;
				case 5:
					newState = MatchTokenAt_5(token, context);
					break;
				case 6:
					newState = MatchTokenAt_6(token, context);
					break;
				case 7:
					newState = MatchTokenAt_7(token, context);
					break;
				case 8:
					newState = MatchTokenAt_8(token, context);
					break;
				case 9:
					newState = MatchTokenAt_9(token, context);
					break;
				case 10:
					newState = MatchTokenAt_10(token, context);
					break;
				case 11:
					newState = MatchTokenAt_11(token, context);
					break;
				case 12:
					newState = MatchTokenAt_12(token, context);
					break;
				case 13:
					newState = MatchTokenAt_13(token, context);
					break;
				case 14:
					newState = MatchTokenAt_14(token, context);
					break;
				case 15:
					newState = MatchTokenAt_15(token, context);
					break;
				case 16:
					newState = MatchTokenAt_16(token, context);
					break;
				case 17:
					newState = MatchTokenAt_17(token, context);
					break;
				case 18:
					newState = MatchTokenAt_18(token, context);
					break;
				case 19:
					newState = MatchTokenAt_19(token, context);
					break;
				case 20:
					newState = MatchTokenAt_20(token, context);
					break;
				case 21:
					newState = MatchTokenAt_21(token, context);
					break;
				case 22:
					newState = MatchTokenAt_22(token, context);
					break;
				case 23:
					newState = MatchTokenAt_23(token, context);
					break;
				case 24:
					newState = MatchTokenAt_24(token, context);
					break;
				case 25:
					newState = MatchTokenAt_25(token, context);
					break;
				case 26:
					newState = MatchTokenAt_26(token, context);
					break;
				case 27:
					newState = MatchTokenAt_27(token, context);
					break;
				case 28:
					newState = MatchTokenAt_28(token, context);
					break;
				case 29:
					newState = MatchTokenAt_29(token, context);
					break;
				case 30:
					newState = MatchTokenAt_30(token, context);
					break;
				case 31:
					newState = MatchTokenAt_31(token, context);
					break;
				case 32:
					newState = MatchTokenAt_32(token, context);
					break;
				case 33:
					newState = MatchTokenAt_33(token, context);
					break;
				case 34:
					newState = MatchTokenAt_34(token, context);
					break;
				case 35:
					newState = MatchTokenAt_35(token, context);
					break;
				case 36:
					newState = MatchTokenAt_36(token, context);
					break;
				case 37:
					newState = MatchTokenAt_37(token, context);
					break;
				case 38:
					newState = MatchTokenAt_38(token, context);
					break;
				case 39:
					newState = MatchTokenAt_39(token, context);
					break;
				case 40:
					newState = MatchTokenAt_40(token, context);
					break;
				case 41:
					newState = MatchTokenAt_41(token, context);
					break;
				case 42:
					newState = MatchTokenAt_42(token, context);
					break;
				case 43:
					newState = MatchTokenAt_43(token, context);
					break;
				case 44:
					newState = MatchTokenAt_44(token, context);
					break;
				case 45:
					newState = MatchTokenAt_45(token, context);
					break;
				case 46:
					newState = MatchTokenAt_46(token, context);
					break;
				case 47:
					newState = MatchTokenAt_47(token, context);
					break;
				case 48:
					newState = MatchTokenAt_48(token, context);
					break;
				case 49:
					newState = MatchTokenAt_49(token, context);
					break;
				case 50:
					newState = MatchTokenAt_50(token, context);
					break;
				case 51:
					newState = MatchTokenAt_51(token, context);
					break;
				case 52:
					newState = MatchTokenAt_52(token, context);
					break;
				case 53:
					newState = MatchTokenAt_53(token, context);
					break;
				case 54:
					newState = MatchTokenAt_54(token, context);
					break;
				case 55:
					newState = MatchTokenAt_55(token, context);
					break;
				case 56:
					newState = MatchTokenAt_56(token, context);
					break;
				case 57:
					newState = MatchTokenAt_57(token, context);
					break;
				case 58:
					newState = MatchTokenAt_58(token, context);
					break;
				case 59:
					newState = MatchTokenAt_59(token, context);
					break;
				case 61:
					newState = MatchTokenAt_61(token, context);
					break;
				case 62:
					newState = MatchTokenAt_62(token, context);
					break;
				case 63:
					newState = MatchTokenAt_63(token, context);
					break;
				case 64:
					newState = MatchTokenAt_64(token, context);
					break;
				case 65:
					newState = MatchTokenAt_65(token, context);
					break;
				case 66:
					newState = MatchTokenAt_66(token, context);
					break;
				case 67:
					newState = MatchTokenAt_67(token, context);
					break;
				case 68:
					newState = MatchTokenAt_68(token, context);
					break;
				default:
					throw new InvalidOperationException("Unknown state: " + state);
			}
			return newState;
		}

		
		// Start
		int MatchTokenAt_0(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_LBracket(token)
)
			{
				StartRule(context, RuleType.Settings);
				Build(context, token);
				return 1;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				StartRule(context, RuleType.RuleDefinition);
				Build(context, token);
				return 10;
			}
				var error = new ParserError(token, new string[] {"#LBracket", "#Rule"}, "State: 0 - Start");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 0;

		}
		
		
		// Grammar:0>Settings:0>__grp5:0>#LBracket:0
		int MatchTokenAt_1(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_EOL(token)
)
			{
				Build(context, token);
				return 2;
			}
				var error = new ParserError(token, new string[] {"#EOL"}, "State: 1 - Grammar:0>Settings:0>__grp5:0>#LBracket:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 1;

		}
		
		
		// Grammar:0>Settings:0>__grp5:1>#EOL:0
		int MatchTokenAt_2(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Rule(token)
)
			{
				StartRule(context, RuleType.Parameter);
				Build(context, token);
				return 3;
			}
			if (	TokenMatcher.Match_RBracket(token)
)
			{
				Build(context, token);
				return 8;
			}
				var error = new ParserError(token, new string[] {"#Rule", "#RBracket"}, "State: 2 - Grammar:0>Settings:0>__grp5:1>#EOL:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 2;

		}
		
		
		// Grammar:0>Settings:1>Parameter:0>#Rule:0
		int MatchTokenAt_3(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Arrow(token)
)
			{
				Build(context, token);
				return 4;
			}
				var error = new ParserError(token, new string[] {"#Arrow"}, "State: 3 - Grammar:0>Settings:1>Parameter:0>#Rule:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 3;

		}
		
		
		// Grammar:0>Settings:1>Parameter:1>#Arrow:0
		int MatchTokenAt_4(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Rule(token)
)
			{
				Build(context, token);
				return 5;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				Build(context, token);
				return 5;
			}
				var error = new ParserError(token, new string[] {"#Rule", "#Token"}, "State: 4 - Grammar:0>Settings:1>Parameter:1>#Arrow:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 4;

		}
		
		
		// Grammar:0>Settings:1>Parameter:2>ParameterValue:0>__alt8:0>#Rule:0
		int MatchTokenAt_5(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Comma(token)
)
			{
				Build(context, token);
				return 6;
			}
			if (	TokenMatcher.Match_EOL(token)
)
			{
				Build(context, token);
				return 7;
			}
				var error = new ParserError(token, new string[] {"#Comma", "#EOL"}, "State: 5 - Grammar:0>Settings:1>Parameter:2>ParameterValue:0>__alt8:0>#Rule:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 5;

		}
		
		
		// Grammar:0>Settings:1>Parameter:3>__grp7:0>#Comma:0
		int MatchTokenAt_6(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Rule(token)
)
			{
				Build(context, token);
				return 5;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				Build(context, token);
				return 5;
			}
				var error = new ParserError(token, new string[] {"#Rule", "#Token"}, "State: 6 - Grammar:0>Settings:1>Parameter:3>__grp7:0>#Comma:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 6;

		}
		
		
		// Grammar:0>Settings:1>Parameter:4>#EOL:0
		int MatchTokenAt_7(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.Parameter);
				StartRule(context, RuleType.Parameter);
				Build(context, token);
				return 3;
			}
			if (	TokenMatcher.Match_RBracket(token)
)
			{
				EndRule(context, RuleType.Parameter);
				Build(context, token);
				return 8;
			}
				var error = new ParserError(token, new string[] {"#Rule", "#RBracket"}, "State: 7 - Grammar:0>Settings:1>Parameter:4>#EOL:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 7;

		}
		
		
		// Grammar:0>Settings:2>__grp6:0>#RBracket:0
		int MatchTokenAt_8(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_EOL(token)
)
			{
				Build(context, token);
				return 9;
			}
				var error = new ParserError(token, new string[] {"#EOL"}, "State: 8 - Grammar:0>Settings:2>__grp6:0>#RBracket:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 8;

		}
		
		
		// Grammar:0>Settings:2>__grp6:1>#EOL:0
		int MatchTokenAt_9(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.Settings);
				StartRule(context, RuleType.RuleDefinition);
				Build(context, token);
				return 10;
			}
				var error = new ParserError(token, new string[] {"#Rule"}, "State: 9 - Grammar:0>Settings:2>__grp6:1>#EOL:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 9;

		}
		
		
		// Grammar:1>RuleDefinition:0>#Rule:0
		int MatchTokenAt_10(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Production(token)
)
			{
				Build(context, token);
				return 11;
			}
			if (	TokenMatcher.Match_Definition(token)
)
			{
				Build(context, token);
				return 12;
			}
				var error = new ParserError(token, new string[] {"#Production", "#Definition"}, "State: 10 - Grammar:1>RuleDefinition:0>#Rule:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 10;

		}
		
		
		// Grammar:1>RuleDefinition:1>#Production:0
		int MatchTokenAt_11(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Definition(token)
)
			{
				Build(context, token);
				return 12;
			}
				var error = new ParserError(token, new string[] {"#Definition"}, "State: 11 - Grammar:1>RuleDefinition:1>#Production:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 11;

		}
		
		
		// Grammar:1>RuleDefinition:2>#Definition:0
		int MatchTokenAt_12(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 13;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 24;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 67;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 67;
			}
				var error = new ParserError(token, new string[] {"#LParen", "#Token", "#Rule"}, "State: 12 - Grammar:1>RuleDefinition:2>#Definition:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 12;

		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:0>#LParen:0
		int MatchTokenAt_13(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Rule(token)
)
			{
				Build(context, token);
				return 14;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				Build(context, token);
				return 14;
			}
				var error = new ParserError(token, new string[] {"#Rule", "#Token"}, "State: 13 - Grammar:1>RuleDefinition:3>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:0>#LParen:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 13;

		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:0>AlternateElementItem:0>__alt3:0>#Rule:0
		int MatchTokenAt_14(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_AlternateOp(token)
)
			{
				Build(context, token);
				return 15;
			}
			if (	TokenMatcher.Match_RParen(token)
)
			{
				Build(context, token);
				return 16;
			}
				var error = new ParserError(token, new string[] {"#AlternateOp", "#RParen"}, "State: 14 - Grammar:1>RuleDefinition:3>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:0>AlternateElementItem:0>__alt3:0>#Rule:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 14;

		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:1>__grp2:0>#AlternateOp:0
		int MatchTokenAt_15(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Rule(token)
)
			{
				Build(context, token);
				return 14;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				Build(context, token);
				return 14;
			}
				var error = new ParserError(token, new string[] {"#Rule", "#Token"}, "State: 15 - Grammar:1>RuleDefinition:3>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:1>__grp2:0>#AlternateOp:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 15;

		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:2>#RParen:0
		int MatchTokenAt_16(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_LBracket(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				StartRule(context, RuleType.LookAhead);
				Build(context, token);
				return 17;
			}
			if (	TokenMatcher.Match_AnyMultiplier(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 23;
			}
			if (	TokenMatcher.Match_OneOrMoreMultiplier(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 23;
			}
			if (	TokenMatcher.Match_OneOrZeroMultiplier(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 23;
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				EndRule(context, RuleType.AlternateElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 13;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 24;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 67;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 67;
			}
			if (	TokenMatcher.Match_EOL(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 68;
			}
				var error = new ParserError(token, new string[] {"#LBracket", "#AnyMultiplier", "#OneOrMoreMultiplier", "#OneOrZeroMultiplier", "#LParen", "#Token", "#Rule", "#EOL"}, "State: 16 - Grammar:1>RuleDefinition:3>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:2>#RParen:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 16;

		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:0>#LBracket:0
		int MatchTokenAt_17(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Token(token)
)
			{
				StartRule(context, RuleType.LookAheadTokenList);
				Build(context, token);
				return 18;
			}
			if (	TokenMatcher.Match_Arrow(token)
)
			{
				Build(context, token);
				return 20;
			}
				var error = new ParserError(token, new string[] {"#Token", "#Arrow"}, "State: 17 - Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:0>#LBracket:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 17;

		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:0>#Token:0
		int MatchTokenAt_18(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_AlternateOp(token)
)
			{
				Build(context, token);
				return 19;
			}
			if (	TokenMatcher.Match_Arrow(token)
)
			{
				EndRule(context, RuleType.LookAheadTokenList);
				Build(context, token);
				return 20;
			}
				var error = new ParserError(token, new string[] {"#AlternateOp", "#Arrow"}, "State: 18 - Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:0>#Token:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 18;

		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:1>__grp4:0>#AlternateOp:0
		int MatchTokenAt_19(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Token(token)
)
			{
				Build(context, token);
				return 18;
			}
				var error = new ParserError(token, new string[] {"#Token"}, "State: 19 - Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:1>__grp4:0>#AlternateOp:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 19;

		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:2>#Arrow:0
		int MatchTokenAt_20(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Token(token)
)
			{
				StartRule(context, RuleType.LookAheadTokenList);
				Build(context, token);
				return 21;
			}
				var error = new ParserError(token, new string[] {"#Token"}, "State: 20 - Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:2>#Arrow:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 20;

		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:3>LookAheadTokenList:0>#Token:0
		int MatchTokenAt_21(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_AlternateOp(token)
)
			{
				Build(context, token);
				return 19;
			}
			if (	TokenMatcher.Match_RBracket(token)
)
			{
				EndRule(context, RuleType.LookAheadTokenList);
				Build(context, token);
				return 22;
			}
				var error = new ParserError(token, new string[] {"#AlternateOp", "#RBracket"}, "State: 21 - Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:3>LookAheadTokenList:0>#Token:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 21;

		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:4>#RBracket:0
		int MatchTokenAt_22(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_AnyMultiplier(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				Build(context, token);
				return 23;
			}
			if (	TokenMatcher.Match_OneOrMoreMultiplier(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				Build(context, token);
				return 23;
			}
			if (	TokenMatcher.Match_OneOrZeroMultiplier(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				Build(context, token);
				return 23;
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				EndRule(context, RuleType.LookAhead);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 13;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 24;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 67;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 67;
			}
			if (	TokenMatcher.Match_EOL(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				EndRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 68;
			}
				var error = new ParserError(token, new string[] {"#AnyMultiplier", "#OneOrMoreMultiplier", "#OneOrZeroMultiplier", "#LParen", "#Token", "#Rule", "#EOL"}, "State: 22 - Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:4>#RBracket:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 22;

		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:2>RuleDefinitionElement_Multiplier:0>__alt1:0>#AnyMultiplier:0
		int MatchTokenAt_23(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 13;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 24;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 67;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 67;
			}
			if (	TokenMatcher.Match_EOL(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 68;
			}
				var error = new ParserError(token, new string[] {"#LParen", "#Token", "#Rule", "#EOL"}, "State: 23 - Grammar:1>RuleDefinition:3>RuleDefinitionElement:2>RuleDefinitionElement_Multiplier:0>__alt1:0>#AnyMultiplier:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 23;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:0>#LParen:0
		int MatchTokenAt_24(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 25;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 36;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 65;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 65;
			}
				var error = new ParserError(token, new string[] {"#LParen", "#Token", "#Rule"}, "State: 24 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:0>#LParen:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 24;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:0>#LParen:0
		int MatchTokenAt_25(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Rule(token)
)
			{
				Build(context, token);
				return 26;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				Build(context, token);
				return 26;
			}
				var error = new ParserError(token, new string[] {"#Rule", "#Token"}, "State: 25 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:0>#LParen:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 25;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:0>AlternateElementItem:0>__alt3:0>#Rule:0
		int MatchTokenAt_26(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_AlternateOp(token)
)
			{
				Build(context, token);
				return 27;
			}
			if (	TokenMatcher.Match_RParen(token)
)
			{
				Build(context, token);
				return 28;
			}
				var error = new ParserError(token, new string[] {"#AlternateOp", "#RParen"}, "State: 26 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:0>AlternateElementItem:0>__alt3:0>#Rule:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 26;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:1>__grp2:0>#AlternateOp:0
		int MatchTokenAt_27(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Rule(token)
)
			{
				Build(context, token);
				return 26;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				Build(context, token);
				return 26;
			}
				var error = new ParserError(token, new string[] {"#Rule", "#Token"}, "State: 27 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:1>__grp2:0>#AlternateOp:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 27;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:2>#RParen:0
		int MatchTokenAt_28(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_LBracket(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				StartRule(context, RuleType.LookAhead);
				Build(context, token);
				return 29;
			}
			if (	TokenMatcher.Match_AnyMultiplier(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 35;
			}
			if (	TokenMatcher.Match_OneOrMoreMultiplier(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 35;
			}
			if (	TokenMatcher.Match_OneOrZeroMultiplier(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 35;
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				EndRule(context, RuleType.AlternateElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 25;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 36;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 65;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 65;
			}
			if (	TokenMatcher.Match_RParen(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 66;
			}
				var error = new ParserError(token, new string[] {"#LBracket", "#AnyMultiplier", "#OneOrMoreMultiplier", "#OneOrZeroMultiplier", "#LParen", "#Token", "#Rule", "#RParen"}, "State: 28 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:2>#RParen:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 28;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:0>#LBracket:0
		int MatchTokenAt_29(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Token(token)
)
			{
				StartRule(context, RuleType.LookAheadTokenList);
				Build(context, token);
				return 30;
			}
			if (	TokenMatcher.Match_Arrow(token)
)
			{
				Build(context, token);
				return 32;
			}
				var error = new ParserError(token, new string[] {"#Token", "#Arrow"}, "State: 29 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:0>#LBracket:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 29;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:0>#Token:0
		int MatchTokenAt_30(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_AlternateOp(token)
)
			{
				Build(context, token);
				return 31;
			}
			if (	TokenMatcher.Match_Arrow(token)
)
			{
				EndRule(context, RuleType.LookAheadTokenList);
				Build(context, token);
				return 32;
			}
				var error = new ParserError(token, new string[] {"#AlternateOp", "#Arrow"}, "State: 30 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:0>#Token:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 30;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:1>__grp4:0>#AlternateOp:0
		int MatchTokenAt_31(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Token(token)
)
			{
				Build(context, token);
				return 30;
			}
				var error = new ParserError(token, new string[] {"#Token"}, "State: 31 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:1>__grp4:0>#AlternateOp:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 31;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:2>#Arrow:0
		int MatchTokenAt_32(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Token(token)
)
			{
				StartRule(context, RuleType.LookAheadTokenList);
				Build(context, token);
				return 33;
			}
				var error = new ParserError(token, new string[] {"#Token"}, "State: 32 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:2>#Arrow:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 32;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:3>LookAheadTokenList:0>#Token:0
		int MatchTokenAt_33(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_AlternateOp(token)
)
			{
				Build(context, token);
				return 31;
			}
			if (	TokenMatcher.Match_RBracket(token)
)
			{
				EndRule(context, RuleType.LookAheadTokenList);
				Build(context, token);
				return 34;
			}
				var error = new ParserError(token, new string[] {"#AlternateOp", "#RBracket"}, "State: 33 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:3>LookAheadTokenList:0>#Token:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 33;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:4>#RBracket:0
		int MatchTokenAt_34(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_AnyMultiplier(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				Build(context, token);
				return 35;
			}
			if (	TokenMatcher.Match_OneOrMoreMultiplier(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				Build(context, token);
				return 35;
			}
			if (	TokenMatcher.Match_OneOrZeroMultiplier(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				Build(context, token);
				return 35;
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				EndRule(context, RuleType.LookAhead);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 25;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 36;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 65;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 65;
			}
			if (	TokenMatcher.Match_RParen(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				EndRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 66;
			}
				var error = new ParserError(token, new string[] {"#AnyMultiplier", "#OneOrMoreMultiplier", "#OneOrZeroMultiplier", "#LParen", "#Token", "#Rule", "#RParen"}, "State: 34 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:4>#RBracket:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 34;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:2>RuleDefinitionElement_Multiplier:0>__alt1:0>#AnyMultiplier:0
		int MatchTokenAt_35(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 25;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 36;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 65;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 65;
			}
			if (	TokenMatcher.Match_RParen(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 66;
			}
				var error = new ParserError(token, new string[] {"#LParen", "#Token", "#Rule", "#RParen"}, "State: 35 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:2>RuleDefinitionElement_Multiplier:0>__alt1:0>#AnyMultiplier:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 35;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:0>#LParen:0
		int MatchTokenAt_36(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 37;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 48;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 63;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 63;
			}
				var error = new ParserError(token, new string[] {"#LParen", "#Token", "#Rule"}, "State: 36 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:0>#LParen:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 36;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:0>#LParen:0
		int MatchTokenAt_37(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Rule(token)
)
			{
				Build(context, token);
				return 38;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				Build(context, token);
				return 38;
			}
				var error = new ParserError(token, new string[] {"#Rule", "#Token"}, "State: 37 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:0>#LParen:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 37;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:0>AlternateElementItem:0>__alt3:0>#Rule:0
		int MatchTokenAt_38(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_AlternateOp(token)
)
			{
				Build(context, token);
				return 39;
			}
			if (	TokenMatcher.Match_RParen(token)
)
			{
				Build(context, token);
				return 40;
			}
				var error = new ParserError(token, new string[] {"#AlternateOp", "#RParen"}, "State: 38 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:0>AlternateElementItem:0>__alt3:0>#Rule:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 38;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:1>__grp2:0>#AlternateOp:0
		int MatchTokenAt_39(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Rule(token)
)
			{
				Build(context, token);
				return 38;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				Build(context, token);
				return 38;
			}
				var error = new ParserError(token, new string[] {"#Rule", "#Token"}, "State: 39 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:1>__grp2:0>#AlternateOp:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 39;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:2>#RParen:0
		int MatchTokenAt_40(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_LBracket(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				StartRule(context, RuleType.LookAhead);
				Build(context, token);
				return 41;
			}
			if (	TokenMatcher.Match_AnyMultiplier(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 47;
			}
			if (	TokenMatcher.Match_OneOrMoreMultiplier(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 47;
			}
			if (	TokenMatcher.Match_OneOrZeroMultiplier(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 47;
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				EndRule(context, RuleType.AlternateElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 37;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 48;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 63;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 63;
			}
			if (	TokenMatcher.Match_RParen(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 64;
			}
				var error = new ParserError(token, new string[] {"#LBracket", "#AnyMultiplier", "#OneOrMoreMultiplier", "#OneOrZeroMultiplier", "#LParen", "#Token", "#Rule", "#RParen"}, "State: 40 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:2>#RParen:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 40;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:0>#LBracket:0
		int MatchTokenAt_41(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Token(token)
)
			{
				StartRule(context, RuleType.LookAheadTokenList);
				Build(context, token);
				return 42;
			}
			if (	TokenMatcher.Match_Arrow(token)
)
			{
				Build(context, token);
				return 44;
			}
				var error = new ParserError(token, new string[] {"#Token", "#Arrow"}, "State: 41 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:0>#LBracket:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 41;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:0>#Token:0
		int MatchTokenAt_42(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_AlternateOp(token)
)
			{
				Build(context, token);
				return 43;
			}
			if (	TokenMatcher.Match_Arrow(token)
)
			{
				EndRule(context, RuleType.LookAheadTokenList);
				Build(context, token);
				return 44;
			}
				var error = new ParserError(token, new string[] {"#AlternateOp", "#Arrow"}, "State: 42 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:0>#Token:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 42;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:1>__grp4:0>#AlternateOp:0
		int MatchTokenAt_43(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Token(token)
)
			{
				Build(context, token);
				return 42;
			}
				var error = new ParserError(token, new string[] {"#Token"}, "State: 43 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:1>__grp4:0>#AlternateOp:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 43;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:2>#Arrow:0
		int MatchTokenAt_44(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Token(token)
)
			{
				StartRule(context, RuleType.LookAheadTokenList);
				Build(context, token);
				return 45;
			}
				var error = new ParserError(token, new string[] {"#Token"}, "State: 44 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:2>#Arrow:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 44;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:3>LookAheadTokenList:0>#Token:0
		int MatchTokenAt_45(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_AlternateOp(token)
)
			{
				Build(context, token);
				return 43;
			}
			if (	TokenMatcher.Match_RBracket(token)
)
			{
				EndRule(context, RuleType.LookAheadTokenList);
				Build(context, token);
				return 46;
			}
				var error = new ParserError(token, new string[] {"#AlternateOp", "#RBracket"}, "State: 45 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:3>LookAheadTokenList:0>#Token:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 45;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:4>#RBracket:0
		int MatchTokenAt_46(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_AnyMultiplier(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				Build(context, token);
				return 47;
			}
			if (	TokenMatcher.Match_OneOrMoreMultiplier(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				Build(context, token);
				return 47;
			}
			if (	TokenMatcher.Match_OneOrZeroMultiplier(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				Build(context, token);
				return 47;
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				EndRule(context, RuleType.LookAhead);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 37;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 48;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 63;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 63;
			}
			if (	TokenMatcher.Match_RParen(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				EndRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 64;
			}
				var error = new ParserError(token, new string[] {"#AnyMultiplier", "#OneOrMoreMultiplier", "#OneOrZeroMultiplier", "#LParen", "#Token", "#Rule", "#RParen"}, "State: 46 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:4>#RBracket:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 46;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:2>RuleDefinitionElement_Multiplier:0>__alt1:0>#AnyMultiplier:0
		int MatchTokenAt_47(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 37;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 48;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 63;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 63;
			}
			if (	TokenMatcher.Match_RParen(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 64;
			}
				var error = new ParserError(token, new string[] {"#LParen", "#Token", "#Rule", "#RParen"}, "State: 47 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:2>RuleDefinitionElement_Multiplier:0>__alt1:0>#AnyMultiplier:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 47;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:0>#LParen:0
		int MatchTokenAt_48(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 49;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 60;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 61;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 61;
			}
				var error = new ParserError(token, new string[] {"#LParen", "#Token", "#Rule"}, "State: 48 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:0>#LParen:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 48;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:0>#LParen:0
		int MatchTokenAt_49(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Rule(token)
)
			{
				Build(context, token);
				return 50;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				Build(context, token);
				return 50;
			}
				var error = new ParserError(token, new string[] {"#Rule", "#Token"}, "State: 49 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:0>#LParen:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 49;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:0>AlternateElementItem:0>__alt3:0>#Rule:0
		int MatchTokenAt_50(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_AlternateOp(token)
)
			{
				Build(context, token);
				return 51;
			}
			if (	TokenMatcher.Match_RParen(token)
)
			{
				Build(context, token);
				return 52;
			}
				var error = new ParserError(token, new string[] {"#AlternateOp", "#RParen"}, "State: 50 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:0>AlternateElementItem:0>__alt3:0>#Rule:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 50;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:1>__grp2:0>#AlternateOp:0
		int MatchTokenAt_51(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Rule(token)
)
			{
				Build(context, token);
				return 50;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				Build(context, token);
				return 50;
			}
				var error = new ParserError(token, new string[] {"#Rule", "#Token"}, "State: 51 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:1>__grp2:0>#AlternateOp:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 51;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:2>#RParen:0
		int MatchTokenAt_52(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_LBracket(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				StartRule(context, RuleType.LookAhead);
				Build(context, token);
				return 53;
			}
			if (	TokenMatcher.Match_AnyMultiplier(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 59;
			}
			if (	TokenMatcher.Match_OneOrMoreMultiplier(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 59;
			}
			if (	TokenMatcher.Match_OneOrZeroMultiplier(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 59;
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				EndRule(context, RuleType.AlternateElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 49;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 60;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 61;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 61;
			}
			if (	TokenMatcher.Match_RParen(token)
)
			{
				EndRule(context, RuleType.AlternateElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 62;
			}
				var error = new ParserError(token, new string[] {"#LBracket", "#AnyMultiplier", "#OneOrMoreMultiplier", "#OneOrZeroMultiplier", "#LParen", "#Token", "#Rule", "#RParen"}, "State: 52 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:2>#RParen:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 52;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:0>#LBracket:0
		int MatchTokenAt_53(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Token(token)
)
			{
				StartRule(context, RuleType.LookAheadTokenList);
				Build(context, token);
				return 54;
			}
			if (	TokenMatcher.Match_Arrow(token)
)
			{
				Build(context, token);
				return 56;
			}
				var error = new ParserError(token, new string[] {"#Token", "#Arrow"}, "State: 53 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:0>#LBracket:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 53;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:0>#Token:0
		int MatchTokenAt_54(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_AlternateOp(token)
)
			{
				Build(context, token);
				return 55;
			}
			if (	TokenMatcher.Match_Arrow(token)
)
			{
				EndRule(context, RuleType.LookAheadTokenList);
				Build(context, token);
				return 56;
			}
				var error = new ParserError(token, new string[] {"#AlternateOp", "#Arrow"}, "State: 54 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:0>#Token:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 54;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:1>__grp4:0>#AlternateOp:0
		int MatchTokenAt_55(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Token(token)
)
			{
				Build(context, token);
				return 54;
			}
				var error = new ParserError(token, new string[] {"#Token"}, "State: 55 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:1>__grp4:0>#AlternateOp:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 55;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:2>#Arrow:0
		int MatchTokenAt_56(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_Token(token)
)
			{
				StartRule(context, RuleType.LookAheadTokenList);
				Build(context, token);
				return 57;
			}
				var error = new ParserError(token, new string[] {"#Token"}, "State: 56 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:2>#Arrow:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 56;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:3>LookAheadTokenList:0>#Token:0
		int MatchTokenAt_57(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_AlternateOp(token)
)
			{
				Build(context, token);
				return 55;
			}
			if (	TokenMatcher.Match_RBracket(token)
)
			{
				EndRule(context, RuleType.LookAheadTokenList);
				Build(context, token);
				return 58;
			}
				var error = new ParserError(token, new string[] {"#AlternateOp", "#RBracket"}, "State: 57 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:3>LookAheadTokenList:0>#Token:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 57;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:4>#RBracket:0
		int MatchTokenAt_58(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_AnyMultiplier(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				Build(context, token);
				return 59;
			}
			if (	TokenMatcher.Match_OneOrMoreMultiplier(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				Build(context, token);
				return 59;
			}
			if (	TokenMatcher.Match_OneOrZeroMultiplier(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				Build(context, token);
				return 59;
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				EndRule(context, RuleType.LookAhead);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 49;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 60;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 61;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 61;
			}
			if (	TokenMatcher.Match_RParen(token)
)
			{
				EndRule(context, RuleType.LookAhead);
				EndRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 62;
			}
				var error = new ParserError(token, new string[] {"#AnyMultiplier", "#OneOrMoreMultiplier", "#OneOrZeroMultiplier", "#LParen", "#Token", "#Rule", "#RParen"}, "State: 58 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:4>#RBracket:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 58;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:2>RuleDefinitionElement_Multiplier:0>__alt1:0>#AnyMultiplier:0
		int MatchTokenAt_59(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 49;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 60;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 61;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 61;
			}
			if (	TokenMatcher.Match_RParen(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 62;
			}
				var error = new ParserError(token, new string[] {"#LParen", "#Token", "#Rule", "#RParen"}, "State: 59 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:2>RuleDefinitionElement_Multiplier:0>__alt1:0>#AnyMultiplier:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 59;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:1>TokenElement:0>#Token:0
		int MatchTokenAt_61(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_LBracket(token)
)
			{
				StartRule(context, RuleType.LookAhead);
				Build(context, token);
				return 53;
			}
			if (	TokenMatcher.Match_AnyMultiplier(token)
)
			{
				Build(context, token);
				return 59;
			}
			if (	TokenMatcher.Match_OneOrMoreMultiplier(token)
)
			{
				Build(context, token);
				return 59;
			}
			if (	TokenMatcher.Match_OneOrZeroMultiplier(token)
)
			{
				Build(context, token);
				return 59;
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 49;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 60;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 61;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 61;
			}
			if (	TokenMatcher.Match_RParen(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 62;
			}
				var error = new ParserError(token, new string[] {"#LBracket", "#AnyMultiplier", "#OneOrMoreMultiplier", "#OneOrZeroMultiplier", "#LParen", "#Token", "#Rule", "#RParen"}, "State: 61 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:1>TokenElement:0>#Token:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 61;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:3>#RParen:0
		int MatchTokenAt_62(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_LBracket(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				StartRule(context, RuleType.LookAhead);
				Build(context, token);
				return 41;
			}
			if (	TokenMatcher.Match_AnyMultiplier(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				Build(context, token);
				return 47;
			}
			if (	TokenMatcher.Match_OneOrMoreMultiplier(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				Build(context, token);
				return 47;
			}
			if (	TokenMatcher.Match_OneOrZeroMultiplier(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				Build(context, token);
				return 47;
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				EndRule(context, RuleType.GroupElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 37;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 48;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 63;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 63;
			}
			if (	TokenMatcher.Match_RParen(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 64;
			}
				var error = new ParserError(token, new string[] {"#LBracket", "#AnyMultiplier", "#OneOrMoreMultiplier", "#OneOrZeroMultiplier", "#LParen", "#Token", "#Rule", "#RParen"}, "State: 62 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:3>#RParen:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 62;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:1>TokenElement:0>#Token:0
		int MatchTokenAt_63(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_LBracket(token)
)
			{
				StartRule(context, RuleType.LookAhead);
				Build(context, token);
				return 41;
			}
			if (	TokenMatcher.Match_AnyMultiplier(token)
)
			{
				Build(context, token);
				return 47;
			}
			if (	TokenMatcher.Match_OneOrMoreMultiplier(token)
)
			{
				Build(context, token);
				return 47;
			}
			if (	TokenMatcher.Match_OneOrZeroMultiplier(token)
)
			{
				Build(context, token);
				return 47;
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 37;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 48;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 63;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 63;
			}
			if (	TokenMatcher.Match_RParen(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 64;
			}
				var error = new ParserError(token, new string[] {"#LBracket", "#AnyMultiplier", "#OneOrMoreMultiplier", "#OneOrZeroMultiplier", "#LParen", "#Token", "#Rule", "#RParen"}, "State: 63 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:1>TokenElement:0>#Token:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 63;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:3>#RParen:0
		int MatchTokenAt_64(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_LBracket(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				StartRule(context, RuleType.LookAhead);
				Build(context, token);
				return 29;
			}
			if (	TokenMatcher.Match_AnyMultiplier(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				Build(context, token);
				return 35;
			}
			if (	TokenMatcher.Match_OneOrMoreMultiplier(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				Build(context, token);
				return 35;
			}
			if (	TokenMatcher.Match_OneOrZeroMultiplier(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				Build(context, token);
				return 35;
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				EndRule(context, RuleType.GroupElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 25;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 36;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 65;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 65;
			}
			if (	TokenMatcher.Match_RParen(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 66;
			}
				var error = new ParserError(token, new string[] {"#LBracket", "#AnyMultiplier", "#OneOrMoreMultiplier", "#OneOrZeroMultiplier", "#LParen", "#Token", "#Rule", "#RParen"}, "State: 64 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:3>#RParen:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 64;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:1>TokenElement:0>#Token:0
		int MatchTokenAt_65(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_LBracket(token)
)
			{
				StartRule(context, RuleType.LookAhead);
				Build(context, token);
				return 29;
			}
			if (	TokenMatcher.Match_AnyMultiplier(token)
)
			{
				Build(context, token);
				return 35;
			}
			if (	TokenMatcher.Match_OneOrMoreMultiplier(token)
)
			{
				Build(context, token);
				return 35;
			}
			if (	TokenMatcher.Match_OneOrZeroMultiplier(token)
)
			{
				Build(context, token);
				return 35;
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 25;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 36;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 65;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 65;
			}
			if (	TokenMatcher.Match_RParen(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 66;
			}
				var error = new ParserError(token, new string[] {"#LBracket", "#AnyMultiplier", "#OneOrMoreMultiplier", "#OneOrZeroMultiplier", "#LParen", "#Token", "#Rule", "#RParen"}, "State: 65 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:1>TokenElement:0>#Token:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 65;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:3>#RParen:0
		int MatchTokenAt_66(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_LBracket(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				StartRule(context, RuleType.LookAhead);
				Build(context, token);
				return 17;
			}
			if (	TokenMatcher.Match_AnyMultiplier(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				Build(context, token);
				return 23;
			}
			if (	TokenMatcher.Match_OneOrMoreMultiplier(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				Build(context, token);
				return 23;
			}
			if (	TokenMatcher.Match_OneOrZeroMultiplier(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				Build(context, token);
				return 23;
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				EndRule(context, RuleType.GroupElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 13;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 24;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 67;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 67;
			}
			if (	TokenMatcher.Match_EOL(token)
)
			{
				EndRule(context, RuleType.GroupElement);
				EndRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 68;
			}
				var error = new ParserError(token, new string[] {"#LBracket", "#AnyMultiplier", "#OneOrMoreMultiplier", "#OneOrZeroMultiplier", "#LParen", "#Token", "#Rule", "#EOL"}, "State: 66 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:3>#RParen:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 66;

		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:1>TokenElement:0>#Token:0
		int MatchTokenAt_67(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_LBracket(token)
)
			{
				StartRule(context, RuleType.LookAhead);
				Build(context, token);
				return 17;
			}
			if (	TokenMatcher.Match_AnyMultiplier(token)
)
			{
				Build(context, token);
				return 23;
			}
			if (	TokenMatcher.Match_OneOrMoreMultiplier(token)
)
			{
				Build(context, token);
				return 23;
			}
			if (	TokenMatcher.Match_OneOrZeroMultiplier(token)
)
			{
				Build(context, token);
				return 23;
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				if (LookAhead_0(context, token))
				{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.AlternateElement);
				Build(context, token);
				return 13;
				}
			}
			if (	TokenMatcher.Match_LParen(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.GroupElement);
				Build(context, token);
				return 24;
			}
			if (	TokenMatcher.Match_Token(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 67;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				StartRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 67;
			}
			if (	TokenMatcher.Match_EOL(token)
)
			{
				EndRule(context, RuleType.RuleDefinitionElement);
				Build(context, token);
				return 68;
			}
				var error = new ParserError(token, new string[] {"#LBracket", "#AnyMultiplier", "#OneOrMoreMultiplier", "#OneOrZeroMultiplier", "#LParen", "#Token", "#Rule", "#EOL"}, "State: 67 - Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:1>TokenElement:0>#Token:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 67;

		}
		
		
		// Grammar:1>RuleDefinition:5>#EOL:0
		int MatchTokenAt_68(Token token, ParserContext context)
		{
			if (	TokenMatcher.Match_EOF(token)
)
			{
				EndRule(context, RuleType.RuleDefinition);
				Build(context, token);
				return 60;
			}
			if (	TokenMatcher.Match_Rule(token)
)
			{
				EndRule(context, RuleType.RuleDefinition);
				StartRule(context, RuleType.RuleDefinition);
				Build(context, token);
				return 10;
			}
				var error = new ParserError(token, new string[] {"#EOF", "#Rule"}, "State: 68 - Grammar:1>RuleDefinition:5>#EOL:0");
	if (StopAtFirstError)
		throw new ParserException(ParserMessageProvider, error);
	context.Errors.Add(error);
	return 68;

		}
		

		
		bool LookAhead_0(ParserContext context, Token currentToken)
		{
			currentToken.Detach();
            Token token;
			var queue = new Queue<Token>();
			bool match = false;
		    do
		    {
		        token = ReadToken(context);
				token.Detach();
		        queue.Enqueue(token);

		        if (false
					|| 	TokenMatcher.Match_AlternateOp(token)

				)
		        {
					match = true;
					break;
		        }
		    } while (false
				|| 	TokenMatcher.Match_Token(token)

				|| 	TokenMatcher.Match_Rule(token)

			);
			foreach(var t in queue)
				context.TokenQueue.Enqueue(t);
			return match;
		}
		
	}
}