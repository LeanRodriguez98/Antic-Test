using AnticTest.Data.Gameplay.Inputs;
using AnticTest.DataModel.Grid;
using AnticTest.Gameplay.Utils;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AnticTest.Gameplay.Components
{
    public class GameCamera : UpdatableGameComponent
    {
        [SerializeField] private Transform focusPoint;
        [SerializeField] private CinemachineVirtualCamera orbitalCamera;
        [SerializeField] [Range(0.0f, 1.0f)] private float zoomPercentage;

        private CinemachineOrbitalTransposer orbitalTransposer;
        private CameraInputs cameraInputs;
        private Camera cinemachineBrain;

        private Vector3 minLimitMap;
        private Vector3 maxLimitMap;

        private float xOffset;
        private float zUpOffset;
        private float zDownOffset;

        private Vector2 moveInput;

        public override void Init()
        {
            cameraInputs = DataBlackboard.CameraInputs;
            cinemachineBrain = Camera.main;

            orbitalTransposer = orbitalCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();

            orbitalTransposer.m_FollowOffset = Vector3.Lerp(cameraInputs.zoomMinOffset, cameraInputs.zoomMaxOffset, zoomPercentage);

            Vector2Int mapsize = new Vector2Int(LogicalMap.Size.x, LogicalMap.Size.y) - Vector2Int.one;

            minLimitMap = GridUtils.CoordinateToWorld(new Coordinate(0, 0));
            maxLimitMap = GridUtils.CoordinateToWorld(new Coordinate(mapsize.x, mapsize.y));
            cameraInputs.moveInput.Enable();
            cameraInputs.moveInput.performed += OnMovePerform;
            cameraInputs.moveInput.canceled += OnMoveCanceled;

            cameraInputs.rotationInput.Enable();
            cameraInputs.rotationInput.performed += OnRotationPerform;
            cameraInputs.rotationInput.canceled += OnRotationCanceled;
            cameraInputs.zoomInput.Enable();
            cameraInputs.zoomInput.performed += OnZoomPerform;
        }

        public override void Dispose()
        {
            cameraInputs.moveInput.performed -= OnMovePerform;
            cameraInputs.moveInput.canceled -= OnMoveCanceled;
            cameraInputs.moveInput.Disable();

            cameraInputs.rotationInput.performed -= OnRotationPerform;
            cameraInputs.rotationInput.canceled -= OnRotationCanceled;
            cameraInputs.rotationInput.Disable();

            cameraInputs.zoomInput.performed -= OnZoomPerform;
            cameraInputs.zoomInput.Disable();
        }

        public override void ComponentUpdate(float deltaTime)
        {
            MoveCamera(moveInput);
            MoveCamera();
            xOffset = (orbitalTransposer.m_FollowOffset.y / 2 - cameraInputs.mapViewOffset);
            zUpOffset = (-orbitalTransposer.m_FollowOffset.z / 2);
            zDownOffset = (-orbitalTransposer.m_FollowOffset.z / 2 - cameraInputs.mapViewOffset * 1.5f);
        }

        private void OnMovePerform(InputAction.CallbackContext context) => moveInput = context.ReadValue<Vector2>();

        private void OnMoveCanceled(InputAction.CallbackContext context) => moveInput = Vector2.zero;

        private void OnRotationPerform(InputAction.CallbackContext context) => OrbitalRotation(context.ReadValue<float>());

        private void OnRotationCanceled(InputAction.CallbackContext context) => OrbitalRotation(context.ReadValue<float>());

        private void OnZoomPerform(InputAction.CallbackContext context) => CameraZoom(context.ReadValue<Vector2>());

        private void MoveCamera(Vector2 delta)
        {
            Vector3 forward = ProjectDirectionOnPlane(cinemachineBrain.transform.forward, Vector3.up);
            Vector3 right = ProjectDirectionOnPlane(cinemachineBrain.transform.right, Vector3.up);

            Vector3 targetPosition = -delta.x * right - delta.y * forward;

            Vector3 result = focusPoint.position + targetPosition * cameraInputs.moveSpeed * Time.unscaledDeltaTime;

            result.x = Mathf.Clamp(result.x, minLimitMap.x + xOffset, maxLimitMap.x - xOffset);
            result.z = Mathf.Clamp(result.z, minLimitMap.z + zDownOffset, maxLimitMap.z - zUpOffset);

            focusPoint.position = result;
        }

        private void MoveCamera()
        {
            Vector3 forward = ProjectDirectionOnPlane(cinemachineBrain.transform.forward, Vector3.up);
            Vector3 right = ProjectDirectionOnPlane(cinemachineBrain.transform.right, Vector3.up);

            Vector3 mousePosition = Mouse.current.position.ReadValue();

            Vector3 movement = Vector3.zero;

            if (mousePosition.x < cameraInputs.edgeSize && mousePosition.x >= 0)
                movement += Vector3.left;
            if (mousePosition.x > Screen.width - cameraInputs.edgeSize && mousePosition.x <= Screen.width)
                movement += Vector3.right;
            if (mousePosition.y < cameraInputs.edgeSize && mousePosition.y >= 0)
                movement += Vector3.back;
            if (mousePosition.y > Screen.height - cameraInputs.edgeSize && mousePosition.y <= Screen.height)
                movement += Vector3.forward;

            Vector3 targetPosition = movement.x * right + movement.z * forward;
            Vector3 result = focusPoint.position + targetPosition * cameraInputs.moveSpeed * Time.fixedDeltaTime;

            result.x = Mathf.Clamp(result.x, minLimitMap.x + xOffset, maxLimitMap.x - xOffset);
            result.z = Mathf.Clamp(result.z, minLimitMap.z + zDownOffset, maxLimitMap.z - zUpOffset);

            focusPoint.position = result;
        }

        private void OrbitalRotation(float side)
        {
            orbitalTransposer.m_XAxis.m_InputAxisValue = Mathf.Clamp(side, -1, 1) * cameraInputs.rotationSpeed * Time.fixedDeltaTime;
        }

        private void CameraZoom(Vector2 delta)
        {
            zoomPercentage -= delta.y * cameraInputs.zoomSpeed * Time.fixedDeltaTime;
            zoomPercentage = Mathf.Clamp01(zoomPercentage);
            orbitalTransposer.m_FollowOffset = Vector3.Lerp(cameraInputs.zoomMinOffset, cameraInputs.zoomMaxOffset, zoomPercentage);
        }

        private Vector3 ProjectDirectionOnPlane(Vector3 direction, Vector3 normal)
        {
            return (direction - normal * Vector3.Dot(direction, normal)).normalized;
        }
	}
}