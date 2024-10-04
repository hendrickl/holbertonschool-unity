using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _playerPosition; 
    [SerializeField] private float _distance = 5.0f; // _distance from the player
    [SerializeField] private float _height = 2.0f; // _height above the player
    [SerializeField] private float _rotationSpeed = 5.0f; 
    [SerializeField] private bool _requireRightClick = true; // If true, right-click is required to rotate

    private float _currentRotation = 0.0f;

    private void LateUpdate()
    {
        if (_playerPosition == null)
        {
            Debug.Log("CameraController: No PlayerPosition assigned");
            return;
        }

        RotateCameraAroundPlayer();
    }

    private void RotateCameraAroundPlayer()
    {
        // Rotate the camera based on mouse input
        float l_mouseX = Input.GetAxis("Mouse X") * _rotationSpeed;

        if (!_requireRightClick || Input.GetMouseButton(1)) // 1 is right mouse button
        {
            _currentRotation += l_mouseX;
        }

        // Calculate the desired position
        Vector3 l_desiredPosition = _playerPosition.position - (Quaternion.Euler(0, _currentRotation, 0) * Vector3.forward * _distance) + (Vector3.up * _height);

        // Set the camera's position and look at the player
        transform.position = l_desiredPosition;
        transform.LookAt(_playerPosition.position + (Vector3.up * _height / 2)); // Look at the upper body of the player
    }
}