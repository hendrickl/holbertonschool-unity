using UnityEngine;

// This script manages the behaviour of the player
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 300f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _fallThreshold = -20f;
    [SerializeField] private Transform _groundContactPoint;
    [SerializeField] private Vector3 _startPosition; 
    [SerializeField] private Vector3 _resetPosition; 
    [SerializeField] private LayerMask _ground;
    [SerializeField] private Animator _animator;

    private Rigidbody _rigidbody;
    private bool _playerHasPermissionToMove;
    private bool _playerHasLandedOnGround;
    private bool _playerIsRunning;
    private bool _playerIsJumping;
    private bool _playerWasGroundedLastFrame; // Detect when the player lands

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); 

        transform.position = _startPosition;
        
        _playerHasPermissionToMove = false;
        _playerHasLandedOnGround = false;
        _playerIsRunning = false;
        _playerIsJumping = false;
        _playerWasGroundedLastFrame = false; 
    }

    private void Update()
    {
        // Check if the player touched the ground for the first time
        if (!_playerHasLandedOnGround && PlayerIsGrounded())
        {
            _playerHasLandedOnGround = true;
            _playerHasPermissionToMove = true;
        }

        // Detect landing from jump
        if (_playerIsJumping && !_playerWasGroundedLastFrame && PlayerIsGrounded())
        {
            _playerIsJumping = false;
        }

        _playerWasGroundedLastFrame = PlayerIsGrounded(); // Remember grounded state for the next frame

        if (Input.GetKeyDown(KeyCode.Space) && PlayerIsGrounded() && _playerHasLandedOnGround)
        {
            Jump();
        }

        RotatePlayer();
        MovePlayer();
        CheckFall();
        UpdateAnimations();
    }

    // Move the player based on WASD and arrows input
    private void MovePlayer()
    {
        if (_playerHasPermissionToMove)
        {
            float verticalInput = Input.GetAxisRaw("Vertical"); 

            // Use the player's facing direction for movement
            Vector3 movementOnZ = transform.forward * verticalInput;
            Vector3 moveDirection = movementOnZ;
            moveDirection.Normalize();

            _rigidbody.velocity = new Vector3(moveDirection.x * _moveSpeed, _rigidbody.velocity.y, moveDirection.z * _moveSpeed);
            
            _playerIsRunning = true;
        }
        else 
        {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0); // The player can fall under the effect of gravity but cannot move horizontally
            _playerIsRunning = false;
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
        _playerIsJumping = true;
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _jumpForce, _rigidbody.velocity.z);
        Debug.Log($"Jump is called");
    }

    // Check if the player is grounded
    private bool PlayerIsGrounded()
    {
        bool playerIsGrounded = Physics.CheckSphere(_groundContactPoint.position, 0.1f, _ground);
        // Debug.Log($"PlayerController - PlayerPlayerIsGrounded : {PlayerPlayerIsGrounded}");
        return playerIsGrounded;
    }

    private void UpdateAnimations()
    {
        if (_animator != null)
        {
            _animator.SetBool("isRunning", _playerIsRunning); //! Remember to check jumping state
            _animator.SetBool("isJumping", _playerIsJumping);
        }
        else
        {
            Debug.Log($"PlayerController: animator not assigned");
        }
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

            // Reset animation states //! Create a method for that
            _playerIsRunning = false;
            _playerIsJumping = false;
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
        _playerIsRunning = false;
        _playerIsJumping = false;
    }
}