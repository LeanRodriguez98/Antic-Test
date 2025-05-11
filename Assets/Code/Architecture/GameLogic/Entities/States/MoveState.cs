using AnticTest.Architecture.Pathfinding;
using AnticTest.Data.Events;
using AnticTest.DataModel.Grid;
using AnticTest.Systems.Events;
using AnticTest.Systems.FSM;
using AnticTest.Systems.Provider;
using System;
using System.Collections.Generic;

namespace AnticTest.Architecture.States
{
	public class MoveState<TCell, TCoordinate> : State
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		protected EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

		private List<TCell> currentPath;
		private int pathIndex;
		private float traveledDistanceBetweenCells;

		private IPathfinder<TCell, TCoordinate> pathfinder;
		private Func<Grid<TCell, TCoordinate>> graphPointer;
		private Func<float> logicalDistanceBetweenCells;

		public MoveState(Func<Grid<TCell, TCoordinate>> graphPointer,
						 IPathfinder<TCell, TCoordinate> pathfinder,
						 Func<float> logicalDistanceBetweenCells)
		{
			this.graphPointer = graphPointer;
			this.pathfinder = pathfinder;
			this.logicalDistanceBetweenCells = logicalDistanceBetweenCells;
		}

		public override List<Action> GetOnEnterBehaviours(params object[] parameters)
		{
			List<Action> enterBehabiours = new List<Action>();

			TCell origin = (TCell)parameters[0];
			Func<TCell> Destiny = (Func<TCell>)parameters[1];

			enterBehabiours.Add(() =>
			{
				currentPath = new List<TCell>(pathfinder.FindPath(origin, Destiny.Invoke(), graphPointer.Invoke().GetGraph()));
			});
			enterBehabiours.Add(() =>
			{
				pathIndex = 0;
				traveledDistanceBetweenCells = 0;
			});

			return enterBehabiours;
		}

		public override List<Action> GetTickBehaviours(float deltatime, params object[] parameters)
		{
			List<Action> tickBehabiours = new List<Action>();

			uint ID = (uint)parameters[0];
			int speed = (int)parameters[1];
			Action<TCoordinate> SetCoordinate = (Action<TCoordinate>)parameters[2];

			tickBehabiours.Add(() =>
			{
				if (currentPath != null)
				{
					if (currentPath.Count - 1 > pathIndex)
					{
						traveledDistanceBetweenCells += speed * deltatime;
						EventBus.Raise(new EntityMovedEvent(ID, currentPath[pathIndex].GetCoordinate(),
							currentPath[pathIndex + 1].GetCoordinate(), traveledDistanceBetweenCells));
						if (traveledDistanceBetweenCells >= logicalDistanceBetweenCells.Invoke())
						{
							traveledDistanceBetweenCells = 0.0f;
							pathIndex++;
							SetCoordinate.Invoke(currentPath[pathIndex].GetCoordinate());
						}
					}
				}
			});

			return tickBehabiours;
		}

		public override List<Action> GetOnExitBehaviours(params object[] parameters)
		{
			List<Action> exitBehabiours = new List<Action>();
			exitBehabiours.Add(() =>
			{
				pathIndex = 0;
				traveledDistanceBetweenCells = 0;
			});
			return exitBehabiours;
		}
	}
}