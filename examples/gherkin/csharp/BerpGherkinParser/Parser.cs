using System;
using System.Collections.Generic;
namespace BerpGherkinParser
{
	public enum TokenType
	{
		None,
EOF,
Empty,
Comment,
TagLine,
Feature,
Background,
Scenario,
ScenarioOutline,
Examples,
Step,
MultiLineArgument,
TableRow,
Other,
	}

	public enum RuleType
	{
		None,
_EOF, // #EOF
_Empty, // #Empty
_Comment, // #Comment
_TagLine, // #TagLine
_Feature, // #Feature
_Background, // #Background
_Scenario, // #Scenario
_ScenarioOutline, // #ScenarioOutline
_Examples, // #Examples
_Step, // #Step
_MultiLineArgument, // #MultiLineArgument
_TableRow, // #TableRow
_Other, // #Other
Feature_File, // Feature_File! := Feature_Def Background? Scenario_Base*
Feature_Def, // Feature_Def! := #TagLine* #Feature Feature_Description
Background, // Background! := #Background Background_Description Scenario_Step*
Scenario_Base, // Scenario_Base! := #TagLine* Scenario_Base_Body
Scenario_Base_Body, // Scenario_Base_Body := __alt0
Scenario, // Scenario! := #Scenario Scenario_Description Scenario_Step*
ScenarioOutline, // ScenarioOutline! := #ScenarioOutline ScenarioOutline_Description ScenarioOutline_Step* Examples+
Examples, // Examples! := #TagLine[#Empty|#Comment|#TagLine-&gt;#Examples]* #Examples Examples_Description Examples_Table
Examples_Table, // Examples_Table! := #TableRow+
Scenario_Step, // Scenario_Step := Step
ScenarioOutline_Step, // ScenarioOutline_Step := Step
Step, // Step! := #Step Step_Arg?
Step_Arg, // Step_Arg := __alt1
Table_And_Multiline_Arg, // Table_And_Multiline_Arg := Table_Arg Multiline_Arg?
Multiline_And_Table_Arg, // Multiline_And_Table_Arg := Multiline_Arg Table_Arg?
Table_Arg, // Table_Arg! := #TableRow+
Multiline_Arg, // Multiline_Arg! := #MultiLineArgument Multiline_Arg_Line* #MultiLineArgument
Multiline_Arg_Line, // Multiline_Arg_Line := __alt2
Feature_Description, // Feature_Description := Description_Helper
Background_Description, // Background_Description := Description_Helper
Scenario_Description, // Scenario_Description := Description_Helper
ScenarioOutline_Description, // ScenarioOutline_Description := Description_Helper
Examples_Description, // Examples_Description := Description_Helper
Description_Helper, // Description_Helper := Description? #Comment*
Description, // Description! := Description_Line+
Description_Line, // Description_Line := __alt3
__alt0, // __alt0 := (Scenario | ScenarioOutline)
__alt1, // __alt1 := (Table_And_Multiline_Arg | Multiline_And_Table_Arg)
__alt2, // __alt2 := (#Empty | #Other)
__alt3, // __alt3 := (#Empty | #Other)
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

			context.Builder.Push(RuleType.Feature_File);
            int state = 0;
            Token token;
            do
			{
				token = ReadToken(context);
				state = MatchToken(state, token, context);
            } while(!token.IsEOF);

			if (state != 32)
			{
				throw new Exception("parsing error: end of file expected");
			}

			context.Builder.Pop(RuleType.Feature_File);
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
				default:
					throw new NotImplementedException();
			}
			return newState;
		}

		
		// Start
		int MatchTokenAt_0(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Push(RuleType.Feature_Def);
				context.Builder.Build(token);
				return 1;
			}
			if (context.TokenScanner.Match_Feature(token))
			{
				context.Builder.Push(RuleType.Feature_Def);
				context.Builder.Build(token);
				return 2;
			}
			throw new Exception("parsing error at state 0: Start");
		}
		
		
		// Feature_File:0>Feature_Def:0>#TagLine:0
		int MatchTokenAt_1(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Build(token);
				return 1;
			}
			if (context.TokenScanner.Match_Feature(token))
			{
				context.Builder.Build(token);
				return 2;
			}
			throw new Exception("parsing error at state 1: Feature_File:0>Feature_Def:0>#TagLine:0");
		}
		
		
		// Feature_File:0>Feature_Def:1>#Feature:0
		int MatchTokenAt_2(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOF(token))
			{
				context.Builder.Pop(RuleType.Feature_Def);
				context.Builder.Build(token);
				return 32;
			}
			if (context.TokenScanner.Match_Empty(token))
			{
				context.Builder.Push(RuleType.Description);
				context.Builder.Build(token);
				return 3;
			}
			if (context.TokenScanner.Match_Comment(token))
			{
				context.Builder.Build(token);
				return 4;
			}
			if (context.TokenScanner.Match_Background(token))
			{
				context.Builder.Pop(RuleType.Feature_Def);
				context.Builder.Push(RuleType.Background);
				context.Builder.Build(token);
				return 5;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Feature_Def);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 12;
			}
			if (context.TokenScanner.Match_Scenario(token))
			{
				context.Builder.Pop(RuleType.Feature_Def);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario);
				context.Builder.Build(token);
				return 13;
			}
			if (context.TokenScanner.Match_ScenarioOutline(token))
			{
				context.Builder.Pop(RuleType.Feature_Def);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.ScenarioOutline);
				context.Builder.Build(token);
				return 20;
			}
			if (context.TokenScanner.Match_Other(token))
			{
				context.Builder.Push(RuleType.Description);
				context.Builder.Build(token);
				return 3;
			}
			throw new Exception("parsing error at state 2: Feature_File:0>Feature_Def:1>#Feature:0");
		}
		
		
		// Feature_File:0>Feature_Def:2>Feature_Description:0>Description_Helper:0>Description:0>Description_Line:0>__alt3:0>#Empty:0
		int MatchTokenAt_3(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOF(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Pop(RuleType.Feature_Def);
				context.Builder.Build(token);
				return 32;
			}
			if (context.TokenScanner.Match_Empty(token))
			{
				context.Builder.Build(token);
				return 3;
			}
			if (context.TokenScanner.Match_Comment(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Build(token);
				return 4;
			}
			if (context.TokenScanner.Match_Background(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Pop(RuleType.Feature_Def);
				context.Builder.Push(RuleType.Background);
				context.Builder.Build(token);
				return 5;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Pop(RuleType.Feature_Def);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 12;
			}
			if (context.TokenScanner.Match_Scenario(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Pop(RuleType.Feature_Def);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario);
				context.Builder.Build(token);
				return 13;
			}
			if (context.TokenScanner.Match_ScenarioOutline(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Pop(RuleType.Feature_Def);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.ScenarioOutline);
				context.Builder.Build(token);
				return 20;
			}
			if (context.TokenScanner.Match_Other(token))
			{
				context.Builder.Build(token);
				return 3;
			}
			throw new Exception("parsing error at state 3: Feature_File:0>Feature_Def:2>Feature_Description:0>Description_Helper:0>Description:0>Description_Line:0>__alt3:0>#Empty:0");
		}
		
		
		// Feature_File:0>Feature_Def:2>Feature_Description:0>Description_Helper:1>#Comment:0
		int MatchTokenAt_4(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOF(token))
			{
				context.Builder.Pop(RuleType.Feature_Def);
				context.Builder.Build(token);
				return 32;
			}
			if (context.TokenScanner.Match_Comment(token))
			{
				context.Builder.Build(token);
				return 4;
			}
			if (context.TokenScanner.Match_Background(token))
			{
				context.Builder.Pop(RuleType.Feature_Def);
				context.Builder.Push(RuleType.Background);
				context.Builder.Build(token);
				return 5;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Feature_Def);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 12;
			}
			if (context.TokenScanner.Match_Scenario(token))
			{
				context.Builder.Pop(RuleType.Feature_Def);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario);
				context.Builder.Build(token);
				return 13;
			}
			if (context.TokenScanner.Match_ScenarioOutline(token))
			{
				context.Builder.Pop(RuleType.Feature_Def);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.ScenarioOutline);
				context.Builder.Build(token);
				return 20;
			}
			throw new Exception("parsing error at state 4: Feature_File:0>Feature_Def:2>Feature_Description:0>Description_Helper:1>#Comment:0");
		}
		
		
		// Feature_File:1>Background:0>#Background:0
		int MatchTokenAt_5(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOF(token))
			{
				context.Builder.Pop(RuleType.Background);
				context.Builder.Build(token);
				return 32;
			}
			if (context.TokenScanner.Match_Empty(token))
			{
				context.Builder.Push(RuleType.Description);
				context.Builder.Build(token);
				return 6;
			}
			if (context.TokenScanner.Match_Comment(token))
			{
				context.Builder.Build(token);
				return 7;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 8;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 12;
			}
			if (context.TokenScanner.Match_Scenario(token))
			{
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario);
				context.Builder.Build(token);
				return 13;
			}
			if (context.TokenScanner.Match_ScenarioOutline(token))
			{
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.ScenarioOutline);
				context.Builder.Build(token);
				return 20;
			}
			if (context.TokenScanner.Match_Other(token))
			{
				context.Builder.Push(RuleType.Description);
				context.Builder.Build(token);
				return 6;
			}
			throw new Exception("parsing error at state 5: Feature_File:1>Background:0>#Background:0");
		}
		
		
		// Feature_File:1>Background:1>Background_Description:0>Description_Helper:0>Description:0>Description_Line:0>__alt3:0>#Empty:0
		int MatchTokenAt_6(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOF(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Build(token);
				return 32;
			}
			if (context.TokenScanner.Match_Empty(token))
			{
				context.Builder.Build(token);
				return 6;
			}
			if (context.TokenScanner.Match_Comment(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Build(token);
				return 7;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 8;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 12;
			}
			if (context.TokenScanner.Match_Scenario(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario);
				context.Builder.Build(token);
				return 13;
			}
			if (context.TokenScanner.Match_ScenarioOutline(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.ScenarioOutline);
				context.Builder.Build(token);
				return 20;
			}
			if (context.TokenScanner.Match_Other(token))
			{
				context.Builder.Build(token);
				return 6;
			}
			throw new Exception("parsing error at state 6: Feature_File:1>Background:1>Background_Description:0>Description_Helper:0>Description:0>Description_Line:0>__alt3:0>#Empty:0");
		}
		
		
		// Feature_File:1>Background:1>Background_Description:0>Description_Helper:1>#Comment:0
		int MatchTokenAt_7(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOF(token))
			{
				context.Builder.Pop(RuleType.Background);
				context.Builder.Build(token);
				return 32;
			}
			if (context.TokenScanner.Match_Comment(token))
			{
				context.Builder.Build(token);
				return 7;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 8;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 12;
			}
			if (context.TokenScanner.Match_Scenario(token))
			{
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario);
				context.Builder.Build(token);
				return 13;
			}
			if (context.TokenScanner.Match_ScenarioOutline(token))
			{
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.ScenarioOutline);
				context.Builder.Build(token);
				return 20;
			}
			throw new Exception("parsing error at state 7: Feature_File:1>Background:1>Background_Description:0>Description_Helper:1>#Comment:0");
		}
		
		
		// Feature_File:1>Background:2>Scenario_Step:0>Step:0>#Step:0
		int MatchTokenAt_8(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOF(token))
			{
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Build(token);
				return 32;
			}
			if (context.TokenScanner.Match_TableRow(token))
			{
				context.Builder.Push(RuleType.Table_Arg);
				context.Builder.Build(token);
				return 9;
			}
			if (context.TokenScanner.Match_MultiLineArgument(token))
			{
				context.Builder.Push(RuleType.Multiline_Arg);
				context.Builder.Build(token);
				return 39;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 8;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 12;
			}
			if (context.TokenScanner.Match_Scenario(token))
			{
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario);
				context.Builder.Build(token);
				return 13;
			}
			if (context.TokenScanner.Match_ScenarioOutline(token))
			{
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.ScenarioOutline);
				context.Builder.Build(token);
				return 20;
			}
			throw new Exception("parsing error at state 8: Feature_File:1>Background:2>Scenario_Step:0>Step:0>#Step:0");
		}
		
		
		// Feature_File:1>Background:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:0>Table_And_Multiline_Arg:0>Table_Arg:0>#TableRow:0
		int MatchTokenAt_9(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOF(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Build(token);
				return 32;
			}
			if (context.TokenScanner.Match_TableRow(token))
			{
				context.Builder.Build(token);
				return 9;
			}
			if (context.TokenScanner.Match_MultiLineArgument(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Push(RuleType.Multiline_Arg);
				context.Builder.Build(token);
				return 10;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 8;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 12;
			}
			if (context.TokenScanner.Match_Scenario(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario);
				context.Builder.Build(token);
				return 13;
			}
			if (context.TokenScanner.Match_ScenarioOutline(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.ScenarioOutline);
				context.Builder.Build(token);
				return 20;
			}
			throw new Exception("parsing error at state 9: Feature_File:1>Background:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:0>Table_And_Multiline_Arg:0>Table_Arg:0>#TableRow:0");
		}
		
		
		// Feature_File:1>Background:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:0>Table_And_Multiline_Arg:1>Multiline_Arg:0>#MultiLineArgument:0
		int MatchTokenAt_10(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Empty(token))
			{
				context.Builder.Build(token);
				return 10;
			}
			if (context.TokenScanner.Match_MultiLineArgument(token))
			{
				context.Builder.Build(token);
				return 11;
			}
			if (context.TokenScanner.Match_Other(token))
			{
				context.Builder.Build(token);
				return 10;
			}
			throw new Exception("parsing error at state 10: Feature_File:1>Background:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:0>Table_And_Multiline_Arg:1>Multiline_Arg:0>#MultiLineArgument:0");
		}
		
		
		// Feature_File:1>Background:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:0>Table_And_Multiline_Arg:1>Multiline_Arg:2>#MultiLineArgument:0
		int MatchTokenAt_11(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOF(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Build(token);
				return 32;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 8;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 12;
			}
			if (context.TokenScanner.Match_Scenario(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario);
				context.Builder.Build(token);
				return 13;
			}
			if (context.TokenScanner.Match_ScenarioOutline(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.ScenarioOutline);
				context.Builder.Build(token);
				return 20;
			}
			throw new Exception("parsing error at state 11: Feature_File:1>Background:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:0>Table_And_Multiline_Arg:1>Multiline_Arg:2>#MultiLineArgument:0");
		}
		
		
		// Feature_File:2>Scenario_Base:0>#TagLine:0
		int MatchTokenAt_12(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Build(token);
				return 12;
			}
			if (context.TokenScanner.Match_Scenario(token))
			{
				context.Builder.Push(RuleType.Scenario);
				context.Builder.Build(token);
				return 13;
			}
			if (context.TokenScanner.Match_ScenarioOutline(token))
			{
				context.Builder.Push(RuleType.ScenarioOutline);
				context.Builder.Build(token);
				return 20;
			}
			throw new Exception("parsing error at state 12: Feature_File:2>Scenario_Base:0>#TagLine:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:0>Scenario:0>#Scenario:0
		int MatchTokenAt_13(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOF(token))
			{
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 32;
			}
			if (context.TokenScanner.Match_Empty(token))
			{
				context.Builder.Push(RuleType.Description);
				context.Builder.Build(token);
				return 14;
			}
			if (context.TokenScanner.Match_Comment(token))
			{
				context.Builder.Build(token);
				return 15;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 16;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 12;
			}
			if (context.TokenScanner.Match_Scenario(token))
			{
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario);
				context.Builder.Build(token);
				return 13;
			}
			if (context.TokenScanner.Match_ScenarioOutline(token))
			{
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.ScenarioOutline);
				context.Builder.Build(token);
				return 20;
			}
			if (context.TokenScanner.Match_Other(token))
			{
				context.Builder.Push(RuleType.Description);
				context.Builder.Build(token);
				return 14;
			}
			throw new Exception("parsing error at state 13: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:0>Scenario:0>#Scenario:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:0>Scenario:1>Scenario_Description:0>Description_Helper:0>Description:0>Description_Line:0>__alt3:0>#Empty:0
		int MatchTokenAt_14(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOF(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 32;
			}
			if (context.TokenScanner.Match_Empty(token))
			{
				context.Builder.Build(token);
				return 14;
			}
			if (context.TokenScanner.Match_Comment(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Build(token);
				return 15;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 16;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 12;
			}
			if (context.TokenScanner.Match_Scenario(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario);
				context.Builder.Build(token);
				return 13;
			}
			if (context.TokenScanner.Match_ScenarioOutline(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.ScenarioOutline);
				context.Builder.Build(token);
				return 20;
			}
			if (context.TokenScanner.Match_Other(token))
			{
				context.Builder.Build(token);
				return 14;
			}
			throw new Exception("parsing error at state 14: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:0>Scenario:1>Scenario_Description:0>Description_Helper:0>Description:0>Description_Line:0>__alt3:0>#Empty:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:0>Scenario:1>Scenario_Description:0>Description_Helper:1>#Comment:0
		int MatchTokenAt_15(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOF(token))
			{
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 32;
			}
			if (context.TokenScanner.Match_Comment(token))
			{
				context.Builder.Build(token);
				return 15;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 16;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 12;
			}
			if (context.TokenScanner.Match_Scenario(token))
			{
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario);
				context.Builder.Build(token);
				return 13;
			}
			if (context.TokenScanner.Match_ScenarioOutline(token))
			{
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.ScenarioOutline);
				context.Builder.Build(token);
				return 20;
			}
			throw new Exception("parsing error at state 15: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:0>Scenario:1>Scenario_Description:0>Description_Helper:1>#Comment:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:0>Scenario:2>Scenario_Step:0>Step:0>#Step:0
		int MatchTokenAt_16(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOF(token))
			{
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 32;
			}
			if (context.TokenScanner.Match_TableRow(token))
			{
				context.Builder.Push(RuleType.Table_Arg);
				context.Builder.Build(token);
				return 17;
			}
			if (context.TokenScanner.Match_MultiLineArgument(token))
			{
				context.Builder.Push(RuleType.Multiline_Arg);
				context.Builder.Build(token);
				return 36;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 16;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 12;
			}
			if (context.TokenScanner.Match_Scenario(token))
			{
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario);
				context.Builder.Build(token);
				return 13;
			}
			if (context.TokenScanner.Match_ScenarioOutline(token))
			{
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.ScenarioOutline);
				context.Builder.Build(token);
				return 20;
			}
			throw new Exception("parsing error at state 16: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:0>Scenario:2>Scenario_Step:0>Step:0>#Step:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:0>Scenario:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:0>Table_And_Multiline_Arg:0>Table_Arg:0>#TableRow:0
		int MatchTokenAt_17(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOF(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 32;
			}
			if (context.TokenScanner.Match_TableRow(token))
			{
				context.Builder.Build(token);
				return 17;
			}
			if (context.TokenScanner.Match_MultiLineArgument(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Push(RuleType.Multiline_Arg);
				context.Builder.Build(token);
				return 18;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 16;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 12;
			}
			if (context.TokenScanner.Match_Scenario(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario);
				context.Builder.Build(token);
				return 13;
			}
			if (context.TokenScanner.Match_ScenarioOutline(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.ScenarioOutline);
				context.Builder.Build(token);
				return 20;
			}
			throw new Exception("parsing error at state 17: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:0>Scenario:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:0>Table_And_Multiline_Arg:0>Table_Arg:0>#TableRow:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:0>Scenario:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:0>Table_And_Multiline_Arg:1>Multiline_Arg:0>#MultiLineArgument:0
		int MatchTokenAt_18(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Empty(token))
			{
				context.Builder.Build(token);
				return 18;
			}
			if (context.TokenScanner.Match_MultiLineArgument(token))
			{
				context.Builder.Build(token);
				return 19;
			}
			if (context.TokenScanner.Match_Other(token))
			{
				context.Builder.Build(token);
				return 18;
			}
			throw new Exception("parsing error at state 18: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:0>Scenario:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:0>Table_And_Multiline_Arg:1>Multiline_Arg:0>#MultiLineArgument:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:0>Scenario:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:0>Table_And_Multiline_Arg:1>Multiline_Arg:2>#MultiLineArgument:0
		int MatchTokenAt_19(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOF(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 32;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 16;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 12;
			}
			if (context.TokenScanner.Match_Scenario(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario);
				context.Builder.Build(token);
				return 13;
			}
			if (context.TokenScanner.Match_ScenarioOutline(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.ScenarioOutline);
				context.Builder.Build(token);
				return 20;
			}
			throw new Exception("parsing error at state 19: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:0>Scenario:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:0>Table_And_Multiline_Arg:1>Multiline_Arg:2>#MultiLineArgument:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:0>#ScenarioOutline:0
		int MatchTokenAt_20(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Empty(token))
			{
				context.Builder.Push(RuleType.Description);
				context.Builder.Build(token);
				return 21;
			}
			if (context.TokenScanner.Match_Comment(token))
			{
				context.Builder.Build(token);
				return 22;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 23;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Push(RuleType.Examples);
				context.Builder.Build(token);
				return 27;
			}
			if (context.TokenScanner.Match_Examples(token))
			{
				context.Builder.Push(RuleType.Examples);
				context.Builder.Build(token);
				return 28;
			}
			if (context.TokenScanner.Match_Other(token))
			{
				context.Builder.Push(RuleType.Description);
				context.Builder.Build(token);
				return 21;
			}
			throw new Exception("parsing error at state 20: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:0>#ScenarioOutline:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:1>ScenarioOutline_Description:0>Description_Helper:0>Description:0>Description_Line:0>__alt3:0>#Empty:0
		int MatchTokenAt_21(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Empty(token))
			{
				context.Builder.Build(token);
				return 21;
			}
			if (context.TokenScanner.Match_Comment(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Build(token);
				return 22;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 23;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Push(RuleType.Examples);
				context.Builder.Build(token);
				return 27;
			}
			if (context.TokenScanner.Match_Examples(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Push(RuleType.Examples);
				context.Builder.Build(token);
				return 28;
			}
			if (context.TokenScanner.Match_Other(token))
			{
				context.Builder.Build(token);
				return 21;
			}
			throw new Exception("parsing error at state 21: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:1>ScenarioOutline_Description:0>Description_Helper:0>Description:0>Description_Line:0>__alt3:0>#Empty:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:1>ScenarioOutline_Description:0>Description_Helper:1>#Comment:0
		int MatchTokenAt_22(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Comment(token))
			{
				context.Builder.Build(token);
				return 22;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 23;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Push(RuleType.Examples);
				context.Builder.Build(token);
				return 27;
			}
			if (context.TokenScanner.Match_Examples(token))
			{
				context.Builder.Push(RuleType.Examples);
				context.Builder.Build(token);
				return 28;
			}
			throw new Exception("parsing error at state 22: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:1>ScenarioOutline_Description:0>Description_Helper:1>#Comment:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:2>ScenarioOutline_Step:0>Step:0>#Step:0
		int MatchTokenAt_23(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_TableRow(token))
			{
				context.Builder.Push(RuleType.Table_Arg);
				context.Builder.Build(token);
				return 24;
			}
			if (context.TokenScanner.Match_MultiLineArgument(token))
			{
				context.Builder.Push(RuleType.Multiline_Arg);
				context.Builder.Build(token);
				return 33;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 23;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Examples);
				context.Builder.Build(token);
				return 27;
			}
			if (context.TokenScanner.Match_Examples(token))
			{
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Examples);
				context.Builder.Build(token);
				return 28;
			}
			throw new Exception("parsing error at state 23: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:2>ScenarioOutline_Step:0>Step:0>#Step:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:2>ScenarioOutline_Step:0>Step:1>Step_Arg:0>__alt1:0>Table_And_Multiline_Arg:0>Table_Arg:0>#TableRow:0
		int MatchTokenAt_24(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_TableRow(token))
			{
				context.Builder.Build(token);
				return 24;
			}
			if (context.TokenScanner.Match_MultiLineArgument(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Push(RuleType.Multiline_Arg);
				context.Builder.Build(token);
				return 25;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 23;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Examples);
				context.Builder.Build(token);
				return 27;
			}
			if (context.TokenScanner.Match_Examples(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Examples);
				context.Builder.Build(token);
				return 28;
			}
			throw new Exception("parsing error at state 24: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:2>ScenarioOutline_Step:0>Step:1>Step_Arg:0>__alt1:0>Table_And_Multiline_Arg:0>Table_Arg:0>#TableRow:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:2>ScenarioOutline_Step:0>Step:1>Step_Arg:0>__alt1:0>Table_And_Multiline_Arg:1>Multiline_Arg:0>#MultiLineArgument:0
		int MatchTokenAt_25(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Empty(token))
			{
				context.Builder.Build(token);
				return 25;
			}
			if (context.TokenScanner.Match_MultiLineArgument(token))
			{
				context.Builder.Build(token);
				return 26;
			}
			if (context.TokenScanner.Match_Other(token))
			{
				context.Builder.Build(token);
				return 25;
			}
			throw new Exception("parsing error at state 25: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:2>ScenarioOutline_Step:0>Step:1>Step_Arg:0>__alt1:0>Table_And_Multiline_Arg:1>Multiline_Arg:0>#MultiLineArgument:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:2>ScenarioOutline_Step:0>Step:1>Step_Arg:0>__alt1:0>Table_And_Multiline_Arg:1>Multiline_Arg:2>#MultiLineArgument:0
		int MatchTokenAt_26(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 23;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Examples);
				context.Builder.Build(token);
				return 27;
			}
			if (context.TokenScanner.Match_Examples(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Examples);
				context.Builder.Build(token);
				return 28;
			}
			throw new Exception("parsing error at state 26: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:2>ScenarioOutline_Step:0>Step:1>Step_Arg:0>__alt1:0>Table_And_Multiline_Arg:1>Multiline_Arg:2>#MultiLineArgument:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:3>Examples:0>#TagLine:0
		int MatchTokenAt_27(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Build(token);
				return 27;
			}
			if (context.TokenScanner.Match_Examples(token))
			{
				context.Builder.Build(token);
				return 28;
			}
			throw new Exception("parsing error at state 27: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:3>Examples:0>#TagLine:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:3>Examples:1>#Examples:0
		int MatchTokenAt_28(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Empty(token))
			{
				context.Builder.Push(RuleType.Description);
				context.Builder.Build(token);
				return 29;
			}
			if (context.TokenScanner.Match_Comment(token))
			{
				context.Builder.Build(token);
				return 30;
			}
			if (context.TokenScanner.Match_TableRow(token))
			{
				context.Builder.Push(RuleType.Examples_Table);
				context.Builder.Build(token);
				return 31;
			}
			if (context.TokenScanner.Match_Other(token))
			{
				context.Builder.Push(RuleType.Description);
				context.Builder.Build(token);
				return 29;
			}
			throw new Exception("parsing error at state 28: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:3>Examples:1>#Examples:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:3>Examples:2>Examples_Description:0>Description_Helper:0>Description:0>Description_Line:0>__alt3:0>#Empty:0
		int MatchTokenAt_29(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Empty(token))
			{
				context.Builder.Build(token);
				return 29;
			}
			if (context.TokenScanner.Match_Comment(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Build(token);
				return 30;
			}
			if (context.TokenScanner.Match_TableRow(token))
			{
				context.Builder.Pop(RuleType.Description);
				context.Builder.Push(RuleType.Examples_Table);
				context.Builder.Build(token);
				return 31;
			}
			if (context.TokenScanner.Match_Other(token))
			{
				context.Builder.Build(token);
				return 29;
			}
			throw new Exception("parsing error at state 29: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:3>Examples:2>Examples_Description:0>Description_Helper:0>Description:0>Description_Line:0>__alt3:0>#Empty:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:3>Examples:2>Examples_Description:0>Description_Helper:1>#Comment:0
		int MatchTokenAt_30(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Comment(token))
			{
				context.Builder.Build(token);
				return 30;
			}
			if (context.TokenScanner.Match_TableRow(token))
			{
				context.Builder.Push(RuleType.Examples_Table);
				context.Builder.Build(token);
				return 31;
			}
			throw new Exception("parsing error at state 30: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:3>Examples:2>Examples_Description:0>Description_Helper:1>#Comment:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:3>Examples:3>Examples_Table:0>#TableRow:0
		int MatchTokenAt_31(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOF(token))
			{
				context.Builder.Pop(RuleType.Examples_Table);
				context.Builder.Pop(RuleType.Examples);
				context.Builder.Pop(RuleType.ScenarioOutline);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 32;
			}
			if (context.TokenScanner.Match_TableRow(token))
			{
				context.Builder.Build(token);
				return 31;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				if (LookAhead_0(context, token))
				{
				context.Builder.Pop(RuleType.Examples_Table);
				context.Builder.Pop(RuleType.Examples);
				context.Builder.Push(RuleType.Examples);
				context.Builder.Build(token);
				return 27;
				}
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Examples_Table);
				context.Builder.Pop(RuleType.Examples);
				context.Builder.Pop(RuleType.ScenarioOutline);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 12;
			}
			if (context.TokenScanner.Match_Examples(token))
			{
				context.Builder.Pop(RuleType.Examples_Table);
				context.Builder.Pop(RuleType.Examples);
				context.Builder.Push(RuleType.Examples);
				context.Builder.Build(token);
				return 28;
			}
			if (context.TokenScanner.Match_Scenario(token))
			{
				context.Builder.Pop(RuleType.Examples_Table);
				context.Builder.Pop(RuleType.Examples);
				context.Builder.Pop(RuleType.ScenarioOutline);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario);
				context.Builder.Build(token);
				return 13;
			}
			if (context.TokenScanner.Match_ScenarioOutline(token))
			{
				context.Builder.Pop(RuleType.Examples_Table);
				context.Builder.Pop(RuleType.Examples);
				context.Builder.Pop(RuleType.ScenarioOutline);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.ScenarioOutline);
				context.Builder.Build(token);
				return 20;
			}
			throw new Exception("parsing error at state 31: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:3>Examples:3>Examples_Table:0>#TableRow:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:2>ScenarioOutline_Step:0>Step:1>Step_Arg:0>__alt1:1>Multiline_And_Table_Arg:0>Multiline_Arg:0>#MultiLineArgument:0
		int MatchTokenAt_33(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Empty(token))
			{
				context.Builder.Build(token);
				return 33;
			}
			if (context.TokenScanner.Match_MultiLineArgument(token))
			{
				context.Builder.Build(token);
				return 34;
			}
			if (context.TokenScanner.Match_Other(token))
			{
				context.Builder.Build(token);
				return 33;
			}
			throw new Exception("parsing error at state 33: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:2>ScenarioOutline_Step:0>Step:1>Step_Arg:0>__alt1:1>Multiline_And_Table_Arg:0>Multiline_Arg:0>#MultiLineArgument:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:2>ScenarioOutline_Step:0>Step:1>Step_Arg:0>__alt1:1>Multiline_And_Table_Arg:0>Multiline_Arg:2>#MultiLineArgument:0
		int MatchTokenAt_34(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_TableRow(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Push(RuleType.Table_Arg);
				context.Builder.Build(token);
				return 35;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 23;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Examples);
				context.Builder.Build(token);
				return 27;
			}
			if (context.TokenScanner.Match_Examples(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Examples);
				context.Builder.Build(token);
				return 28;
			}
			throw new Exception("parsing error at state 34: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:2>ScenarioOutline_Step:0>Step:1>Step_Arg:0>__alt1:1>Multiline_And_Table_Arg:0>Multiline_Arg:2>#MultiLineArgument:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:2>ScenarioOutline_Step:0>Step:1>Step_Arg:0>__alt1:1>Multiline_And_Table_Arg:1>Table_Arg:0>#TableRow:0
		int MatchTokenAt_35(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_TableRow(token))
			{
				context.Builder.Build(token);
				return 35;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 23;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Examples);
				context.Builder.Build(token);
				return 27;
			}
			if (context.TokenScanner.Match_Examples(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Examples);
				context.Builder.Build(token);
				return 28;
			}
			throw new Exception("parsing error at state 35: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:1>ScenarioOutline:2>ScenarioOutline_Step:0>Step:1>Step_Arg:0>__alt1:1>Multiline_And_Table_Arg:1>Table_Arg:0>#TableRow:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:0>Scenario:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:1>Multiline_And_Table_Arg:0>Multiline_Arg:0>#MultiLineArgument:0
		int MatchTokenAt_36(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Empty(token))
			{
				context.Builder.Build(token);
				return 36;
			}
			if (context.TokenScanner.Match_MultiLineArgument(token))
			{
				context.Builder.Build(token);
				return 37;
			}
			if (context.TokenScanner.Match_Other(token))
			{
				context.Builder.Build(token);
				return 36;
			}
			throw new Exception("parsing error at state 36: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:0>Scenario:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:1>Multiline_And_Table_Arg:0>Multiline_Arg:0>#MultiLineArgument:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:0>Scenario:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:1>Multiline_And_Table_Arg:0>Multiline_Arg:2>#MultiLineArgument:0
		int MatchTokenAt_37(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOF(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 32;
			}
			if (context.TokenScanner.Match_TableRow(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Push(RuleType.Table_Arg);
				context.Builder.Build(token);
				return 38;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 16;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 12;
			}
			if (context.TokenScanner.Match_Scenario(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario);
				context.Builder.Build(token);
				return 13;
			}
			if (context.TokenScanner.Match_ScenarioOutline(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.ScenarioOutline);
				context.Builder.Build(token);
				return 20;
			}
			throw new Exception("parsing error at state 37: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:0>Scenario:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:1>Multiline_And_Table_Arg:0>Multiline_Arg:2>#MultiLineArgument:0");
		}
		
		
		// Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:0>Scenario:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:1>Multiline_And_Table_Arg:1>Table_Arg:0>#TableRow:0
		int MatchTokenAt_38(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOF(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 32;
			}
			if (context.TokenScanner.Match_TableRow(token))
			{
				context.Builder.Build(token);
				return 38;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 16;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 12;
			}
			if (context.TokenScanner.Match_Scenario(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario);
				context.Builder.Build(token);
				return 13;
			}
			if (context.TokenScanner.Match_ScenarioOutline(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Scenario);
				context.Builder.Pop(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.ScenarioOutline);
				context.Builder.Build(token);
				return 20;
			}
			throw new Exception("parsing error at state 38: Feature_File:2>Scenario_Base:1>Scenario_Base_Body:0>__alt0:0>Scenario:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:1>Multiline_And_Table_Arg:1>Table_Arg:0>#TableRow:0");
		}
		
		
		// Feature_File:1>Background:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:1>Multiline_And_Table_Arg:0>Multiline_Arg:0>#MultiLineArgument:0
		int MatchTokenAt_39(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_Empty(token))
			{
				context.Builder.Build(token);
				return 39;
			}
			if (context.TokenScanner.Match_MultiLineArgument(token))
			{
				context.Builder.Build(token);
				return 40;
			}
			if (context.TokenScanner.Match_Other(token))
			{
				context.Builder.Build(token);
				return 39;
			}
			throw new Exception("parsing error at state 39: Feature_File:1>Background:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:1>Multiline_And_Table_Arg:0>Multiline_Arg:0>#MultiLineArgument:0");
		}
		
		
		// Feature_File:1>Background:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:1>Multiline_And_Table_Arg:0>Multiline_Arg:2>#MultiLineArgument:0
		int MatchTokenAt_40(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOF(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Build(token);
				return 32;
			}
			if (context.TokenScanner.Match_TableRow(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Push(RuleType.Table_Arg);
				context.Builder.Build(token);
				return 41;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 8;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 12;
			}
			if (context.TokenScanner.Match_Scenario(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario);
				context.Builder.Build(token);
				return 13;
			}
			if (context.TokenScanner.Match_ScenarioOutline(token))
			{
				context.Builder.Pop(RuleType.Multiline_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.ScenarioOutline);
				context.Builder.Build(token);
				return 20;
			}
			throw new Exception("parsing error at state 40: Feature_File:1>Background:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:1>Multiline_And_Table_Arg:0>Multiline_Arg:2>#MultiLineArgument:0");
		}
		
		
		// Feature_File:1>Background:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:1>Multiline_And_Table_Arg:1>Table_Arg:0>#TableRow:0
		int MatchTokenAt_41(Token token, ParserContext context)
		{
			if (context.TokenScanner.Match_EOF(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Build(token);
				return 32;
			}
			if (context.TokenScanner.Match_TableRow(token))
			{
				context.Builder.Build(token);
				return 41;
			}
			if (context.TokenScanner.Match_Step(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Push(RuleType.Step);
				context.Builder.Build(token);
				return 8;
			}
			if (context.TokenScanner.Match_TagLine(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Build(token);
				return 12;
			}
			if (context.TokenScanner.Match_Scenario(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.Scenario);
				context.Builder.Build(token);
				return 13;
			}
			if (context.TokenScanner.Match_ScenarioOutline(token))
			{
				context.Builder.Pop(RuleType.Table_Arg);
				context.Builder.Pop(RuleType.Step);
				context.Builder.Pop(RuleType.Background);
				context.Builder.Push(RuleType.Scenario_Base);
				context.Builder.Push(RuleType.ScenarioOutline);
				context.Builder.Build(token);
				return 20;
			}
			throw new Exception("parsing error at state 41: Feature_File:1>Background:2>Scenario_Step:0>Step:1>Step_Arg:0>__alt1:1>Multiline_And_Table_Arg:1>Table_Arg:0>#TableRow:0");
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
					|| context.TokenScanner.Match_Examples(token)
				)
		        {
					match = true;
					break;
		        }
		    } while (false
				|| context.TokenScanner.Match_Empty(token)
				|| context.TokenScanner.Match_Comment(token)
				|| context.TokenScanner.Match_TagLine(token)
			);
			foreach(var t in queue)
				context.TokenQueue.Enqueue(t);
			return match;
		}
		
	}
}