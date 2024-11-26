using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _playerPosition; 
    [SerializeField] private float _distance = 5f; // _distance from the player
    [SerializeField] private float _height = 2f; // _height above the player
    [SerializeField] private float _rotationSpeed = 5f; 
    [SerializeField] private bool _requireRightClick = true; // If true, right-click is required to rotate
    [SerializeField] private bool _isInverted = false; // Controls Y-axis inversion

    private float _currentRotationX = 0f;
    private float _currentRotationY = 0f;
    private const float MIN_VERTICAL_ANGLE = -40f; // Prevent camera from going too low
    private const float MAX_VERTICAL_ANGLE = 80f; // Prevent camera from going too high

    private void LateUpdate()
    {
        if (_playerPosition == null)
        {
            Debug.Log("CameraController: No PlayerPosition assigned");
            return;
        }

        RotateCameraAroundPlayer();
    }

    // Rotate the camera based on mouse input
    private void RotateCameraAroundPlayer()
    {
        float l_mouseX = Input.GetAxis("Mouse X") * _rotationSpeed;
        float l_mouseY = Input.GetAxis("Mouse Y") * _rotationSpeed;

        if (!_requireRightClick || Input.GetMouseButton(1)) // 1 is right mouse button
        {
            _currentRotationX += l_mouseX;
            Debug.Log($"currentRotationX: {_currentRotationX}");

            float l_verticalRotation = _isInverted ? -l_mouseY : l_mouseY;
            _currentRotationY = Mathf.Clamp(_currentRotationY + l_verticalRotation, MIN_VERTICAL_ANGLE, MAX_VERTICAL_ANGLE);
            // _currentRotationY += l_verticalRotation;
            Debug.Log($"currentRotationY: {_currentRotationY}");
        }

        Vector3 l_direction = Quaternion.Euler(_currentRotationY, _currentRotationX, 0) * Vector3.forward;
        Vector3 l_desiredPosition = _playerPosition.position - (l_direction * _distance) + (Vector3.up * _height);

        // Set the camera's position and look at the player
        transform.position = l_desiredPosition;
        transform.LookAt(_playerPosition.position + (Vector3.up * _height / 2));
    }
}