using AnticTest.Gameplay.Events;
using AnticTest.Systems.Events;
using AnticTest.Systems.Provider;
using UnityEngine;
using UnityEngine.UI;

namespace AnticTest.Gameplay.PauseMenu
{
	public class PauseMenu : MonoBehaviour
	{
		private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

		[SerializeField] private GameObject pausePanel;
		[SerializeField] private Button pauseButton;
		[SerializeField] private Button resumeButton;
		[SerializeField] private Button toggleMusicButton;
		[SerializeField] private Button exitButton;

		void OnEnable()
		{
			pauseButton.onClick.AddListener(Pause);
			resumeButton.onClick.AddListener(Resume);
			toggleMusicButton.onClick.AddListener(ToggleMusic);
			exitButton.onClick.AddListener(Exit);
		}


		private void OnDisable()
		{
			pauseButton.onClick.RemoveListener(Pause);
			resumeButton.onClick.RemoveListener(Resume);
			toggleMusicButton.onClick.RemoveListener(ToggleMusic);
			exitButton.onClick.RemoveListener(Exit);
		}

		private void Pause()
		{
			pausePanel.SetActive(true);
		}

		private void Resume()
		{
			pausePanel.SetActive(false);
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
