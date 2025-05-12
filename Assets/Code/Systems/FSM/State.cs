using AnticTest.Systems.Events;
using AnticTest.Systems.Provider;
using System;

namespace AnticTest.Systems.FSM
{
	public abstract class State
	{
		protected EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

		public Action<int> FSMTrigger;
		public abstract void EnterBehaviours(params object[] parameters);
		public abstract void TickBehaviours(float deltaTime, params object[] parameters);
		public abstract void ExitBehaviours(params object[] parameters);
	}
}
