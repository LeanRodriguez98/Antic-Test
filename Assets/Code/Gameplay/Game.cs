using AnticTest.Architecture.GameLogic;
using AnticTest.Architecture.Map;
using AnticTest.Data.Blackboard;
using UnityEngine;

namespace AnticTest.Gameplay.Game
{
	public class Gameplay : MonoBehaviour
	{
		private GameLogic gameLogic;
		private DataBlackboard dataBlackboard;
		[SerializeField] private int SizeX = 30;
		[SerializeField] private int SizeY = 30;

		private void Awake()
		{
			dataBlackboard = new DataBlackboard();
			gameLogic = new GameLogic(new Coordinate(SizeX, SizeY));
		}
	}
}
