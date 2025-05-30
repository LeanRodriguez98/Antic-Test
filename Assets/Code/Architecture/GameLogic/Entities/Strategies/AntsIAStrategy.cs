﻿using AnticTest.Architecture.Events;
using AnticTest.Architecture.States;
using AnticTest.Architecture.Utils;
using AnticTest.Data.Blackboard;
using AnticTest.Data.Events;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;
using AnticTest.Systems.Events;
using AnticTest.Systems.Provider;
using System.Collections.Generic;

namespace AnticTest.Architecture.GameLogic.Strategies
{
	public class AntsIAStrategy<TCell, TCoordinate> : IAntsStrategy
		where TCell : class, ICell<TCoordinate>, new()
		where TCoordinate : struct, ICoordinate
	{
		private DataBlackboard DataBlackboard => ServiceProvider.Instance.GetService<DataBlackboard>();
		private Map<TCell, TCoordinate> Map => ServiceProvider.Instance.GetService<Map<TCell, TCoordinate>>();
		private EntityRegistry<TCell, TCoordinate> EntityRegistry => ServiceProvider.Instance.GetService<EntityRegistry<TCell, TCoordinate>>();
		private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

		private TCoordinate FlagCoordinate => (TCoordinate)EntityRegistry.Flag.GetCoordinate();


		private Dictionary<uint, List<TCoordinate>> distancesToFlag;

		private List<uint> potentialThreats;
		private List<uint> patrolingAnts;

		private float enemyDistanceWeightMultiplier;
		private float enemyHealthWeightMultiplier;
		private float enemyDamageWeightMultiplier;
		private float enemySpeedWeightMultiplier;

		private uint rangeToDefend;

		public AntsIAStrategy()
		{
			enemyDistanceWeightMultiplier = DataBlackboard.AntsIAConfiguration.EnemyDistanceWeightMultiplier;
			enemyHealthWeightMultiplier = DataBlackboard.AntsIAConfiguration.EnemyHealthWeightMultiplier;
			enemyDamageWeightMultiplier = DataBlackboard.AntsIAConfiguration.EnemyDamageWeightMultiplier;
			enemySpeedWeightMultiplier = DataBlackboard.AntsIAConfiguration.EnemySpeedWeightMultiplier;

			rangeToDefend = DataBlackboard.AntsIAConfiguration.RangeToDefend;

			potentialThreats = new List<uint>();

			distancesToFlag = new Dictionary<uint, List<TCoordinate>>();
			for (uint i = 1; i <= rangeToDefend; i++)
			{
				distancesToFlag.Add(i, CoordinateUtils<TCoordinate>.GetCoordinatesInRing(FlagCoordinate, i));
			}

			patrolingAnts = new List<uint>();
		}

		public void Enable()
		{
			EventBus.Subscribe<EntityCreatedEvent>(OnEntityCreated);
			EventBus.Subscribe<EntityChangeCoordinateEvent>(OnEntityChangeCoordinate);
			EventBus.Subscribe<EntityDestroyEvent>(OnEntityDestroy);
			RecalculatePotentialThreats();
		}

		public void Update()
		{
			FindPatrolingAnts();
			AssignAntToThreats();
		}

		public void Disable()
		{
			EventBus.Unsubscribe<EntityCreatedEvent>(OnEntityCreated);
			EventBus.Unsubscribe<EntityChangeCoordinateEvent>(OnEntityChangeCoordinate);
			EventBus.Unsubscribe<EntityDestroyEvent>(OnEntityDestroy);
		}

		private void OnEntityCreated(EntityCreatedEvent _)
		{
			RecalculatePotentialThreats();
		}

		private void OnEntityChangeCoordinate(EntityChangeCoordinateEvent _)
		{
			RecalculatePotentialThreats();
		}

		private void OnEntityDestroy(EntityDestroyEvent _)
		{
			RecalculatePotentialThreats();
		}

		private void RecalculatePotentialThreats()
		{
			potentialThreats.Clear();
			foreach (KeyValuePair<uint, List<TCoordinate>> distanceToFlag in distancesToFlag)
			{
				List<(float cellThreat, List<uint> enemiesId)> enemiesInDestance = new List<(float, List<uint>)>();
				foreach (TCoordinate coordinate in distanceToFlag.Value)
				{
					List<uint> enemiesInCoordinate = Map.GetAllEntitiesIn<EnemyBug<TCell, TCoordinate>>(coordinate);
					float cellThreat = 0.0f; 
					foreach (uint enemyId in enemiesInCoordinate)
					{
						MobileEntity<TCell, TCoordinate> enemy = (EnemyBug<TCell, TCoordinate>)EntityRegistry[enemyId];
						cellThreat += distanceToFlag.Key * enemyDistanceWeightMultiplier;
						cellThreat += enemy.Health * enemyHealthWeightMultiplier;
						cellThreat += enemy.Damage * enemyDamageWeightMultiplier;
						cellThreat += enemy.Speed * enemySpeedWeightMultiplier;
					}
					enemiesInDestance.Add((cellThreat, enemiesInCoordinate));
				}
				enemiesInDestance.Sort((a, b) => a.cellThreat.CompareTo(b.cellThreat));
				foreach ((int cellThreat, List<uint> enemiesId) enemies in enemiesInDestance)
				{
					potentialThreats.AddRange(enemies.enemiesId);
				}
			}
		}

		private void FindPatrolingAnts()
		{
			patrolingAnts.Clear();
			foreach (Ant<TCell, TCoordinate> ant in EntityRegistry.Ants)
			{
				if (ant.GetFSMState() == (int)AntStates.Patrol)
				{
					patrolingAnts.Add(ant.GetID());
				}
			}
		}

		private void AssignAntToThreats()
		{
			while (patrolingAnts.Count > 0 && potentialThreats.Count > 0)
			{
				foreach (uint enemyId in potentialThreats)
				{
					if (patrolingAnts.Count == 0)
						return;
					AssingAnt(enemyId);
				}
			}
		}

		private void AssingAnt(uint enemyId)
		{
			Ant<TCell, TCoordinate> ant = (Ant<TCell, TCoordinate>)EntityRegistry[patrolingAnts[0]];
			ant.Destiny = (TCoordinate)EntityRegistry[enemyId].GetCoordinate();
			patrolingAnts.RemoveAt(0);
		}

		public List<uint> PotentialThreats => potentialThreats;
	}
}
