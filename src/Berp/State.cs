using System;
using System.Collections.Generic;
using System.Linq;

namespace Berp
{
    public class StateTransition
    {
        public TokenType TokenType { get; private set; }
        public int TargetState { get; private set; }
        public List<ProductionRule> Productions { get; private set; }
        public LookAheadHint LookAheadHint { get; set; }

        public StateTransition(TokenType tokenType, int targetState, List<ProductionRule> productions, LookAheadHint lookAheadHint)
        {
            Productions = productions;
            LookAheadHint = lookAheadHint;
            TokenType = tokenType;
            TargetState = targetState;
        }
    }
    public class State
    {
        private readonly int id;
        private readonly List<Branch> branches;
        private readonly List<StateTransition> transitions = new List<StateTransition>();

        public int Id
        {
            get { return id; }
        }

        public bool IsEndState { get { return !transitions.Any(); } }

        public string Comment { get; set; }

        public IEnumerable<StateTransition> Transitions
        {
            get { return
                transitions.Where(s => s.TokenType.Equals(TokenType.EOF))
                .Concat(transitions.Where(s => !s.TokenType.Equals(TokenType.Other) && !s.TokenType.Equals(TokenType.EOF)))
                .Concat(transitions.Where(s => s.TokenType.Equals(TokenType.Other)));
            }
        }

        public void AddTransition(StateTransition transition)
        {
            transitions.Add(transition);
        }

        internal List<Branch> Branches
        {
            get { return branches; }
        }

        internal State(int id, List<Branch> branches)
        {
            this.id = id;
            this.branches = branches;
        }

        internal bool Equals(List<Branch> otherBranches)
        {
            return otherBranches.Count == branches.Count && !otherBranches.Except(branches, Branch.ProductionComparer).Any();
        }
    }
}