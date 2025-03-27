using UnityEngine;

// This script manages the behaviour of the player
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 300f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private Transform _groundContactPoint;
    [SerializeField] private float _fallThreshold = -20f;
    [SerializeField] private Vector3 _startPosition; 
    [SerializeField] private Vector3 _resetPosition; 
    [SerializeField] private LayerMask _ground;

    private Rigidbody _rigidbody;
    private bool _playerHasPermissionToMove;
    private bool _playerHasLandedOnGround;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); 
        transform.position = _startPosition;
        _playerHasPermissionToMove = false;
        _playerHasLandedOnGround = false;
    }

    private void Update()
    {
        // We check if the player touched the ground for the first time
        if (!_playerHasLandedOnGround && IsGrounded())
        {
            _playerHasLandedOnGround = true;
            _playerHasPermissionToMove = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && _playerHasLandedOnGround)
        {
            Jump();
        }

        MovePlayerLinearly();
        RotatePlayer();
        CheckFall();
    }

    // Move the player based on WASD and arrows input
    private void MovePlayerLinearly()
    {
        if (_playerHasPermissionToMove)
        {
            float horizontalInput = Input.GetAxis("Horizontal"); 
            float verticalInput = Input.GetAxisRaw("Vertical"); 

            // Use the player's facing direction for movement
            Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
            moveDirection.Normalize();

            _rigidbody.velocity = new Vector3(moveDirection.x * _moveSpeed, _rigidbody.velocity.y, moveDirection.z * _moveSpeed);
            // _rigidbody.velocity = new Vector3(horizontalInput * _moveSpeed, _rigidbody.velocity.y, verticalInput * _moveSpeed);
        }
        else 
        {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0); // The player can fall under the effect of gravity but cannot move horizontally
        }
    }

    // Rotate player gradually based on horizontal input 
    private void RotatePlayer()
    {
        if (_playerHasPermissionToMove)
        {
            float horizontalInput = Input.GetAxis("Horizontal");

            if (horizontalInput != 0)
            {
                float rotationAngle = horizontalInput * _rotationSpeed * Time.deltaTime;
                transform.Rotate(0, rotationAngle, 0);
            }   
        }
    }

    // Make jump the player when the space button is pressed
    private void Jump()
    {
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _jumpForce, _rigidbody.velocity.z);
        Debug.Log($"Jump is called");
    }

    // Check if the player is grounded
    private bool IsGrounded()
    {
        bool isGrounded = Physics.CheckSphere(_groundContactPoint.position, 0.1f, _ground);
        // Debug.Log($"PlayerController - IsGrounded : {isGrounded}");
        return isGrounded;
    }

    private void CheckFall()
    {
        if (transform.position.y < _fallThreshold)
        {
            ResetPlayerPosition();

            // If the player falls the control state is reset
            if (_playerHasLandedOnGround) 
            {
                _playerHasPermissionToMove = true; // Keep controls enabled after fall if already enabled previously 
            }
            else
            {
                _playerHasPermissionToMove = false;
            }
        }
    }

    private void ResetPlayerPosition()
    {
        transform.position = _resetPosition;
    }

    // Method to reset player state if necessary
    public void ResetPlayerState()
    {
        _playerHasLandedOnGround = false;
        _playerHasPermissionToMove = false;
    }
}