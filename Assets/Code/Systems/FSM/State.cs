using System;
using System.Collections.Generic;

namespace AnticTest.Systems.FSM
{
	public abstract class State
	{
		public Action<int> OnFlag;
		public abstract List<Action> GetOnEnterBehaviours(params object[] parameters);
		public abstract List<Action> GetTickBehaviours(float deltaTime, params object[] parameters);
		public abstract List<Action> GetOnExitBehaviours(params object[] parameters);
	}
}
