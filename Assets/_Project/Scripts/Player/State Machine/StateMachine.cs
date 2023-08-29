using System;
using System.Collections.Generic;

namespace ThePatient
{
    public class StateMachine
    {
        StateNode currentState;
        Dictionary<Type, StateNode> stateNodes = new Dictionary<Type, StateNode>();
        HashSet<ITransition> anyTransitions = new HashSet<ITransition>();

        public void Update()
        {
            var transition = GetTransition();
            if(transition != null)
                ChangeState(transition.TargetState);
            
            currentState.State?.Update();
        }

        public void FixedUpdate()
        {
            currentState.State?.FixedUpdate();
        }
        
        public void SetState(IState state)
        {
            currentState = stateNodes[state.GetType()];
            currentState.State?.OnEnter();
        }

        void ChangeState(IState newState)
        {
            if (newState == currentState.State) return;

            var previousState = currentState.State;
            var nextState = stateNodes[newState.GetType()].State;

            previousState?.OnExit();
            nextState?.OnEnter();
            currentState = stateNodes[newState.GetType()];
        }

        ITransition GetTransition()
        {
            foreach (var transition in anyTransitions)
            {
                if (transition.Predicate.Evaluate())
                    return transition;
            }

            foreach (var transition in currentState.Transitions)
            {
                if (transition.Predicate.Evaluate())
                    return transition;
            }

            return null;
        }

        public void AddTransition(IState fromState, IState toState, IPredicate predicate)
        {
            GetOrAddNode(fromState).AddTransition(GetOrAddNode(toState).State, predicate);
        }

        public void AddAnyTransition(IState toState, IPredicate predicate)
        {
            anyTransitions.Add(new Transition(GetOrAddNode(toState).State, predicate));
        }

        StateNode GetOrAddNode(IState state)
        {
            var node = stateNodes.GetValueOrDefault(state.GetType());

            if(node == null)
            {
                node = new StateNode(state);
                stateNodes.Add(state.GetType(), node);
            }

            return node;
        }

        class StateNode
        {
            public IState State { get; }
            public HashSet<ITransition> Transitions { get; }

            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<ITransition>();
            }

            public void AddTransition(IState toState, IPredicate predicate)
            {
                Transitions.Add(new Transition(toState, predicate));
            }
        }
    }
}
