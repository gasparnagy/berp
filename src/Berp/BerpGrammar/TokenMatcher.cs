using System;
using System.Linq;

namespace Berp.BerpGrammar
{
    public class TokenMatcher
    {
        public bool Match_AlternateOp(Token token)
        {
            return token.TokenType == TokenType.AlternateOp;
        }

        public bool Match_Token(Token token)
        {
            return token.TokenType == TokenType.Token;
        }

        public bool Match_Rule(Token token)
        {
            return token.TokenType == TokenType.Rule;
        }

        public bool Match_Definition(Token token)
        {
            return token.TokenType == TokenType.Definition;
        }

        public bool Match_LParen(Token token)
        {
            return token.TokenType == TokenType.LParen;
        }

        public bool Match_RParen(Token token)
        {
            return token.TokenType == TokenType.RParen;
        }

        public bool Match_AnyMultiplier(Token token)
        {
            return token.TokenType == TokenType.AnyMultiplier;
        }

        public bool Match_OneOrMoreMultiplier(Token token)
        {
            return token.TokenType == TokenType.OneOrMoreMultiplier;
        }

        public bool Match_OneOrZeroMultiplier(Token token)
        {
            return token.TokenType == TokenType.OneOrZeroMultiplier;
        }

        public bool Match_EOL(Token token)
        {
            return token.TokenType == TokenType.EOL;
        }

        public bool Match_Production(Token token)
        {
            return token.TokenType == TokenType.Production;
        }

        public bool Match_Arrow(Token token)
        {
            return token.TokenType == TokenType.Arrow;
        }

        public bool Match_Comma(Token token)
        {
            return token.TokenType == TokenType.Comma;
        }

        public bool Match_LBracket(Token token)
        {
            return token.TokenType == TokenType.LBracket;
        }

        public bool Match_RBracket(Token token)
        {
            return token.TokenType == TokenType.RBracket;
        }

        public bool Match_EOF(Token token)
        {
            return token.IsEOF;
        }

    }
}