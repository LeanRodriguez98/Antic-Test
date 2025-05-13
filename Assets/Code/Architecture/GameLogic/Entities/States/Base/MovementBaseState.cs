using AnticTest.Architecture.Events;
using AnticTest.Architecture.Pathfinding;
using AnticTest.DataModel.Grid;
using AnticTest.Systems.FSM;
using System;
using System.Collections.Generic;

namespace AnticTest.Architecture.States
{
	public abstract class MovementBaseState<TCell, TCoordinate> : State
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		protected List<TCell> currentPath;
		protected int pathIndex;
		protected float traveledDistanceBetweenCells;

		protected IPathfinder<TCell, TCoordinate> pathfinder;
		protected Func<Grid<TCell, TCoordinate>> graphPointer;
		protected Func<float> logicalDistanceBetweenCells;
		protected bool hasUnfinishedTravelToCellCenter;
		protected TCell unfinishedTravelOrigin;

		public MovementBaseState(Func<Grid<TCell, TCoordinate>> graphPointer,
						 IPathfinder<TCell, TCoordinate> pathfinder,
						 Func<float> logicalDistanceBetweenCells)
		{
			this.graphPointer = graphPointer;
			this.pathfinder = pathfinder;
			this.logicalDistanceBetweenCells = logicalDistanceBetweenCells;
			hasUnfinishedTravelToCellCenter = false;
			unfinishedTravelOrigin = null;
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

			int enemyTransitionFlag = (int)parameters[5];

			if (hasUnfinishedTravelToCellCenter)
			{
				Travel(unfinishedTravelOrigin, currentPath[pathIndex]);
			}
			else
			{
				if (currentPath != null)
				{
					if (currentPath.Count - 1 > pathIndex)
						Travel(currentPath[pathIndex], currentPath[pathIndex + 1]);
				}
			}

			void Travel(TCell origin, TCell destination)
			{
				Move(speed, deltatime, ID, origin, destination);

				if (ShouldChangeCell(GetCoordinate.Invoke(), destination.GetCoordinate()))
					ChangeCell(origin.GetCoordinate(), destination.GetCoordinate(), ID, SetCoordinate);

				CheckForEnemies(HasOponents, enemyTransitionFlag);

				if (FinishedStep())
					ReachNewCell();
			}
		}

		public override void ExitBehaviours(params object[] parameters)
		{
			pathIndex = 0;
		}

		protected void Move(float speed, float deltatime, uint ID, TCell origin, TCell destination)
		{
			traveledDistanceBetweenCells += speed * deltatime;
			EventBus.Raise(new EntityMovedEvent(ID, origin.GetCoordinate(),
				destination.GetCoordinate(), traveledDistanceBetweenCells));
		}

		protected bool FinishedStep()
		{
			return traveledDistanceBetweenCells >= logicalDistanceBetweenCells.Invoke();
		}

		protected void ReachNewCell()
		{
			traveledDistanceBetweenCells = 0.0f;

			if (hasUnfinishedTravelToCellCenter)
				hasUnfinishedTravelToCellCenter = false;
			else
				pathIndex++;
		}

		protected bool ShouldChangeCell(TCoordinate currentCoordinate, TCoordinate reachCoordinate)
		{
			return !currentCoordinate.Equals(reachCoordinate) &&
					traveledDistanceBetweenCells >= logicalDistanceBetweenCells.Invoke() / 2.0f;
		}

		protected void ChangeCell(TCoordinate origin, TCoordinate destination, uint ID, Action<TCoordinate> SetCoordinate)
		{
			unfinishedTravelOrigin = currentPath[pathIndex];
			SetCoordinate.Invoke(destination);
			EventBus.Raise(new EntityChangeCoordinateEvent(ID, origin, destination));
		}

		protected void CheckForEnemies(Func<bool> HasOponents, int enemyTransition)
		{
			if (HasOponents.Invoke())
			{
				hasUnfinishedTravelToCellCenter = true;
				FSMTrigger(enemyTransition);
			}
		}
	}
}