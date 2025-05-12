using AnticTest.Architecture.Events;
using AnticTest.DataModel.Grid;
using AnticTest.Gameplay.Utils;
using AnticTest.Systems.Events;
using System.Collections.Generic;
using UnityEngine;

namespace AnticTest.Gameplay.Components
{
	public class CellSelectorViewer : UpdatableGameComponent
	{
		private const int HEX_SIDES = 6;
		private const float DRAW_ROTATION = 30.0f;

		public float drawRadius = 0.85f;
		public float drawHeight = 0.1f;
		public float drawWidth = 0.1f;
		
		private LineRenderer cellSelectionRenderer;
		private Coordinate selectedCoordinate;

		public override void Init()
		{
			cellSelectionRenderer = GenerateRenderer(Color.white);

			EventBus.Subscribe<CellSelectedEvent<Coordinate>>(OnCellSelected);
			EventBus.Subscribe<CellDeselectedEvent>(OnCellDeselected);
		}

		public override void Dispose()
		{
			EventBus.Unsubscribe<CellSelectedEvent<Coordinate>>(OnCellSelected);
			EventBus.Unsubscribe<CellDeselectedEvent>(OnCellDeselected);
		}

		private void OnCellSelected(CellSelectedEvent<Coordinate> cellSelectedEvent)
		{
			selectedCoordinate = cellSelectedEvent.selectedCoordinate;
		}

		private void OnCellDeselected(CellDeselectedEvent cellDeselectedEvent)
		{
			selectedCoordinate.SetAsInvalid();
			cellSelectionRenderer.gameObject.SetActive(false);
		}

		public override void ComponentUpdate(float deltaTime)
		{
			if (!selectedCoordinate.InInvalid())
			{
				DrawHexa(GridUtils.CoordinateToWorld(selectedCoordinate), cellSelectionRenderer);
			}
		}

		private void DrawHexa(Vector3 center, LineRenderer renderer)
		{
			if (!renderer.gameObject.activeSelf)
				renderer.gameObject.SetActive(true);

			renderer.startWidth = drawWidth;
			renderer.endWidth = drawWidth;

			for (int i = 0; i < HEX_SIDES; i++)
			{
				float angle = ((float)i / HEX_SIDES) * 2.0f * Mathf.PI;
				float x = Mathf.Cos(angle) * drawRadius;
				float z = Mathf.Sin(angle) * drawRadius;
				Vector3 point = Quaternion.Euler(Vector3.up * DRAW_ROTATION) * new Vector3(x, drawHeight, z);
				renderer.SetPosition(i, center + point);
			}
		}

		private Dictionary<Color, Material> usedMaterials = new Dictionary<Color, Material>();
		private LineRenderer GenerateRenderer(Color color)
		{
			if (!usedMaterials.ContainsKey(color))
			{
				usedMaterials.Add(color, new Material(Shader.Find("Unlit/Color")));
				usedMaterials[color].color = color;
			}
			GameObject container = new GameObject("LineRenderer container");
			container.transform.parent = this.transform;
			LineRenderer lineRenderer = container.AddComponent<LineRenderer>();
			lineRenderer.material = usedMaterials[color];
			lineRenderer.startColor = color;
			lineRenderer.endColor = color;
			lineRenderer.startWidth = drawWidth;
			lineRenderer.endWidth = drawWidth;
			lineRenderer.shadowCastingMode = 0;
			lineRenderer.receiveShadows = false;
			lineRenderer.staticShadowCaster = false;
			lineRenderer.loop = true;
			lineRenderer.positionCount = HEX_SIDES;
			container.SetActive(false);
			return lineRenderer;
		}
	}
}
