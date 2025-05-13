using AnticTest.Data.Architecture;
using CSharpExtensions.GridMatrix;
using UnityEditor;
using UnityEngine;

namespace AnticTest.Editor.Architecture
{
	[CustomEditor(typeof(MapArchitectureData))]
	public class MapArchitectureDataEditor : UnityEditor.Editor
	{
		private MapArchitectureData Target => target as MapArchitectureData;

		private const float cellRadius = 20.0f;
		private Vector2 offset = new Vector2(30.0f, 10.0f);

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if (GUILayout.Button("Regenerate Map"))
				RegenerateMap();
			if (CanDrawCustomEditor())
			{
				(int x, int y) Size = Target.Map.Lenght;
				Vector2 gridSize = new Vector2(Size.x * cellRadius * 2.0f, Size.y * cellRadius * 2.0f);
				Rect drawArea = GUILayoutUtility.GetRect(gridSize.x, gridSize.y + 100.0f);
				Handles.BeginGUI();
				DrawCellGrid(drawArea);
				Handles.EndGUI();
			}
			serializedObject.ApplyModifiedProperties();
		}

		private bool CanDrawCustomEditor()
		{
			return Target.MapSize.x > 0 && Target.MapSize.y > 0 && Target.CellsInMap.Count > 0 && Target.CellsInMap[0].cellArchitectureData != null;
		}

		private void RegenerateMap()
		{
			if (Target.CellsInMap != null && Target.CellsInMap.Count > 0 &&
				Target.CellsInMap[0].cellArchitectureData != null)
			{
				MapArchitectureData.CellData defaultCellData = new MapArchitectureData.CellData();
				defaultCellData.cellArchitectureData = Target.CellsInMap[0].cellArchitectureData;
				defaultCellData.entityArchitectureData = null;
				if (Target.Map == null)
					Target.Map = new GridMatrix<MapArchitectureData.CellData>(Target.MapSize.x, Target.MapSize.y, defaultCellData);
				else
					Target.Map.Resize(Target.MapSize.x, Target.MapSize.y, defaultCellData);

				EditorUtility.SetDirty(Target);
				AssetDatabase.SaveAssets();
			}
		}

		private void DrawCellGrid(Rect drawArea)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			(int x, int y) Size = Target.Map.Lenght;
			for (int y = 0; y < Size.y; y++)
			{
				for (int x = 0; x < Size.x; x++)
				{
					Vector2Int cellCoord = new Vector2Int(x, y);
					Vector2 cellPixelPosition = CellToPixel(x, Size.y - y);
					Vector2 screenPosition = drawArea.position + cellPixelPosition + offset;

					Color fillColor = GetCellArchitectureDataColor(Target.Map[x, y].cellArchitectureData);

					DrawCell(screenPosition, cellRadius, fillColor);

					if (Event.current.type == EventType.MouseDown &&
						Event.current.button == 1 &&
						Vector2.Distance(mousePosition, screenPosition) < cellRadius)
					{
						ShowContextMenu(cellCoord);
						Event.current.Use();
					}

					DrawEntities(cellCoord, screenPosition);
				}
			}
		}

		private Color GetCellArchitectureDataColor(CellArchitectureData cellArchitectureData)
		{
			foreach (MapArchitectureData.CellArchitectureDataEditorRepresentation cellRepresentation in Target.CellsInMap)
			{
				if (cellRepresentation.cellArchitectureData == cellArchitectureData)
				{
					return cellRepresentation.representativeColor;
				}
			}
			return Color.magenta;
		}

		private char GetEntityArchitectureDataChar(ArchitectureData entityArchitectureData)
		{
			foreach (MapArchitectureData.EntityArchitectureDataEditorRepresentation entityRepresentation in Target.EntitiesInMap)
			{
				if (entityRepresentation.entityArchitectureData == entityArchitectureData)
				{
					return entityRepresentation.representativeChar;
				}
			}
			return ' ';
		}

		private Vector2 CellToPixel(int x, int y)
		{
			float width = Mathf.Sqrt(3) * cellRadius;
			float height = 2.0f * cellRadius;
			float verticalSpacing = height * 0.75f;

			float offsetX = (y % 2 == 1) ? -width / 2.0f : 0.0f;

			return new Vector2(x * width + offsetX, y * verticalSpacing);
		}

		private void DrawCell(Vector2 center, float radius, Color fillColor)
		{
			Vector3[] corners = new Vector3[6];
			for (int i = 0; i < 6; i++)
			{
				float angleDegrees = 60.0f * i - 30.0f;
				float angleRadians = Mathf.Deg2Rad * angleDegrees;
				corners[i] = new Vector3(
					center.x + radius * Mathf.Cos(angleRadians),
					center.y + radius * Mathf.Sin(angleRadians),
					0
				);
			}

			Handles.color = fillColor;
			Handles.DrawAAConvexPolygon(corners);

			Handles.color = Color.black;
			Vector3[] outline = new Vector3[7];
			for (int i = 0; i < 6; i++)
			{
				outline[i] = corners[i];
			}
			outline[6] = corners[0];
			Handles.DrawPolyLine(outline);
		}

		private void DrawEntities(Vector2Int cellCoordinate, Vector2 screenPosition)
		{
			ArchitectureData entityArchitectureData = Target.Map[cellCoordinate.x, cellCoordinate.y].entityArchitectureData;
			if (entityArchitectureData != null)
			{
				char representativeChar = GetEntityArchitectureDataChar(entityArchitectureData);
				GUIStyle style = new GUIStyle(GUI.skin.label);
				style.fontSize = 24;
				style.normal.textColor = Color.black;
				Vector2 size = style.CalcSize(new GUIContent(representativeChar.ToString()));
				Vector2 charPosition = screenPosition - size / 2.0f;
				GUI.Label(new Rect(charPosition, size), representativeChar.ToString(), style);
			}
		}

		private void ShowContextMenu(Vector2Int cellCoordinate)
		{
			GenericMenu menu = new GenericMenu();

			void AddCellArchitectureDataOption(string name, CellArchitectureData cellArchitectureData)
			{
				menu.AddItem(new GUIContent("CellArchitectureData/" + name), false, () =>
				{
					MapArchitectureData.CellData newData = new MapArchitectureData.CellData();
					newData.cellArchitectureData = cellArchitectureData;
					newData.entityArchitectureData = Target.Map[cellCoordinate.x, cellCoordinate.y].entityArchitectureData;
					Target.Map.Set(cellCoordinate.x, cellCoordinate.y, newData);
					EditorUtility.SetDirty(Target);
					AssetDatabase.SaveAssets();
					Repaint();
				});
			}

			foreach (MapArchitectureData.CellArchitectureDataEditorRepresentation dataRepresentation in Target.CellsInMap)
			{
				AddCellArchitectureDataOption(dataRepresentation.cellArchitectureData.name, dataRepresentation.cellArchitectureData);
			}

			void AddEntityArchitectureDataOption(string name, ArchitectureData entityArchitectureData)
			{
				menu.AddItem(new GUIContent("EntityArchitectureData/" + name), false, () =>
				{
					MapArchitectureData.CellData newData = new MapArchitectureData.CellData();
					newData.cellArchitectureData = Target.Map[cellCoordinate.x, cellCoordinate.y].cellArchitectureData;
					newData.entityArchitectureData = entityArchitectureData;
					Target.Map.Set(cellCoordinate.x, cellCoordinate.y, newData);
					EditorUtility.SetDirty(Target);
					AssetDatabase.SaveAssets();
					Repaint();
				});
			}

			foreach (MapArchitectureData.EntityArchitectureDataEditorRepresentation dataRepresentation in Target.EntitiesInMap)
			{
				AddEntityArchitectureDataOption(dataRepresentation.entityArchitectureData.name, dataRepresentation.entityArchitectureData);
			}

			if (Target.Map[cellCoordinate.x, cellCoordinate.y].entityArchitectureData != null)
				AddEntityArchitectureDataOption("Remove entity", null);

			menu.ShowAsContext();
		}
	}
}