using AnticTest.Architecture.Map;
using AnticTest.Data.Architecture;
using AnticTest.Data.Gameplay;
using UnityEditor;
using UnityEngine;

namespace AnticTest.Editor.Gameplay
{
	[CustomEditor(typeof(CellGameplayData))]
	public class CellGameplayDataEditor : UnityEditor.Editor
	{
		private const char check = '✓';
		private const char cross = 'X';
		private CellGameplayData GameplayData => target as CellGameplayData;
		private CellArchitectureData ArchitectureData => GameplayData.architectureData as CellArchitectureData;

		public override void OnInspectorGUI()
		{
			GameplayData.architectureData = (CellArchitectureData)EditorGUILayout.ObjectField((CellArchitectureData)GameplayData.architectureData, typeof(CellArchitectureData), false, GUILayout.MinHeight(20));
			EditorUtility.SetDirty(target);

			if (ArchitectureData != null)
			{
				EditorGUILayout.LabelField("Cell type (from architecture data)");
				((CellArchitectureData)GameplayData.architectureData).cellType = (CellType)EditorGUILayout.EnumPopup(((CellArchitectureData)GameplayData.architectureData).cellType);
				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Cell height (from architecture data)");
				((CellArchitectureData)GameplayData.architectureData).cellHeight = (CellHeight)EditorGUILayout.EnumPopup(((CellArchitectureData)GameplayData.architectureData).cellHeight);
				EditorGUILayout.Space();

				DrawCellEditor("Full pass", "Assets/2D Assets/Grid/Editor/CellPattern - 00.png", 100, ref GameplayData.prefafs[0], CellGameplayData.Passability[0]);
				if (ArchitectureData.cellHeight != CellHeight.Zero)
				{
					DrawCellEditor("Isolated", "Assets/2D Assets/Grid/Editor/CellPattern - 01.png", 100, ref GameplayData.prefafs[1], CellGameplayData.Passability[1]);
					DrawCellEditor("Clockwize 1", "Assets/2D Assets/Grid/Editor/CellPattern - 02.png", 100, ref GameplayData.prefafs[2], CellGameplayData.Passability[2]);
					DrawCellEditor("Clockwize 2", "Assets/2D Assets/Grid/Editor/CellPattern - 03.png", 100, ref GameplayData.prefafs[3], CellGameplayData.Passability[3]);
					DrawCellEditor("Clockwize 3", "Assets/2D Assets/Grid/Editor/CellPattern - 04.png", 100, ref GameplayData.prefafs[4], CellGameplayData.Passability[4]);
					DrawCellEditor("Clockwize 4", "Assets/2D Assets/Grid/Editor/CellPattern - 05.png", 100, ref GameplayData.prefafs[5], CellGameplayData.Passability[5]);
					DrawCellEditor("Clockwize 5", "Assets/2D Assets/Grid/Editor/CellPattern - 06.png", 100, ref GameplayData.prefafs[6], CellGameplayData.Passability[6]);
					DrawCellEditor("Shape of -", "Assets/2D Assets/Grid/Editor/CellPattern - 07.png", 100, ref GameplayData.prefafs[7], CellGameplayData.Passability[7]);
					DrawCellEditor("Shape of X", "Assets/2D Assets/Grid/Editor/CellPattern - 08.png", 100, ref GameplayData.prefafs[8], CellGameplayData.Passability[8]);
					DrawCellEditor("Shape of Y", "Assets/2D Assets/Grid/Editor/CellPattern - 09.png", 100, ref GameplayData.prefafs[9], CellGameplayData.Passability[9]);
					DrawCellEditor("Shape of flipped Y", "Assets/2D Assets/Grid/Editor/CellPattern - 10.png", 100, ref GameplayData.prefafs[10], CellGameplayData.Passability[10]);
					DrawCellEditor("Shape of ▷", "Assets/2D Assets/Grid/Editor/CellPattern - 11.png", 100, ref GameplayData.prefafs[11], CellGameplayData.Passability[11]);
					DrawCellEditor("Shape of Ψ", "Assets/2D Assets/Grid/Editor/CellPattern - 12.png", 100, ref GameplayData.prefafs[12], CellGameplayData.Passability[12]);
					DrawCellEditor("Shape of V", "Assets/2D Assets/Grid/Editor/CellPattern - 13.png", 100, ref GameplayData.prefafs[13], CellGameplayData.Passability[13]);
				}
				serializedObject.ApplyModifiedProperties();
			}
		}

		private void DrawCellEditor(string cellName, string imagePath, int rectSize, ref GameObject cellPrefab, bool[] passability)
		{
			DrawCellEditor(cellName, imagePath, rectSize, ref cellPrefab, passability[0], passability[1], passability[2], passability[3], passability[4], passability[5]);
		}

		private void DrawCellEditor(string cellName, string imagePath, int rectSize, ref GameObject cellPrefab,
								   bool right, bool rightDown, bool leftDown, bool left, bool leftUp, bool rightUp)
		{
			GUILayout.BeginHorizontal();
			Texture image = (Texture)AssetDatabase.LoadAssetAtPath(imagePath, typeof(Texture));
			GUILayout.Box(image, GUILayout.MinHeight(rectSize), GUILayout.MinWidth(rectSize), GUILayout.MaxHeight(rectSize), GUILayout.MaxWidth(rectSize));
			GUILayout.BeginVertical();
			EditorGUILayout.LabelField("Passability: " + cellName);

			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Right: " + (right ? check : cross));
			EditorGUILayout.LabelField("Right Down: " + (rightDown ? check : cross));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Left Down: " + (leftDown ? check : cross));
			EditorGUILayout.LabelField("Left: " + (left ? check : cross));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Left Up: " + (leftUp ? check : cross));
			EditorGUILayout.LabelField("Right Up: " + (rightUp ? check : cross));
			GUILayout.EndHorizontal();

			cellPrefab = EditorGUILayout.ObjectField(cellPrefab, typeof(GameObject), false, GUILayout.MinHeight(20)) as GameObject;
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			EditorGUILayout.Space(20);
		}
	}
}