using AnticTest.Architecture.Pathfinding;
using AnticTest.DataModel.Grid;
using System;
using System.Collections.Generic;

namespace AnticTest.Architecture.States
{
	class AntMovementState<TCell, TCoordinate> : MovementBaseState<TCell, TCoordinate>
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{

		public AntMovementState(Func<Grid<TCell, TCoordinate>> graphPointer,
								IPathfinder<TCell, TCoordinate> pathfinder,
								Func<float> logicalDistanceBetweenCells) :
								base(graphPointer, pathfinder, logicalDistanceBetweenCells)
		{
		}

		public override void TickBehaviours(float deltatime, params object[] parameters)
		{
			base.TickBehaviours(deltatime, parameters.Add(AntFlags.OnEnemyApproach));

			if (pathIndex >= currentPath.Count)
				FSMTrigger((int)AntFlags.OnDestinationReach);
		}
	}
}
