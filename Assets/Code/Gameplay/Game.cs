using AnticTest.Architecture.GameLogic;
using AnticTest.DataModel.Map;
using AnticTest.Data.Blackboard;
using UnityEngine;
using AnticTest.Gameplay.Map;
using AnticTest.Gameplay.Entities.Factory;
using AnticTest.Data.Architecture;

namespace AnticTest.Gameplay.Game
{
	[RequireComponent(typeof(GameMap))]
	[RequireComponent(typeof(GameEntityFactory))]
	public class Gameplay : MonoBehaviour
	{
		private GameMap gameMap;
		private GameEntityFactory gameEntityFactory;

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
	}
}
