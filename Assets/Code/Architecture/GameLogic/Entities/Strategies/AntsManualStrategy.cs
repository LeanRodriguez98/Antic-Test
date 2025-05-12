using AnticTest.Architecture.Events;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;
using AnticTest.Systems.Events;
using AnticTest.Systems.Provider;
using System.Collections.Generic;

namespace AnticTest.Architecture.GameLogic.Strategies
{
	public class AntsManualStrategy<TCell, TCoordinate> : IAntsStrategy
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		private Map<TCell, TCoordinate> Map => ServiceProvider.Instance.GetService<Map<TCell, TCoordinate>>();
		private EntityRegistry<TCell, TCoordinate> EntityRegistry => ServiceProvider.Instance.GetService<EntityRegistry<TCell, TCoordinate>>();
		private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

		private List<uint> selectedAntsId;

		public AntsManualStrategy()
		{
			selectedAntsId = new List<uint>();
		}

		public void Enable()
		{
			EventBus.Subscribe<EntityChangeCoordinateEvent>(SelectAnts);
			EventBus.Subscribe<CellSelectedEvent<TCoordinate>>(SetAntsDestination);
			EventBus.Subscribe<CellSelectedEvent<TCoordinate>>(SelectAnts);
			EventBus.Subscribe<CellDeselectedEvent>(DeselectAnts);
			if (Map.HasCellSelected)
				SelectEntities(Map.SelectedCell.GetCoordinate());
		}

		public void Disable()
		{
			EventBus.Unsubscribe<EntityChangeCoordinateEvent>(SelectAnts);
			EventBus.Unsubscribe<CellSelectedEvent<TCoordinate>>(SetAntsDestination);
			EventBus.Unsubscribe<CellSelectedEvent<TCoordinate>>(SelectAnts);
			EventBus.Unsubscribe<CellDeselectedEvent>(DeselectAnts);
			ClearSelection();
		}

		private void SetAntsDestination(CellSelectedEvent<TCoordinate> cellSelectedEvent)
		{
			foreach (uint id in selectedAntsId)
			{
				(EntityRegistry[id] as Ant<TCell, TCoordinate>).Destiny = cellSelectedEvent.selectedCoordinate;
			}
		}

		private void SelectAnts(EntityChangeCoordinateEvent entityChangeCoordinateEvent)
		{
			if (Map.HasCellSelected)
			{
				if (entityChangeCoordinateEvent.oldCoordinate.Equals(Map.SelectedCell.GetCoordinate()) ||
					entityChangeCoordinateEvent.newCoordinate.Equals(Map.SelectedCell.GetCoordinate()))
					SelectEntities(Map.SelectedCell.GetCoordinate());
			}
		}

		private void SelectAnts(CellSelectedEvent<TCoordinate> cellSelectedEvent)
		{
			SelectEntities(cellSelectedEvent.selectedCoordinate);
		}

		private void DeselectAnts(CellDeselectedEvent cellDeselectedEvent)
		{
			ClearSelection();
		}

		private void SelectEntities(TCoordinate selection)
		{
			selectedAntsId = Map.GetAllEntitiesIn<Ant<TCell, TCoordinate>>(selection);
		}

		private void ClearSelection()
		{
			selectedAntsId.Clear();
		}
	}
}
