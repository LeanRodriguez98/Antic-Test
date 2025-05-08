using AnticTest.Architecture.GameLogic;
using AnticTest.DataModel.Map;
using AnticTest.Data.Blackboard;
using UnityEngine;
using AnticTest.Gameplay.Map;
using AnticTest.Gameplay.Entities.Factory;

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
		[SerializeField] private int SizeX = 30;
		[SerializeField] private int SizeY = 30;

		private void Awake()
		{
			dataBlackboard = new DataBlackboard();
			gameLogic = new GameLogic(new Coordinate(SizeX, SizeY));
			gameEntityFactory = GetComponent<GameEntityFactory>();
			gameEntityFactory.Init();
			gameMap = GetComponent<GameMap>();
			gameMap.Init();
			gameLogic.InitSimulation();
		}
	}
}
