using AnticTest.Architecture.GameLogic;
using AnticTest.Data.Blackboard;
using AnticTest.DataModel.Grid;
using AnticTest.Systems.Events;
using AnticTest.Systems.Provider;
using System;
using UnityEngine;

namespace AnticTest.Gameplay.Components
{
	public abstract class GameComponent : MonoBehaviour , IDisposable
	{
		protected DataBlackboard DataBlackboard => ServiceProvider.Instance.GetService<DataBlackboard>();
		protected EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
		protected GameMap GameMap => ServiceProvider.Instance.GetService<GameMap>();
		protected Map<Cell<Coordinate>, Coordinate> LogicalMap => ServiceProvider.Instance.GetService<Map<Cell<Coordinate>, Coordinate>>();

		public abstract void Init();
		public abstract void Dispose();
	}
}
