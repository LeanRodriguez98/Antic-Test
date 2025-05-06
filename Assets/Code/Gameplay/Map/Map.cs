using AnticTest.Architecture.GameLogic;
using AnticTest.Architecture.Map;
using AnticTest.Architecture.Services;
using UnityEngine;

namespace AnticTest.Gameplay.Map
{
	[RequireComponent(typeof(Grid))]
	public class Map : MonoBehaviour
	{
		private Grid<Cell> LogicalGrid => ServiceProvider.Instance.GetService(typeof(Grid<Cell>)) as Grid<Cell>;
		private Grid gameGrid;

		private void Start()
		{
			gameGrid = GetComponent<Grid>();
		}
	}
}
