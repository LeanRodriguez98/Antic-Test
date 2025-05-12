using AnticTest.Architecture.Events;
using AnticTest.Architecture.Pathfinding;
using AnticTest.Architecture.States;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;
using AnticTest.Systems.Events;
using AnticTest.Systems.Provider;
using System;
using System.Collections.Generic;

namespace AnticTest.Architecture.GameLogic
{
	public class EntitiesLogic<TCell, TCoordinate> : IService
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		private EntityRegistry<TCell, TCoordinate> EntityRegistry => ServiceProvider.Instance.GetService<EntityRegistry<TCell, TCoordinate>>();
		private Map<TCell, TCoordinate> Map => ServiceProvider.Instance.GetService<Map<TCell, TCoordinate>>();
		private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

		public EntitiesLogic() { }

		public void Init()
		{
			EventBus.Subscribe<EntityChangeCoordinateEvent>(UpdateCurrentOponents);

			foreach (EnemyBug<TCell, TCoordinate> enemyBug in EntityRegistry.Enemies)
			{
				enemyBug.InitFSM(Enum.GetValues(typeof(EntityStates)).Length, Enum.GetValues(typeof(EntityFlags)).Length);
				enemyBug.Destiny = (TCoordinate)EntityRegistry.Flag.GetCoordinate();
				AddMoveState(enemyBug);
				AddFightState(enemyBug);
				RegisterEnemyTransitions(enemyBug);
				enemyBug.StartFSM((int)EntityStates.Movement);
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
			Func<TCoordinate> GetCoordinate = () => (TCoordinate)mobileEntity.GetCoordinate();
			Func<bool> HasOponents = () => mobileEntity.HasOponents;

			mobileEntity.AddState
				(
				stateIndex: (int)EntityStates.Movement,
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
							SetCoordinate,
							GetCoordinate,
							HasOponents
					}
				);
		}

		private void AddFightState(MobileEntity<TCell, TCoordinate> mobileEntity) 
		{
			mobileEntity.AddState
				(
				stateIndex: (int)EntityStates.Fight,
				state: new FightState(),
				onTickParameters: () => new object[] 
					{
						mobileEntity.CurrentOponents,
						mobileEntity.Damage,
						mobileEntity.TimeBetweenAtacks
					}
				);
		}

		private void RegisterEnemyTransitions(EnemyBug<TCell, TCoordinate> enemyBug)
		{
			enemyBug.SetFSMTransition((int)EntityStates.Movement, 
									  (int)EntityFlags.OnOponentReach, 
									  (int)EntityStates.Fight, () =>
									  {
										  
									  });

			enemyBug.SetFSMTransition((int)EntityStates.Fight, 
									  (int)EntityFlags.OnEnemyDead, 
									  (int)EntityStates.Movement, () => 
									  {

									  });
		}


		private void UpdateCurrentOponents(EntityChangeCoordinateEvent entityChangeCoordinateEvent)
		{
			UpdateOponentsIn((TCoordinate)entityChangeCoordinateEvent.oldCoordinate);
			UpdateOponentsIn((TCoordinate)entityChangeCoordinateEvent.newCoordinate);
		}

		private void UpdateOponentsIn(TCoordinate coordinate) 
		{
			foreach (KeyValuePair<Type, List<uint>> allEntiiesInCoordinate in Map.GetAllEntitiesIn(coordinate))
			{
				foreach (uint entityID in allEntiiesInCoordinate.Value)
				{
					SetCurrentOponentsOf(EntityRegistry[entityID]);
				}
			}
		}

		private void SetCurrentOponentsOf(IEntity entity) 
		{
			if (!(entity is ICombatant))
				return;
			
			ICombatant combatant = (ICombatant)entity;
			combatant.ClearCurrentOponents();

			foreach (KeyValuePair<Type, List<uint>> allEntiiesInCoordinate in Map.GetAllEntitiesIn((TCoordinate)entity.GetCoordinate()))
			{
				foreach (uint otherEntityID in allEntiiesInCoordinate.Value)
				{
					if (!(EntityRegistry[otherEntityID] is ICombatant))
						continue;

					ICombatant otherCombatant = (ICombatant)EntityRegistry[otherEntityID];
					if (AreOponents(combatant, otherCombatant))
						combatant.AddOponent(otherCombatant);
				}
			}
		}

		private bool AreOponents(ICombatant combatantA, ICombatant combatantB) 
		{
			return (combatantA is Ant<TCell, TCoordinate> && combatantB is EnemyBug<TCell, TCoordinate>) ||
				   (combatantB is Ant<TCell, TCoordinate> && combatantA is EnemyBug<TCell, TCoordinate>);
		}
	}
}
