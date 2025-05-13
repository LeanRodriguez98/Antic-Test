using AnticTest.Architecture.GameLogic;
using AnticTest.Data.Architecture;
using AnticTest.Gameplay.Audio;
using AnticTest.Gameplay.Components;
using AnticTest.Gameplay.UI;
using UnityEngine;

namespace AnticTest.Gameplay
{
	[RequireComponent(typeof(GameMap))]
	[RequireComponent(typeof(GameEntityFactory))]
	[RequireComponent(typeof(GameEntityRegistry))]
	[RequireComponent(typeof(CellSelector))]
	public class Game : MonoBehaviour
	{
		private GameEntityFactory gameEntityFactory;
		private GameEntityRegistry gameEntityRegistry;
		private GameMap gameMap;
		private CellSelector inputReader;

		private GameLogic gameLogic;

		[SerializeField] private GameUI gameUI;
		[SerializeField] private GameAudio gameAudio;
		[SerializeField] private GameCamera gameCamera;
		[SerializeField] private MapArchitectureData levelToLoad;

		private void Awake()
		{
			gameLogic = new GameLogic(levelToLoad);
			gameEntityFactory = GetComponent<GameEntityFactory>();
			gameEntityFactory.Init();
			gameEntityRegistry = GetComponent<GameEntityRegistry>();
			gameEntityRegistry.Init();
			gameMap = GetComponent<GameMap>();
			gameMap.Init();
			inputReader = GetComponent<CellSelector>();
			inputReader.Init();
			gameCamera.Init();
			gameUI.Init();
			gameAudio.Init();
			gameLogic.InitSimulation();
		}

		private void Update()
		{
			gameLogic.Update(Time.deltaTime);
			gameCamera.ComponentUpdate(Time.deltaTime);
			inputReader.ComponentUpdate(Time.deltaTime);
		}

		private void LateUpdate()
		{
			gameLogic.PostUpdate();
		}

		private void OnDisable()
		{
			gameEntityFactory.Dispose();
			gameEntityRegistry.Dispose();
			gameMap.Dispose();
			gameCamera.Dispose();
			inputReader.Dispose();
			gameUI.Dispose();
			gameAudio.Dispose();
			gameLogic.Dispose();
		}
	}
}
