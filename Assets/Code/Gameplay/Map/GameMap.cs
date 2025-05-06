using AnticTest.Architecture.GameLogic;
using AnticTest.Architecture.Map;
using AnticTest.Architecture.Services;
using UnityEngine;

namespace AnticTest.Gameplay.Map
{
	[RequireComponent(typeof(Grid))]
	public class GameMap : MonoBehaviour, IService
	{
		private Grid<Cell> LogicalGrid => ServiceProvider.Instance.GetService(typeof(Grid<Cell>)) as Grid<Cell>;
		private Grid gameGrid;
		[SerializeField] private float cellheight;

		private void Start()
		{
			gameGrid = GetComponent<Grid>();
			ServiceProvider.Instance.AddService(typeof(GameMap), this);
		}

		public float GetMapHeight() 
		{
			return cellheight;
		}
	}
}
