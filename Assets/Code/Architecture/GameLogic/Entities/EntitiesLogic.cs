using AnticTest.Architecture.Pathfinding;
using AnticTest.Architecture.States;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;
using AnticTest.Systems.Provider;
using System;

namespace AnticTest.Architecture.GameLogic
{
	public class EntitiesLogic<TCell, TCoordinate> : IService
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		private EntityRegistry<TCell, TCoordinate> EntityRegistry => ServiceProvider.Instance.GetService<EntityRegistry<TCell, TCoordinate>>();
		private Map<TCell, TCoordinate> Map => ServiceProvider.Instance.GetService<Map<TCell, TCoordinate>>();

		public EntitiesLogic() { }

		public void Init()
		{
			foreach (EnemyBug<TCell, TCoordinate> enemyBug in EntityRegistry.Enemies)
			{
				enemyBug.InitFSM(1, 1);
				enemyBug.Destiny = (TCoordinate)EntityRegistry.Flag.GetCoordinate();
				AddMoveState(enemyBug);
				enemyBug.StartFSM(0);
			}
		}

		public void Update(float deltaTime)
		{
			foreach (IEntity entity in EntityRegistry.Entities)
			{
				entity.Update(deltaTime);
			}
		}

		private void AddMoveState(MobileEntity<TCell, TCoordinate> mobileEntity)
		{
			Action<TCoordinate> SetCoordinate = (coordinate) => { mobileEntity.SetCoordinate(coordinate); };
			Func<TCell> GetDestiny = () => Map.Grid[mobileEntity.Destiny];

			mobileEntity.AddState
				(
				stateIndex: 0,
				state: new MoveState<TCell, TCoordinate>(() => Map.Grid,
												  new EntityPathfinding<TCell, TCoordinate>(mobileEntity),
												  () => Map.DistanceBetweenCells),
				onEnterParameters: () => new object[]
					{
							Map.Grid[(TCoordinate)mobileEntity.GetCoordinate()],
							GetDestiny
					},
				onTickParameters: () => new object[]
					{
							mobileEntity.GetID(),
							mobileEntity.Speed,
							SetCoordinate
					}
				);
		}
	}
}
