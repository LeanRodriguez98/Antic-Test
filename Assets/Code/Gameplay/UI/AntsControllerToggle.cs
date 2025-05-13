using AnticTest.Architecture.Events;
using AnticTest.Architecture.GameLogic.Strategies;
using AnticTest.Data.Blackboard;
using AnticTest.Systems.Events;
using AnticTest.Systems.Provider;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AnticTest.Gameplay.UI
{
	public class AntsControllerToggle : MonoBehaviour
	{
		private DataBlackboard DataBlackboard => ServiceProvider.Instance.GetService<DataBlackboard>();
		private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

		private const string IA_MODE_TEXT = "IA Mode";
		private const string MANUAL_MODE_TEXT = "Player Mode";

		[SerializeField] private Button toggleButton;
		[SerializeField] private TMP_Text toggleButtonText;
		private bool isIAMode;

		private void Start()
		{
			isIAMode = DataBlackboard.AntsIAConfiguration.UseIAByDefault;

			if (isIAMode)
				toggleButtonText.text = IA_MODE_TEXT;
			else
				toggleButtonText.text = MANUAL_MODE_TEXT;
		}

		private void OnEnable()
		{
			toggleButton.onClick.AddListener(SwapControllerMode);
		}

		private void OnDisable()
		{
			toggleButton.onClick.RemoveListener(SwapControllerMode);
		}

		private void SwapControllerMode()
		{
			if (isIAMode)
			{
				toggleButtonText.text = MANUAL_MODE_TEXT;
				EventBus.Raise(new SwapAntsStrategyEvent(AntStrategies.Manual));
			}
			else
			{
				toggleButtonText.text = IA_MODE_TEXT;
				EventBus.Raise(new SwapAntsStrategyEvent(AntStrategies.IA));
			}
			isIAMode = !isIAMode;
		}
	}
}
