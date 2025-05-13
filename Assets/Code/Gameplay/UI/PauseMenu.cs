using AnticTest.Gameplay.Components;
using AnticTest.Gameplay.Events;
using AnticTest.Systems.Events;
using UnityEngine;
using UnityEngine.UI;

namespace AnticTest.Gameplay.UI
{
	public class PauseMenu : GameComponent
	{
		[SerializeField] private GameObject pausePanel;
		[SerializeField] private Button pauseButton;
		[SerializeField] private Button resumeButton;
		[SerializeField] private Button toggleMusicButton;
		[SerializeField] private Button exitButton;

		public override void Init()
		{
			pausePanel.SetActive(false);
			pauseButton.onClick.AddListener(Pause);
			resumeButton.onClick.AddListener(Resume);
			toggleMusicButton.onClick.AddListener(ToggleMusic);
			exitButton.onClick.AddListener(Exit);
		}

		public override void Dispose()
		{
			pauseButton.onClick.RemoveListener(Pause);
			resumeButton.onClick.RemoveListener(Resume);
			toggleMusicButton.onClick.RemoveListener(ToggleMusic);
			exitButton.onClick.RemoveListener(Exit);
		}

		private void Pause()
		{
			pausePanel.SetActive(true);
			Time.timeScale = 0.0f;
		}

		private void Resume()
		{
			pausePanel.SetActive(false);
			Time.timeScale = 1.0f;
		}

		private void ToggleMusic()
		{
			EventBus.Raise(new ToggleMusicEvent());
		}

		private void Exit()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}
