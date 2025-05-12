using AnticTest.Architecture.Events;
using AnticTest.Architecture.Pathfinding;
using AnticTest.DataModel.Grid;
using AnticTest.Systems.Events;
using AnticTest.Systems.FSM;
using System;
using System.Collections.Generic;

namespace AnticTest.Architecture.States
{
	public class MoveState<TCell, TCoordinate> : State
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
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

		public override void EnterBehaviours(params object[] parameters)
		{
			TCell origin = (TCell)parameters[0];
			Func<TCell> Destiny = (Func<TCell>)parameters[1];
			currentPath = new List<TCell>(pathfinder.FindPath(origin, Destiny.Invoke(), graphPointer.Invoke().GetGraph()));
		}

		public override void TickBehaviours(float deltatime, params object[] parameters)
		{
			uint ID = (uint)parameters[0];
			int speed = (int)parameters[1];
			Action<TCoordinate> SetCoordinate = (Action<TCoordinate>)parameters[2];
			Func<TCoordinate> GetCoordinate = (Func<TCoordinate>)parameters[3];
			Func<bool> HasOponents = (Func<bool>)parameters[4];

			if (currentPath != null)
			{
				if (currentPath.Count - 1 > pathIndex)
				{
					traveledDistanceBetweenCells += speed * deltatime;
					EventBus.Raise(new EntityMovedEvent(ID, currentPath[pathIndex].GetCoordinate(),
						currentPath[pathIndex + 1].GetCoordinate(), traveledDistanceBetweenCells));

					if (!GetCoordinate.Invoke().Equals(currentPath[pathIndex + 1].GetCoordinate()) &&
						traveledDistanceBetweenCells >= logicalDistanceBetweenCells.Invoke() / 2.0f)
					{
						SetCoordinate.Invoke(currentPath[pathIndex + 1].GetCoordinate());
						EventBus.Raise(new EntityChangeCoordinateEvent(ID,
							currentPath[pathIndex].GetCoordinate(),
							currentPath[pathIndex + 1].GetCoordinate()));

						if (HasOponents.Invoke())
							FSMTrigger((int)EntityFlags.OnOponentReach);
					}

					if (traveledDistanceBetweenCells >= logicalDistanceBetweenCells.Invoke())
					{
						traveledDistanceBetweenCells = 0.0f;
						pathIndex++;
					}
				}
			}
		}

		public override void ExitBehaviours(params object[] parameters)
		{
			pathIndex = 0;
		}
	}
}