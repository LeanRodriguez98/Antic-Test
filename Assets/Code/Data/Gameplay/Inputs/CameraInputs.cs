using UnityEngine;
using UnityEngine.InputSystem;

namespace AnticTest.Data.Gameplay.Inputs
{
	[CreateAssetMenu(fileName = "New Camera Inputs", menuName = "AnticTest/Data/Gameplay/Inputs/Camera Inputs")]
	public class CameraInputs : ScriptableObject
	{
		[Range(0.0f, 10.0f)] public float moveSpeed;
		[Range(0.0f, 180.0f)] public float rotationSpeed;
		[Range(0.0f, 5.0f)] public float zoomSpeed;
		public Vector3 zoomMinOffset;
		public Vector3 zoomMaxOffset;
		public float edgeSize;
		public float mapViewOffset;

		public InputAction moveInput;
		public InputAction rotationInput;
		public InputAction zoomInput;
	}
}
