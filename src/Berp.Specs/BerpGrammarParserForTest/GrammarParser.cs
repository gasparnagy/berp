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

    public class Parser
    {
		class ParserContext
		{
			public TokenScanner TokenScanner { get; set; }
			public ASTBuilder Builder { get; set; }
			public Queue<Token> TokenQueue { get; set; }
		}

        public object Parse(TokenScanner tokenScanner)
		{
			var context = new ParserContext
			{
				TokenScanner = tokenScanner,
				Builder = new ASTBuilder(),
				TokenQueue = new Queue<Token>()
			};

			context.Builder.Push(RuleType.Grammar);
            int state = 0;
            Token token;
            do
			{
				token = ReadToken(context);
				state = MatchToken(state, token, context);
            } while(!token.IsEOF);

			if (state != 60)
			{
				throw new Exception("parsing error: end of file expected");
			}

			context.Builder.Pop(RuleType.Grammar);
			return context.Builder.RootNode;
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
					throw new NotImplementedException();
			}
			return newState;
		}

		
		// Start
		int MatchTokenAt_0(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_LBracket(token))
			{
				context.Builder.Push(RuleType.Settings);
				context.Builder.Build(token);
				return 1;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Push(RuleType.RuleDefinition);
				context.Builder.Build(token);
				return 10;
			}
			throw new Exception("parsing error at state 0: Start");
		}
		
		
		// Grammar:0>Settings:0>__grp5:0>#LBracket:0
		int MatchTokenAt_1(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOL(token))
			{
				context.Builder.Build(token);
				return 2;
			}
			throw new Exception("parsing error at state 1: Grammar:0>Settings:0>__grp5:0>#LBracket:0");
		}
		
		
		// Grammar:0>Settings:0>__grp5:1>#EOL:0
		int MatchTokenAt_2(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Push(RuleType.Parameter);
				context.Builder.Build(token);
				return 3;
			}
			if (context.TokenScanner.Match_RBracket(token))
			{
				context.Builder.Build(token);
				return 8;
			}
			throw new Exception("parsing error at state 2: Grammar:0>Settings:0>__grp5:1>#EOL:0");
		}
		
		
		// Grammar:0>Settings:1>Parameter:0>#Rule:0
		int MatchTokenAt_3(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Arrow(token))
			{
				context.Builder.Build(token);
				return 4;
			}
			throw new Exception("parsing error at state 3: Grammar:0>Settings:1>Parameter:0>#Rule:0");
		}
		
		
		// Grammar:0>Settings:1>Parameter:1>#Arrow:0
		int MatchTokenAt_4(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Build(token);
				return 5;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Build(token);
				return 5;
			}
			throw new Exception("parsing error at state 4: Grammar:0>Settings:1>Parameter:1>#Arrow:0");
		}
		
		
		// Grammar:0>Settings:1>Parameter:2>ParameterValue:0>__alt8:0>#Rule:0
		int MatchTokenAt_5(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Comma(token))
			{
				context.Builder.Build(token);
				return 6;
			}
			if (context.TokenScanner.Match_EOL(token))
			{
				context.Builder.Build(token);
				return 7;
			}
			throw new Exception("parsing error at state 5: Grammar:0>Settings:1>Parameter:2>ParameterValue:0>__alt8:0>#Rule:0");
		}
		
		
		// Grammar:0>Settings:1>Parameter:3>__grp7:0>#Comma:0
		int MatchTokenAt_6(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Build(token);
				return 5;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Build(token);
				return 5;
			}
			throw new Exception("parsing error at state 6: Grammar:0>Settings:1>Parameter:3>__grp7:0>#Comma:0");
		}
		
		
		// Grammar:0>Settings:1>Parameter:4>#EOL:0
		int MatchTokenAt_7(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.Parameter);
				context.Builder.Push(RuleType.Parameter);
				context.Builder.Build(token);
				return 3;
			}
			if (context.TokenScanner.Match_RBracket(token))
			{
				context.Builder.Pop(RuleType.Parameter);
				context.Builder.Build(token);
				return 8;
			}
			throw new Exception("parsing error at state 7: Grammar:0>Settings:1>Parameter:4>#EOL:0");
		}
		
		
		// Grammar:0>Settings:2>__grp6:0>#RBracket:0
		int MatchTokenAt_8(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOL(token))
			{
				context.Builder.Build(token);
				return 9;
			}
			throw new Exception("parsing error at state 8: Grammar:0>Settings:2>__grp6:0>#RBracket:0");
		}
		
		
		// Grammar:0>Settings:2>__grp6:1>#EOL:0
		int MatchTokenAt_9(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.Settings);
				context.Builder.Push(RuleType.RuleDefinition);
				context.Builder.Build(token);
				return 10;
			}
			throw new Exception("parsing error at state 9: Grammar:0>Settings:2>__grp6:1>#EOL:0");
		}
		
		
		// Grammar:1>RuleDefinition:0>#Rule:0
		int MatchTokenAt_10(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Production(token))
			{
				context.Builder.Build(token);
				return 11;
			}
			if (context.TokenScanner.Match_Definition(token))
			{
				context.Builder.Build(token);
				return 12;
			}
			throw new Exception("parsing error at state 10: Grammar:1>RuleDefinition:0>#Rule:0");
		}
		
		
		// Grammar:1>RuleDefinition:1>#Production:0
		int MatchTokenAt_11(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Definition(token))
			{
				context.Builder.Build(token);
				return 12;
			}
			throw new Exception("parsing error at state 11: Grammar:1>RuleDefinition:1>#Production:0");
		}
		
		
		// Grammar:1>RuleDefinition:2>#Definition:0
		int MatchTokenAt_12(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 13;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 24;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 67;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 67;
			}
			throw new Exception("parsing error at state 12: Grammar:1>RuleDefinition:2>#Definition:0");
		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:0>#LParen:0
		int MatchTokenAt_13(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Build(token);
				return 14;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Build(token);
				return 14;
			}
			throw new Exception("parsing error at state 13: Grammar:1>RuleDefinition:3>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:0>#LParen:0");
		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:0>AlternateElementItem:0>__alt3:0>#Rule:0
		int MatchTokenAt_14(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_AlternateOp(token))
			{
				context.Builder.Build(token);
				return 15;
			}
			if (context.TokenScanner.Match_RParen(token))
			{
				context.Builder.Build(token);
				return 16;
			}
			throw new Exception("parsing error at state 14: Grammar:1>RuleDefinition:3>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:0>AlternateElementItem:0>__alt3:0>#Rule:0");
		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:1>__grp2:0>#AlternateOp:0
		int MatchTokenAt_15(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Build(token);
				return 14;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Build(token);
				return 14;
			}
			throw new Exception("parsing error at state 15: Grammar:1>RuleDefinition:3>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:1>__grp2:0>#AlternateOp:0");
		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:2>#RParen:0
		int MatchTokenAt_16(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_LBracket(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Push(RuleType.LookAhead);
				context.Builder.Build(token);
				return 17;
			}
			if (context.TokenScanner.Match_AnyMultiplier(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 23;
			}
			if (context.TokenScanner.Match_OneOrMoreMultiplier(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 23;
			}
			if (context.TokenScanner.Match_OneOrZeroMultiplier(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 23;
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 13;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 24;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 67;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 67;
			}
			if (context.TokenScanner.Match_EOL(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 68;
			}
			throw new Exception("parsing error at state 16: Grammar:1>RuleDefinition:3>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:2>#RParen:0");
		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:0>#LBracket:0
		int MatchTokenAt_17(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Push(RuleType.LookAheadTokenList);
				context.Builder.Build(token);
				return 18;
			}
			if (context.TokenScanner.Match_Arrow(token))
			{
				context.Builder.Build(token);
				return 20;
			}
			throw new Exception("parsing error at state 17: Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:0>#LBracket:0");
		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:0>#Token:0
		int MatchTokenAt_18(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_AlternateOp(token))
			{
				context.Builder.Build(token);
				return 19;
			}
			if (context.TokenScanner.Match_Arrow(token))
			{
				context.Builder.Pop(RuleType.LookAheadTokenList);
				context.Builder.Build(token);
				return 20;
			}
			throw new Exception("parsing error at state 18: Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:0>#Token:0");
		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:1>__grp4:0>#AlternateOp:0
		int MatchTokenAt_19(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Build(token);
				return 18;
			}
			throw new Exception("parsing error at state 19: Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:1>__grp4:0>#AlternateOp:0");
		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:2>#Arrow:0
		int MatchTokenAt_20(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Push(RuleType.LookAheadTokenList);
				context.Builder.Build(token);
				return 21;
			}
			throw new Exception("parsing error at state 20: Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:2>#Arrow:0");
		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:3>LookAheadTokenList:0>#Token:0
		int MatchTokenAt_21(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_AlternateOp(token))
			{
				context.Builder.Build(token);
				return 19;
			}
			if (context.TokenScanner.Match_RBracket(token))
			{
				context.Builder.Pop(RuleType.LookAheadTokenList);
				context.Builder.Build(token);
				return 22;
			}
			throw new Exception("parsing error at state 21: Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:3>LookAheadTokenList:0>#Token:0");
		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:4>#RBracket:0
		int MatchTokenAt_22(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_AnyMultiplier(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Build(token);
				return 23;
			}
			if (context.TokenScanner.Match_OneOrMoreMultiplier(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Build(token);
				return 23;
			}
			if (context.TokenScanner.Match_OneOrZeroMultiplier(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Build(token);
				return 23;
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 13;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 24;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 67;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 67;
			}
			if (context.TokenScanner.Match_EOL(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 68;
			}
			throw new Exception("parsing error at state 22: Grammar:1>RuleDefinition:3>RuleDefinitionElement:1>LookAhead:4>#RBracket:0");
		}
		
		
		// Grammar:1>RuleDefinition:3>RuleDefinitionElement:2>RuleDefinitionElement_Multiplier:0>__alt1:0>#AnyMultiplier:0
		int MatchTokenAt_23(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 13;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 24;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 67;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 67;
			}
			if (context.TokenScanner.Match_EOL(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 68;
			}
			throw new Exception("parsing error at state 23: Grammar:1>RuleDefinition:3>RuleDefinitionElement:2>RuleDefinitionElement_Multiplier:0>__alt1:0>#AnyMultiplier:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:0>#LParen:0
		int MatchTokenAt_24(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 25;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 36;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 65;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 65;
			}
			throw new Exception("parsing error at state 24: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:0>#LParen:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:0>#LParen:0
		int MatchTokenAt_25(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Build(token);
				return 26;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Build(token);
				return 26;
			}
			throw new Exception("parsing error at state 25: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:0>#LParen:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:0>AlternateElementItem:0>__alt3:0>#Rule:0
		int MatchTokenAt_26(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_AlternateOp(token))
			{
				context.Builder.Build(token);
				return 27;
			}
			if (context.TokenScanner.Match_RParen(token))
			{
				context.Builder.Build(token);
				return 28;
			}
			throw new Exception("parsing error at state 26: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:0>AlternateElementItem:0>__alt3:0>#Rule:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:1>__grp2:0>#AlternateOp:0
		int MatchTokenAt_27(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Build(token);
				return 26;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Build(token);
				return 26;
			}
			throw new Exception("parsing error at state 27: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:1>__grp2:0>#AlternateOp:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:2>#RParen:0
		int MatchTokenAt_28(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_LBracket(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Push(RuleType.LookAhead);
				context.Builder.Build(token);
				return 29;
			}
			if (context.TokenScanner.Match_AnyMultiplier(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 35;
			}
			if (context.TokenScanner.Match_OneOrMoreMultiplier(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 35;
			}
			if (context.TokenScanner.Match_OneOrZeroMultiplier(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 35;
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 25;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 36;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 65;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 65;
			}
			if (context.TokenScanner.Match_RParen(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 66;
			}
			throw new Exception("parsing error at state 28: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:2>#RParen:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:0>#LBracket:0
		int MatchTokenAt_29(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Push(RuleType.LookAheadTokenList);
				context.Builder.Build(token);
				return 30;
			}
			if (context.TokenScanner.Match_Arrow(token))
			{
				context.Builder.Build(token);
				return 32;
			}
			throw new Exception("parsing error at state 29: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:0>#LBracket:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:0>#Token:0
		int MatchTokenAt_30(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_AlternateOp(token))
			{
				context.Builder.Build(token);
				return 31;
			}
			if (context.TokenScanner.Match_Arrow(token))
			{
				context.Builder.Pop(RuleType.LookAheadTokenList);
				context.Builder.Build(token);
				return 32;
			}
			throw new Exception("parsing error at state 30: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:0>#Token:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:1>__grp4:0>#AlternateOp:0
		int MatchTokenAt_31(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Build(token);
				return 30;
			}
			throw new Exception("parsing error at state 31: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:1>__grp4:0>#AlternateOp:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:2>#Arrow:0
		int MatchTokenAt_32(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Push(RuleType.LookAheadTokenList);
				context.Builder.Build(token);
				return 33;
			}
			throw new Exception("parsing error at state 32: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:2>#Arrow:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:3>LookAheadTokenList:0>#Token:0
		int MatchTokenAt_33(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_AlternateOp(token))
			{
				context.Builder.Build(token);
				return 31;
			}
			if (context.TokenScanner.Match_RBracket(token))
			{
				context.Builder.Pop(RuleType.LookAheadTokenList);
				context.Builder.Build(token);
				return 34;
			}
			throw new Exception("parsing error at state 33: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:3>LookAheadTokenList:0>#Token:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:4>#RBracket:0
		int MatchTokenAt_34(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_AnyMultiplier(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Build(token);
				return 35;
			}
			if (context.TokenScanner.Match_OneOrMoreMultiplier(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Build(token);
				return 35;
			}
			if (context.TokenScanner.Match_OneOrZeroMultiplier(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Build(token);
				return 35;
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 25;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 36;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 65;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 65;
			}
			if (context.TokenScanner.Match_RParen(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 66;
			}
			throw new Exception("parsing error at state 34: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:4>#RBracket:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:2>RuleDefinitionElement_Multiplier:0>__alt1:0>#AnyMultiplier:0
		int MatchTokenAt_35(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 25;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 36;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 65;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 65;
			}
			if (context.TokenScanner.Match_RParen(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 66;
			}
			throw new Exception("parsing error at state 35: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:2>RuleDefinitionElement_Multiplier:0>__alt1:0>#AnyMultiplier:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:0>#LParen:0
		int MatchTokenAt_36(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 37;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 48;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 63;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 63;
			}
			throw new Exception("parsing error at state 36: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:0>#LParen:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:0>#LParen:0
		int MatchTokenAt_37(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Build(token);
				return 38;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Build(token);
				return 38;
			}
			throw new Exception("parsing error at state 37: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:0>#LParen:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:0>AlternateElementItem:0>__alt3:0>#Rule:0
		int MatchTokenAt_38(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_AlternateOp(token))
			{
				context.Builder.Build(token);
				return 39;
			}
			if (context.TokenScanner.Match_RParen(token))
			{
				context.Builder.Build(token);
				return 40;
			}
			throw new Exception("parsing error at state 38: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:0>AlternateElementItem:0>__alt3:0>#Rule:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:1>__grp2:0>#AlternateOp:0
		int MatchTokenAt_39(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Build(token);
				return 38;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Build(token);
				return 38;
			}
			throw new Exception("parsing error at state 39: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:1>__grp2:0>#AlternateOp:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:2>#RParen:0
		int MatchTokenAt_40(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_LBracket(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Push(RuleType.LookAhead);
				context.Builder.Build(token);
				return 41;
			}
			if (context.TokenScanner.Match_AnyMultiplier(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 47;
			}
			if (context.TokenScanner.Match_OneOrMoreMultiplier(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 47;
			}
			if (context.TokenScanner.Match_OneOrZeroMultiplier(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 47;
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 37;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 48;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 63;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 63;
			}
			if (context.TokenScanner.Match_RParen(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 64;
			}
			throw new Exception("parsing error at state 40: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:2>#RParen:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:0>#LBracket:0
		int MatchTokenAt_41(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Push(RuleType.LookAheadTokenList);
				context.Builder.Build(token);
				return 42;
			}
			if (context.TokenScanner.Match_Arrow(token))
			{
				context.Builder.Build(token);
				return 44;
			}
			throw new Exception("parsing error at state 41: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:0>#LBracket:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:0>#Token:0
		int MatchTokenAt_42(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_AlternateOp(token))
			{
				context.Builder.Build(token);
				return 43;
			}
			if (context.TokenScanner.Match_Arrow(token))
			{
				context.Builder.Pop(RuleType.LookAheadTokenList);
				context.Builder.Build(token);
				return 44;
			}
			throw new Exception("parsing error at state 42: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:0>#Token:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:1>__grp4:0>#AlternateOp:0
		int MatchTokenAt_43(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Build(token);
				return 42;
			}
			throw new Exception("parsing error at state 43: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:1>__grp4:0>#AlternateOp:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:2>#Arrow:0
		int MatchTokenAt_44(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Push(RuleType.LookAheadTokenList);
				context.Builder.Build(token);
				return 45;
			}
			throw new Exception("parsing error at state 44: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:2>#Arrow:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:3>LookAheadTokenList:0>#Token:0
		int MatchTokenAt_45(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_AlternateOp(token))
			{
				context.Builder.Build(token);
				return 43;
			}
			if (context.TokenScanner.Match_RBracket(token))
			{
				context.Builder.Pop(RuleType.LookAheadTokenList);
				context.Builder.Build(token);
				return 46;
			}
			throw new Exception("parsing error at state 45: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:3>LookAheadTokenList:0>#Token:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:4>#RBracket:0
		int MatchTokenAt_46(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_AnyMultiplier(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Build(token);
				return 47;
			}
			if (context.TokenScanner.Match_OneOrMoreMultiplier(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Build(token);
				return 47;
			}
			if (context.TokenScanner.Match_OneOrZeroMultiplier(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Build(token);
				return 47;
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 37;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 48;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 63;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 63;
			}
			if (context.TokenScanner.Match_RParen(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 64;
			}
			throw new Exception("parsing error at state 46: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:4>#RBracket:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:2>RuleDefinitionElement_Multiplier:0>__alt1:0>#AnyMultiplier:0
		int MatchTokenAt_47(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 37;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 48;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 63;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 63;
			}
			if (context.TokenScanner.Match_RParen(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 64;
			}
			throw new Exception("parsing error at state 47: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:2>RuleDefinitionElement_Multiplier:0>__alt1:0>#AnyMultiplier:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:0>#LParen:0
		int MatchTokenAt_48(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 49;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 60;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 61;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 61;
			}
			throw new Exception("parsing error at state 48: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:0>#LParen:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:0>#LParen:0
		int MatchTokenAt_49(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Build(token);
				return 50;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Build(token);
				return 50;
			}
			throw new Exception("parsing error at state 49: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:0>#LParen:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:0>AlternateElementItem:0>__alt3:0>#Rule:0
		int MatchTokenAt_50(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_AlternateOp(token))
			{
				context.Builder.Build(token);
				return 51;
			}
			if (context.TokenScanner.Match_RParen(token))
			{
				context.Builder.Build(token);
				return 52;
			}
			throw new Exception("parsing error at state 50: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:0>AlternateElementItem:0>__alt3:0>#Rule:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:1>__grp2:0>#AlternateOp:0
		int MatchTokenAt_51(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Build(token);
				return 50;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Build(token);
				return 50;
			}
			throw new Exception("parsing error at state 51: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:1>AlternateElementBody:1>__grp2:0>#AlternateOp:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:2>#RParen:0
		int MatchTokenAt_52(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_LBracket(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Push(RuleType.LookAhead);
				context.Builder.Build(token);
				return 53;
			}
			if (context.TokenScanner.Match_AnyMultiplier(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 59;
			}
			if (context.TokenScanner.Match_OneOrMoreMultiplier(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 59;
			}
			if (context.TokenScanner.Match_OneOrZeroMultiplier(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 59;
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 49;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 60;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 61;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 61;
			}
			if (context.TokenScanner.Match_RParen(token))
			{
				context.Builder.Pop(RuleType.AlternateElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 62;
			}
			throw new Exception("parsing error at state 52: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:0>AlternateElement:2>#RParen:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:0>#LBracket:0
		int MatchTokenAt_53(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Push(RuleType.LookAheadTokenList);
				context.Builder.Build(token);
				return 54;
			}
			if (context.TokenScanner.Match_Arrow(token))
			{
				context.Builder.Build(token);
				return 56;
			}
			throw new Exception("parsing error at state 53: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:0>#LBracket:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:0>#Token:0
		int MatchTokenAt_54(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_AlternateOp(token))
			{
				context.Builder.Build(token);
				return 55;
			}
			if (context.TokenScanner.Match_Arrow(token))
			{
				context.Builder.Pop(RuleType.LookAheadTokenList);
				context.Builder.Build(token);
				return 56;
			}
			throw new Exception("parsing error at state 54: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:0>#Token:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:1>__grp4:0>#AlternateOp:0
		int MatchTokenAt_55(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Build(token);
				return 54;
			}
			throw new Exception("parsing error at state 55: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:1>LookAheadTokenList:1>__grp4:0>#AlternateOp:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:2>#Arrow:0
		int MatchTokenAt_56(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Push(RuleType.LookAheadTokenList);
				context.Builder.Build(token);
				return 57;
			}
			throw new Exception("parsing error at state 56: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:2>#Arrow:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:3>LookAheadTokenList:0>#Token:0
		int MatchTokenAt_57(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_AlternateOp(token))
			{
				context.Builder.Build(token);
				return 55;
			}
			if (context.TokenScanner.Match_RBracket(token))
			{
				context.Builder.Pop(RuleType.LookAheadTokenList);
				context.Builder.Build(token);
				return 58;
			}
			throw new Exception("parsing error at state 57: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:3>LookAheadTokenList:0>#Token:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:4>#RBracket:0
		int MatchTokenAt_58(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_AnyMultiplier(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Build(token);
				return 59;
			}
			if (context.TokenScanner.Match_OneOrMoreMultiplier(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Build(token);
				return 59;
			}
			if (context.TokenScanner.Match_OneOrZeroMultiplier(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Build(token);
				return 59;
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 49;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 60;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 61;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 61;
			}
			if (context.TokenScanner.Match_RParen(token))
			{
				context.Builder.Pop(RuleType.LookAhead);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 62;
			}
			throw new Exception("parsing error at state 58: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:1>LookAhead:4>#RBracket:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:2>RuleDefinitionElement_Multiplier:0>__alt1:0>#AnyMultiplier:0
		int MatchTokenAt_59(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 49;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 60;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 61;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 61;
			}
			if (context.TokenScanner.Match_RParen(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 62;
			}
			throw new Exception("parsing error at state 59: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:1>RuleDefinitionElement:2>RuleDefinitionElement_Multiplier:0>__alt1:0>#AnyMultiplier:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:1>TokenElement:0>#Token:0
		int MatchTokenAt_61(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_LBracket(token))
			{
				context.Builder.Push(RuleType.LookAhead);
				context.Builder.Build(token);
				return 53;
			}
			if (context.TokenScanner.Match_AnyMultiplier(token))
			{
				context.Builder.Build(token);
				return 59;
			}
			if (context.TokenScanner.Match_OneOrMoreMultiplier(token))
			{
				context.Builder.Build(token);
				return 59;
			}
			if (context.TokenScanner.Match_OneOrZeroMultiplier(token))
			{
				context.Builder.Build(token);
				return 59;
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 49;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 60;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 61;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 61;
			}
			if (context.TokenScanner.Match_RParen(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 62;
			}
			throw new Exception("parsing error at state 61: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:1>TokenElement:0>#Token:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:3>#RParen:0
		int MatchTokenAt_62(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_LBracket(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Push(RuleType.LookAhead);
				context.Builder.Build(token);
				return 41;
			}
			if (context.TokenScanner.Match_AnyMultiplier(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Build(token);
				return 47;
			}
			if (context.TokenScanner.Match_OneOrMoreMultiplier(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Build(token);
				return 47;
			}
			if (context.TokenScanner.Match_OneOrZeroMultiplier(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Build(token);
				return 47;
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 37;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 48;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 63;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 63;
			}
			if (context.TokenScanner.Match_RParen(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 64;
			}
			throw new Exception("parsing error at state 62: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:3>#RParen:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:1>TokenElement:0>#Token:0
		int MatchTokenAt_63(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_LBracket(token))
			{
				context.Builder.Push(RuleType.LookAhead);
				context.Builder.Build(token);
				return 41;
			}
			if (context.TokenScanner.Match_AnyMultiplier(token))
			{
				context.Builder.Build(token);
				return 47;
			}
			if (context.TokenScanner.Match_OneOrMoreMultiplier(token))
			{
				context.Builder.Build(token);
				return 47;
			}
			if (context.TokenScanner.Match_OneOrZeroMultiplier(token))
			{
				context.Builder.Build(token);
				return 47;
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 37;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 48;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 63;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 63;
			}
			if (context.TokenScanner.Match_RParen(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 64;
			}
			throw new Exception("parsing error at state 63: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:1>TokenElement:0>#Token:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:3>#RParen:0
		int MatchTokenAt_64(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_LBracket(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Push(RuleType.LookAhead);
				context.Builder.Build(token);
				return 29;
			}
			if (context.TokenScanner.Match_AnyMultiplier(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Build(token);
				return 35;
			}
			if (context.TokenScanner.Match_OneOrMoreMultiplier(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Build(token);
				return 35;
			}
			if (context.TokenScanner.Match_OneOrZeroMultiplier(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Build(token);
				return 35;
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 25;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 36;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 65;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 65;
			}
			if (context.TokenScanner.Match_RParen(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 66;
			}
			throw new Exception("parsing error at state 64: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:3>#RParen:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:1>TokenElement:0>#Token:0
		int MatchTokenAt_65(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_LBracket(token))
			{
				context.Builder.Push(RuleType.LookAhead);
				context.Builder.Build(token);
				return 29;
			}
			if (context.TokenScanner.Match_AnyMultiplier(token))
			{
				context.Builder.Build(token);
				return 35;
			}
			if (context.TokenScanner.Match_OneOrMoreMultiplier(token))
			{
				context.Builder.Build(token);
				return 35;
			}
			if (context.TokenScanner.Match_OneOrZeroMultiplier(token))
			{
				context.Builder.Build(token);
				return 35;
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 25;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 36;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 65;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 65;
			}
			if (context.TokenScanner.Match_RParen(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 66;
			}
			throw new Exception("parsing error at state 65: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:2>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:1>TokenElement:0>#Token:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:3>#RParen:0
		int MatchTokenAt_66(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_LBracket(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Push(RuleType.LookAhead);
				context.Builder.Build(token);
				return 17;
			}
			if (context.TokenScanner.Match_AnyMultiplier(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Build(token);
				return 23;
			}
			if (context.TokenScanner.Match_OneOrMoreMultiplier(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Build(token);
				return 23;
			}
			if (context.TokenScanner.Match_OneOrZeroMultiplier(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Build(token);
				return 23;
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 13;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 24;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 67;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 67;
			}
			if (context.TokenScanner.Match_EOL(token))
			{
				context.Builder.Pop(RuleType.GroupElement);
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 68;
			}
			throw new Exception("parsing error at state 66: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:3>GroupElement:3>#RParen:0");
		}
		
		
		// Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:1>TokenElement:0>#Token:0
		int MatchTokenAt_67(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_LBracket(token))
			{
				context.Builder.Push(RuleType.LookAhead);
				context.Builder.Build(token);
				return 17;
			}
			if (context.TokenScanner.Match_AnyMultiplier(token))
			{
				context.Builder.Build(token);
				return 23;
			}
			if (context.TokenScanner.Match_OneOrMoreMultiplier(token))
			{
				context.Builder.Build(token);
				return 23;
			}
			if (context.TokenScanner.Match_OneOrZeroMultiplier(token))
			{
				context.Builder.Build(token);
				return 23;
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.AlternateElement);
				context.Builder.Build(token);
				return 13;
				}
			}
			if (context.TokenScanner.Match_LParen(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.GroupElement);
				context.Builder.Build(token);
				return 24;
			}
			if (context.TokenScanner.Match_Token(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 67;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Push(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 67;
			}
			if (context.TokenScanner.Match_EOL(token))
			{
				context.Builder.Pop(RuleType.RuleDefinitionElement);
				context.Builder.Build(token);
				return 68;
			}
			throw new Exception("parsing error at state 67: Grammar:1>RuleDefinition:4>RuleDefinitionElement:0>RuleDefinitionElement_Core:0>__alt0:1>TokenElement:0>#Token:0");
		}
		
		
		// Grammar:1>RuleDefinition:5>#EOL:0
		int MatchTokenAt_68(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOF(token))
			{
				context.Builder.Pop(RuleType.RuleDefinition);
				context.Builder.Build(token);
				return 60;
			}
			if (context.TokenScanner.Match_Rule(token))
			{
				context.Builder.Pop(RuleType.RuleDefinition);
				context.Builder.Push(RuleType.RuleDefinition);
				context.Builder.Build(token);
				return 10;
			}
			throw new Exception("parsing error at state 68: Grammar:1>RuleDefinition:5>#EOL:0");
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
					|| context.TokenScanner.Match_AlternateOp(token)
				)
		        {
					match = true;
					break;
		        }
		    } while (false
				|| context.TokenScanner.Match_Token(token)
				|| context.TokenScanner.Match_Rule(token)
			);
			foreach(var t in queue)
				context.TokenQueue.Enqueue(t);
			return match;
		}
		
	}
}