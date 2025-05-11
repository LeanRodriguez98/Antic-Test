using AnticTest.Architecture.Events;
using AnticTest.DataModel.Grid;
using AnticTest.Gameplay.Utils;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AnticTest.Gameplay.Components
{
	public class CellSelector : GameComponent
	{
		private Camera gameCamera;
		private int heights;
		private Plane[] planes;
		private bool isSelectedHit;
		private ICell<Coordinate> selectedCell;

		private Grid Grid => GameMap.GetGrid();

		public override void Init()
		{
			gameCamera = Camera.main;
			heights = Enum.GetNames(typeof(CellHeight)).Length;
			planes = new Plane[heights];

			for (int i = 0; i < heights; i++)
			{
				planes[i] = new Plane(Vector3.up, Grid.transform.position + (Vector3.up * (GameMap.GetCellHeight() * i)));
			}

			DataBlackboard.SelectionInputs.leftPointerTapInput.performed += OnLeftPointerTap;
			DataBlackboard.SelectionInputs.leftPointerTapInput.Enable();

			DataBlackboard.SelectionInputs.rightPointerTapInput.performed += OnRightPointerTap;
			DataBlackboard.SelectionInputs.rightPointerTapInput.Enable();

			DataBlackboard.SelectionInputs.pointerPositionInput.performed += OnPointerPosition;
			DataBlackboard.SelectionInputs.pointerPositionInput.Enable();
		}

		public override void Dispose()
		{
			DataBlackboard.SelectionInputs.leftPointerTapInput.performed -= OnLeftPointerTap;
			DataBlackboard.SelectionInputs.leftPointerTapInput.Disable();

			DataBlackboard.SelectionInputs.rightPointerTapInput.performed -= OnRightPointerTap;
			DataBlackboard.SelectionInputs.rightPointerTapInput.Disable();

			DataBlackboard.SelectionInputs.pointerPositionInput.performed -= OnPointerPosition;
			DataBlackboard.SelectionInputs.pointerPositionInput.Disable();
		}

		private void OnLeftPointerTap(InputAction.CallbackContext context)
		{
			if (isSelectedHit)
				EventBus.Raise(new CellSelectedEvent<Coordinate>(selectedCell.GetCoordinate()));
		}

		private void OnRightPointerTap(InputAction.CallbackContext context)
		{
			EventBus.Raise(new CellDeselectedEvent());
		}

		private void OnPointerPosition(InputAction.CallbackContext context)
		{
			SelectCell(context.ReadValue<Vector2>());
		}

		private void SelectCell(Vector2 screenPosition)
		{
			Ray ray = gameCamera.ScreenPointToRay(screenPosition);

			isSelectedHit = false;

			for (int i = heights - 1; i >= 0; i--)
			{
				if (planes[i].Raycast(ray, out float enterPoint))
				{
					Vector3 worldPos = ray.GetPoint(enterPoint);
					Coordinate hitCell = GridUtils.ToCoordinate(Grid.WorldToCell(worldPos));
					selectedCell = LogicalMap.GetCell(hitCell);

					if (selectedCell != null && selectedCell.GetHeight() == (CellHeight)i &&
						LogicalMap.IsInBorders(selectedCell.GetCoordinate()))
					{
						isSelectedHit = true;
					}
				}
			}
		}
	}
}
