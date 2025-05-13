using AnticTest.Gameplay.Components;
using UnityEngine;

namespace AnticTest.Gameplay.UI
{
    [RequireComponent(typeof(AntsControllerToggle))]
    [RequireComponent(typeof(PauseMenu))]
    [RequireComponent(typeof(WinLoseMenu))]
    public class GameUI : GameComponent
    {
        private AntsControllerToggle antsControllerToggle;
        private PauseMenu pauseMenu;
        private WinLoseMenu winLoseMenu;

		public override void Init()
		{
            antsControllerToggle = GetComponent<AntsControllerToggle>();
            antsControllerToggle.Init();
            pauseMenu = GetComponent<PauseMenu>();
            pauseMenu.Init();
            winLoseMenu = GetComponent<WinLoseMenu>();
            winLoseMenu.Init();
        }

		public override void Dispose()
		{
            antsControllerToggle.Dispose();
            pauseMenu.Dispose();
            winLoseMenu.Dispose();
        }
    }
}
