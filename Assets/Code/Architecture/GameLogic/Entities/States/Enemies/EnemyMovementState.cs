using AnticTest.Architecture.Events;
using AnticTest.Architecture.GameLogic;
using AnticTest.Architecture.Pathfinding;
using AnticTest.DataModel.Grid;
using AnticTest.Systems.Provider;
using System;
using System.Collections.Generic;

namespace AnticTest.Architecture.States
{
	class EnemyMovementState<TCell, TCoordinate> : MovementBaseState<TCell, TCoordinate>
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		private EntityRegistry<TCell, TCoordinate> EntityRegistry =>
			ServiceProvider.Instance.GetService<EntityRegistry<TCell, TCoordinate>>();

		public EnemyMovementState(Func<Grid<TCell, TCoordinate>> graphPointer,
								  IPathfinder<TCell, TCoordinate> pathfinder,
								  Func<float> logicalDistanceBetweenCells) :
								  base(graphPointer, pathfinder, logicalDistanceBetweenCells)
		{
		}

		public override void TickBehaviours(float deltatime, params object[] parameters)
		{
			base.TickBehaviours(deltatime, parameters.Add(EnemyFlags.OnAntReach));
		}

		protected override void ReachNewCell()
		{
			base.ReachNewCell();
			if (currentPath[pathIndex].GetCoordinate().Equals(EntityRegistry.Flag.GetCoordinate()))
				EventBus.Raise(new FlagCapturedEvent());
		}
	}
}
