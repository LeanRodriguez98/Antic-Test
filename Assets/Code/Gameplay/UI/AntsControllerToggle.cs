using AnticTest.Architecture.Events;
using AnticTest.Architecture.GameLogic.Strategies;
using AnticTest.Data.Blackboard;
using AnticTest.Gameplay.Components;
using AnticTest.Systems.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AnticTest.Gameplay.UI
{
	public class AntsControllerToggle : GameComponent
	{
		private const string IA_MODE_TEXT = "IA Mode";
		private const string MANUAL_MODE_TEXT = "Player Mode";

		[SerializeField] private Button toggleButton;
		[SerializeField] private TMP_Text toggleButtonText;
		private bool isIAMode;

		public override void Init()
		{
			toggleButton.onClick.AddListener(SwapControllerMode);

			isIAMode = DataBlackboard.AntsIAConfiguration.UseIAByDefault;

			if (isIAMode)
				toggleButtonText.text = IA_MODE_TEXT;
			else
				toggleButtonText.text = MANUAL_MODE_TEXT;
		}

		public override void Dispose()
		{
			toggleButton.onClick.RemoveAllListeners();
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
