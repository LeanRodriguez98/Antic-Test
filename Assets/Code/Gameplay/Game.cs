using AnticTest.Architecture.GameLogic;
using AnticTest.Architecture.Map;
using UnityEngine;

namespace AnticTest.Gameplay.Game
{
	public class Gameplay : MonoBehaviour
	{
		private GameLogic gameLogic;
		[SerializeField] private int SizeX = 30;
		[SerializeField] private int SizeY = 30;

		private void Awake()
		{
			gameLogic = new GameLogic(new Coordinate(SizeX, SizeY));
		}
	}
}
