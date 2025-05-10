using AnticTest.Architecture.GameLogic;
using AnticTest.DataModel.Grid;
using AnticTest.Data.Blackboard;
using UnityEngine;
using AnticTest.Gameplay.Map;
using AnticTest.Gameplay.Entities.Factory;
using AnticTest.Data.Architecture;
using AnticTest.Gameplay.Components;
using UnityEngine;

namespace AnticTest.Gameplay.Game
{
	[RequireComponent(typeof(GameMap))]
	[RequireComponent(typeof(GameEntityFactory))]
	[RequireComponent(typeof(GameEntityRegistry))]
	public class Game : MonoBehaviour
	{
		private GameMap gameMap;
		private GameEntityFactory gameEntityFactory;
		private GameMap gameMap;

		private GameLogic gameLogic;
		private DataBlackboard dataBlackboard;

		[SerializeField] private MapArchitectureData levelToLoad;

		private void Awake()
		{
			dataBlackboard = new DataBlackboard();
			gameLogic = new GameLogic(levelToLoad);
			gameEntityFactory = GetComponent<GameEntityFactory>();
			gameEntityFactory.Init();
			gameMap = GetComponent<GameMap>();
			gameMap.Init();
			gameLogic.InitSimulation();
		}

		private void OnDisable()
		{
			gameEntityFactory.Dispose();
			gameMap.Dispose();
		}
	}
}
