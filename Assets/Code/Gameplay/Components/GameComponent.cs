using AnticTest.Data.Blackboard;
using AnticTest.Systems.Events;
using AnticTest.Systems.Provider;
using UnityEngine;

namespace AnticTest.Gameplay.Components
{
	public abstract class GameComponent : MonoBehaviour
	{
		protected DataBlackboard DataBlackboard => ServiceProvider.Instance.GetService<DataBlackboard>();
		protected EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

		public abstract void Init();
		public abstract void Dispose();
	}
}
