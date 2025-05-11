using AnticTest.Data.Events;
using AnticTest.DataModel.Grid;
using AnticTest.DataModel.Pathfinding;
using System;
using System.Collections.Generic;

namespace AnticTest.DataModel.Entities
{
	public abstract class MobileEntity<TCell, TCoordinate> : Entity<TCell, TCoordinate>, ITransitabilityProvider
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		protected int speed;
		protected int health;

		protected List<TCell> currentPath;
		protected int pathIndex;
		protected float traveledDistanceBetweenCells;
		protected Transitability transitability;

		protected IPathfinder<TCell, TCoordinate> pathfinder;
		protected Func<Grid<TCell, TCoordinate>> graphPointer;
		protected Func<float> logicalDistanceBetweenCells;

		public MobileEntity(TCoordinate coordinate, uint ID) : base(coordinate, ID) { }

		public override void SetCoordinate(TCoordinate coordinate)
		{
			base.SetCoordinate(coordinate);
		}

		protected override void SetParameters(params object[] parameters)
		{
			speed = (int)parameters[0];
			health = (int)parameters[1];
			transitability = (Transitability)parameters[2];
		}

		public void SetLogicalDistanceBetweenCells(Func<float> logicalDistanceBetweenCells)
		{
			this.logicalDistanceBetweenCells = logicalDistanceBetweenCells;
		}

		public void SetPathfinderSystem(Func<Grid<TCell, TCoordinate>> graphPointer, IPathfinder<TCell, TCoordinate> pathfinder)
		{
			this.graphPointer = graphPointer;
			this.pathfinder = pathfinder;
		}

		public void GoTo(TCell cell)
		{
			currentPath = new List<TCell>(pathfinder.FindPath(graphPointer.Invoke()[coordinate], cell, graphPointer.Invoke().GetGraph()));
			pathIndex = 0;
			traveledDistanceBetweenCells = 0;
		}

		public override void Update(float deltatime)
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
						SetCoordinate(currentPath[pathIndex].GetCoordinate());
					}
				}
			}
		}

		public Transitability GetTransitability()
		{
			return transitability;
		}
	}
}
