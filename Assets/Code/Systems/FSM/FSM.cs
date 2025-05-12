using System;
using System.Collections.Generic;

namespace AnticTest.Systems.FSM
{
	public class FSM
	{
		private const int UNNASSIGNED_TRANSITION = -1;
		public int currentState = 0;
		private Dictionary<int, State> behaviours;
		private Dictionary<int, Func<object[]>> behaviourTickParameters;
		private Dictionary<int, Func<object[]>> behaviourOnEnterParameters;
		private Dictionary<int, Func<object[]>> behaviourOnExitParameters;
		private (int destinatinState, Action onTransition)[,] transitions;

		public FSM(int states, int flags)
		{
			behaviours = new Dictionary<int, State>();
			transitions = new (int, Action)[states, flags];

			for (int i = 0; i < states; i++)
			{
				for (int j = 0; j < flags; j++)
				{
					transitions[i, j] = (UNNASSIGNED_TRANSITION, null);
				}
			}

			behaviourTickParameters = new Dictionary<int, Func<object[]>>();
			behaviourOnEnterParameters = new Dictionary<int, Func<object[]>>();
			behaviourOnExitParameters = new Dictionary<int, Func<object[]>>();
		}

		public void AddBehaviour<TState>(int stateIndex, TState state, Func<object[]> onEnterParameters = null,
			Func<object[]> onTickParameters = null, Func<object[]> onExitParameters = null) where TState : State
		{
			if (!behaviours.ContainsKey(stateIndex))
			{
				state.FSMTrigger += Transition;
				behaviours.Add(stateIndex, state);
				behaviourOnEnterParameters.Add(stateIndex, onEnterParameters);
				behaviourTickParameters.Add(stateIndex, onTickParameters);
				behaviourOnExitParameters.Add(stateIndex, onExitParameters);
			}
		}

		public void ForceState(int state)
		{
			if (behaviours.ContainsKey(currentState))
				behaviours[currentState].ExitBehaviours(behaviourOnExitParameters[currentState]?.Invoke());

			currentState = state;

			behaviours[currentState].EnterBehaviours(behaviourOnEnterParameters[currentState]?.Invoke());
		}

		public void SetTransition(int originState, int flag, int destinationState, Action onTransition = null)
		{
			transitions[originState, flag] = (destinationState, onTransition);
		}

		public void Transition(int flag)
		{
			if (transitions[currentState, flag].destinatinState != UNNASSIGNED_TRANSITION)
			{
				behaviours[currentState].ExitBehaviours(behaviourOnExitParameters[currentState]?.Invoke());
				transitions[currentState, flag].onTransition?.Invoke();
				currentState = transitions[currentState, flag].destinatinState;
				behaviours[currentState].EnterBehaviours(behaviourOnEnterParameters[currentState]?.Invoke());
			}
		}

		public void Tick(float deltatime)
		{
			if (behaviours.ContainsKey(currentState))
				behaviours[currentState].TickBehaviours(deltatime, behaviourTickParameters[currentState]?.Invoke());
		}
	}
}