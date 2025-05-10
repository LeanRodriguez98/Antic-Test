using AnticTest.Systems.Provider;
using System;
using System.Collections.Generic;

namespace AnticTest.Systems.Events
{
	public class EventBus : IService
	{
		private readonly Dictionary<Type, List<Delegate>> subscribers = new Dictionary<Type, List<Delegate>>();

		public void Subscribe<TEvent>(Action<TEvent> callback) where TEvent : IEvent
		{
			Type eventType = typeof(TEvent);
			if (!subscribers.ContainsKey(eventType))
				subscribers[eventType] = new List<Delegate>();

			subscribers[eventType].Add(callback);
		}

		public void Unsubscribe<TEent>(Action<TEent> callback) where TEent : IEvent
		{
			Type eventType = typeof(TEent);
			if (subscribers.TryGetValue(eventType, out List<Delegate> list))
				list.Remove(callback);
		}

		public void Raise<Tevent>(Tevent raisingEvent) where Tevent : IEvent
		{
			Type eventType = typeof(Tevent);
			if (subscribers.TryGetValue(eventType, out List<Delegate> list))
			{
				foreach (Delegate callback in list)
					((Action<Tevent>)callback)?.Invoke(raisingEvent);
			}
		}

		public void ClearAll() => subscribers.Clear();
	}
}