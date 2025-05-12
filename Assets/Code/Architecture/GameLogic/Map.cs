using AnticTest.Architecture.Events;
using AnticTest.Data.Architecture;
using AnticTest.Data.Events;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;
using AnticTest.Systems.Events;
using AnticTest.Systems.Provider;
using System;
using System.Collections.Generic;

namespace AnticTest.Architecture.GameLogic
{
	public class Map<TCell, TCoordinate> : IService
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
		private EntityRegistry<TCell, TCoordinate> EntityRegistry => ServiceProvider.Instance.GetService<EntityRegistry<TCell, TCoordinate>>();

		private Grid<TCell, TCoordinate> grid;
		private float distanceBetweenCells;
		private Dictionary<TCoordinate, Dictionary<Type, List<uint>>> entitiesInCoordinate;

		public (int x, int y) Size => grid.GetSize();
		public float DistanceBetweenCells => distanceBetweenCells;
		public Grid<TCell, TCoordinate> Grid => grid;

		public Map(MapArchitectureData levelData)
		{
			grid = new Grid<TCell, TCoordinate>(levelData.Map.width, levelData.Map.height);
			distanceBetweenCells = levelData.LogicalDistanceBetweenCells;

			CreateTerrain(levelData);

			entitiesInCoordinate = new Dictionary<TCoordinate, Dictionary<Type, List<uint>>>();
			foreach (TCell cell in grid.GetGraph())
			{
				entitiesInCoordinate.Add(cell.GetCoordinate(), new Dictionary<Type, List<uint>>());
			}

			EventBus.Subscribe<CellSelectedEvent<TCoordinate>>(OnCellSelected);
			EventBus.Subscribe<CellDeselectedEvent>(OnCellDeselected);
			EventBus.Subscribe<EntityCreatedEvent>(RegisterEntity);
			EventBus.Subscribe<EntityDestroyEvent>(UnregisterEntity);
			EventBus.Subscribe<EntityChangeCoordinateEvent>(Move);
		}

		public TCell SelectedCell => (TCell)grid.SelectedCell;

		private void OnCellSelected(CellSelectedEvent<TCoordinate> cellSelectedEvent)
		{
			grid.SetSelectedCell(cellSelectedEvent.selectedCoordinate);
		}

		private void OnCellDeselected(CellDeselectedEvent cellDeselectedEvent)
		{
			grid.SetSelectedCell(null);
		}

		public void CreateTerrain(MapArchitectureData levelData)
		{
			for (int y = 0; y < levelData.Map.height; y++)
			{
				for (int x = 0; x < levelData.Map.width; x++)
				{
					TCell newCell = new TCell();
					TCoordinate newCellCordinate = new TCoordinate();
					newCellCordinate.Set(x, y);
					newCell.Init(newCellCordinate,
						levelData.Map[x, y].cellArchitectureData.cellType,
						levelData.Map[x, y].cellArchitectureData.cellHeight);
					grid.SetCell(newCell);
				}
			}
		}

		public TCell this[TCoordinate coordinate] => GetCell(coordinate);

		public TCell GetCell(TCoordinate coordinate)
		{
			return grid.GetCell(coordinate);
		}

		public TCell this[ICoordinate coordinate] => GetCell(coordinate);

		public TCell GetCell(ICoordinate coordinate)
		{
			if (coordinate is TCoordinate)
				return grid.GetCell((TCoordinate)coordinate);
			throw new InvalidCastException();
		}

		public bool IsInBorders(TCoordinate coordinate)
		{
			return grid.IsValid(coordinate);
		}

		private void RegisterEntity(EntityCreatedEvent entityCreatedEvent)
		{
			RegisterEntity((TCoordinate)entityCreatedEvent.entity.GetCoordinate(),
				entityCreatedEvent.entity);
		}

		private void RegisterEntity(TCoordinate coordinate, IEntity entity)
		{
			if (!entitiesInCoordinate[coordinate].ContainsKey(entity.GetType()))
				entitiesInCoordinate[coordinate].Add(entity.GetType(), new List<uint>());
			entitiesInCoordinate[coordinate][entity.GetType()].Add(entity.GetID());
		}

		private void UnregisterEntity(EntityDestroyEvent entityDestroyEvent)
		{
			UnregisterEntity((TCoordinate)entityDestroyEvent.entity.GetCoordinate(),
				entityDestroyEvent.entity);
		}

		private void UnregisterEntity(TCoordinate coordinate, IEntity entity)
		{
			entitiesInCoordinate[coordinate][entity.GetType()].Remove(entity.GetID());
		}

		private void Move(EntityChangeCoordinateEvent entityChangeCoordinateEvent)
		{
			Move((TCoordinate)entityChangeCoordinateEvent.oldCoordinate,
				 (TCoordinate)entityChangeCoordinateEvent.newCoordinate,
				 EntityRegistry[entityChangeCoordinateEvent.entityId]);
		}

		private void Move(TCoordinate oldCoordinate, TCoordinate newCoordinate, IEntity entity)
		{
			UnregisterEntity(oldCoordinate, entity);
			RegisterEntity(newCoordinate, entity);
		}

		public Dictionary<Type, List<uint>> GetAllEntitiesIn(TCoordinate coordinate)
		{
			return entitiesInCoordinate[coordinate];
		}

		public List<uint> GetAllEntitiesIn<TEntity>(TCoordinate coordinate) where TEntity : IEntity
		{
			return GetAllEntitiesIn(typeof(TEntity), coordinate);
		}

		public List<uint> GetAllEntitiesIn(Type entityType, TCoordinate coordinate)
		{
			if (!entityType.InheritsFromRawGeneric(typeof(Entity<,>)))
				throw new InvalidCastException();
			if (entitiesInCoordinate[coordinate].ContainsKey(entityType))
				return entitiesInCoordinate[coordinate][entityType];
			return new List<uint>();
		}
	}
}