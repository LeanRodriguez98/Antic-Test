using AnticTest.Architecture.GameLogic;
using AnticTest.Architecture.GameLogic.Strategies;
using AnticTest.Architecture.States;
using AnticTest.Data.Blackboard;
using AnticTest.DataModel.Entities;
using AnticTest.DataModel.Grid;
using AnticTest.Systems.Provider;
using UnityEditor;
using UnityEngine;

namespace AnticTest.Editor.Architecture
{
	public class StrategyDebugWindow : EditorWindow
	{
		private DataBlackboard DataBlackboard => ServiceProvider.Instance.GetService<DataBlackboard>();
		private EntitiesLogic<Cell<Coordinate>, Coordinate> EntitiesLogic =>
			ServiceProvider.Instance.GetService<EntitiesLogic<Cell<Coordinate>, Coordinate>>();
		private EntityRegistry<Cell<Coordinate>, Coordinate> EntityRegistry =>
			ServiceProvider.Instance.GetService<EntityRegistry<Cell<Coordinate>, Coordinate>>();
		private AntsIAStrategy<Cell<Coordinate>, Coordinate> AntsIAStrategy =>
			EntitiesLogic.Strategies[AntStrategies.IA] as AntsIAStrategy<Cell<Coordinate>, Coordinate>;

		private Vector2 scrollPosition;

		private enum Tabs
		{
			AntsStatus = 0,
			StrategyInfo = 1
		}

		private readonly string[] TAB_TITLES = { "Ants Status", "Strategy info" };
		private Tabs selectedTab = Tabs.AntsStatus;

		[MenuItem("Antic Test/Strategy Debug Window")]
		public static void ShowWindow()
		{
			GetWindow<StrategyDebugWindow>("Strategy Debug Window");
		}

		private void Update()
		{
			if (Application.isPlaying)
			{
				Repaint();
			}
		}

		private void OnGUI()
		{
			if (!Application.isPlaying)
			{
				EditorGUILayout.HelpBox("The Strategy Debug Window is only functional during Play mode.", MessageType.Info);
				return;
			}

			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition,
															alwaysShowHorizontal: true,
															alwaysShowVertical: false);

			switch (EntitiesLogic.AntsCurrentStrategy)
			{
				case AntStrategies.IA:
					selectedTab = (Tabs)GUILayout.Toolbar((int)selectedTab, TAB_TITLES, GUILayout.Height(30));
					switch (selectedTab)
					{
						case Tabs.AntsStatus:
							DrawAntStatus();
							break;
						case Tabs.StrategyInfo:
							DrawStrategyInfo();
							break;
					}
					break;
				case AntStrategies.Manual:
					GUILayout.Label("Ants are in manual control mode.", EditorStyles.boldLabel);
					break;
			}

			EditorGUILayout.EndScrollView();
		}

		private void DrawAntStatus()
		{
			foreach (Ant<Cell<Coordinate>, Coordinate> ant in EntityRegistry.Ants)
			{
				EditorGUILayout.LabelField("Ant[ " + ant.GetID() + "]");
				EditorGUILayout.LabelField("\tHealth: " + ant.Health);
				EditorGUILayout.LabelField("\tCurrent state: " + (AntStates)ant.GetFSMState());
				switch ((AntStates)ant.GetFSMState())
				{
					case AntStates.Movement:
						EditorGUILayout.LabelField("\t\tTarget coordinate: " + ant.Destiny.ToString());
						break;
					case AntStates.Defend:
						for (int i = 0; i < ant.CurrentOponents.Count; i++)
						{
							EditorGUILayout.LabelField("\t\tCurrent oponent[" + i + "]: ");
							EditorGUILayout.LabelField("\t\t\tHealth:" + ant.CurrentOponents[i].Health);
							EditorGUILayout.LabelField("\t\t\tDamage:" + ant.CurrentOponents[i].Damage);
							EditorGUILayout.LabelField("\t\t\tTime between attacks:" + ant.CurrentOponents[i].TimeBetweenAtacks);
						}
						break;
					case AntStates.Patrol:
						EditorGUILayout.LabelField("\t\tLooking for enemies");
						break;
				}
			}
		}

		private void DrawStrategyInfo()
		{
			EditorGUILayout.LabelField("Potential threats in order:");

			foreach (uint potentialThreatId in AntsIAStrategy.PotentialThreats)
			{
				EnemyBug<Cell<Coordinate>, Coordinate> enemy =
					(EnemyBug<Cell<Coordinate>, Coordinate>)EntityRegistry[potentialThreatId];
				float distanceRisk = enemy.GetCoordinate().Distance(EntityRegistry.Flag.GetCoordinate()) *
					DataBlackboard.AntsIAConfiguration.EnemyDistanceWeightMultiplier;
				float healthRisk = enemy.Health * DataBlackboard.AntsIAConfiguration.EnemyHealthWeightMultiplier;
				float damageRisk = enemy.Damage * DataBlackboard.AntsIAConfiguration.EnemyDamageWeightMultiplier;
				float speedRisk = enemy.Speed * DataBlackboard.AntsIAConfiguration.EnemySpeedWeightMultiplier;
				float totalRisk = distanceRisk + healthRisk + damageRisk + speedRisk;

				EditorGUILayout.LabelField("\tEnemy[" + potentialThreatId + "] - Risk:" + totalRisk);
				EditorGUILayout.LabelField("\t\tDistance Risk: " + distanceRisk);
				EditorGUILayout.LabelField("\t\tHealth Risk: " + healthRisk);
				EditorGUILayout.LabelField("\t\tDamage Risk: " + damageRisk);
				EditorGUILayout.LabelField("\t\tSpeed Risk: " + speedRisk);
			}
		}
	}
}