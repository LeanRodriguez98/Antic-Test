using AnticTest.Data.Events;
using AnticTest.DataModel.Grid;
using AnticTest.DataModel.Pathfinding;
using System;
using System.Collections.Generic;

namespace AnticTest.DataModel.Entities
{
	public abstract class MobileEntity<TCell, TCoordinate> : Entity<TCell, TCoordinate>
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		protected int speed;
		protected int health;
		protected IPathfinder<TCell, TCoordinate> pathfinder;
		protected Func<Grid<TCell, TCoordinate>> graphPointer;

		public MobileEntity(TCoordinate coordinate, uint ID) : base(coordinate, ID) { }

		public override void SetCoordinate(TCoordinate coordinate)
		{
			base.SetCoordinate(coordinate);
			EventBus.Raise(new EntityMovedEvent(ID, coordinate));
		}

		protected override void SetParameters(params object[] parameters)
		{
			speed = (int)parameters[0];
			health = (int)parameters[1];
		}

		public void SetPathfinderSystem(Func<Grid<TCell, TCoordinate>> graphPointer, IPathfinder<TCell, TCoordinate> pathfinder)
		{
			this.graphPointer = graphPointer;
			this.pathfinder = pathfinder;
		}

		public void GoTo(TCell cell)
		{
			IEnumerable<TCell> path = pathfinder.FindPath(graphPointer.Invoke()[coordinate], cell, graphPointer.Invoke().GetGraph());
			foreach (TCell step in path)
			{
				SetCoordinate(step.GetCoordinate());
			}
		}
	}
}
