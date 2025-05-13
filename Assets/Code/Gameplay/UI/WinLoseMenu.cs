using AnticTest.Architecture.Events;
using AnticTest.Gameplay.Components;
using AnticTest.Systems.Events;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AnticTest.Gameplay.UI
{
	public class WinLoseMenu : GameComponent
	{
		private const string WIN_TEXT = "YOU WIN";
		private const string WIN_LOSE = "YOU LOSE";

		[SerializeField] private GameObject winLosePanel;
		[SerializeField] private TMP_Text winLoseText;
		[SerializeField] private Button reloadButton;

		public override void Init()
		{
			winLosePanel.SetActive(false);
			reloadButton.onClick.AddListener(Reload);
			EventBus.Subscribe<WinEvent>(OnWin);
			EventBus.Subscribe<LoseEvent>(OnLose);
		}

		public override void Dispose()
		{
			reloadButton.onClick.RemoveAllListeners();
			EventBus.Unsubscribe<WinEvent>(OnWin);
			EventBus.Unsubscribe<LoseEvent>(OnLose);
		}

		private void OnWin(WinEvent _)
		{
			DisplayPanel(true);
		}

		private void OnLose(LoseEvent _)
		{
			DisplayPanel(false);
		}

		private void DisplayPanel(bool win)
		{
			winLosePanel.SetActive(true);
			winLoseText.text = win ? WIN_TEXT : WIN_LOSE;
		}

		private void Reload()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}
}
