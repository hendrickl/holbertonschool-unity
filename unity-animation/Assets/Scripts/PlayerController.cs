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
    private bool _playerIsFalling;
    private bool _playerIsFallingFlat;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); 

        transform.position = _startPosition;
        
        _playerHasPermissionToMove = false;
        _playerHasLandedOnGround = false;
        _playerWasGroundedLastFrame = false; 

        _playerIsRunning = false;
        _playerIsJumping = false;

        Debug.Log($"PlayerController: START - Player is falling = {PlayerIsFalling()}");
        Debug.Log($"PlayerController: START - Player is falling flat = {PlayerIsFallingFlat()}");

        _playerIsFalling = false;
        _playerIsFallingFlat = false;
    }

    private void Update()
    {
        // Check if the player touched the ground for the first time
        if (PlayerIsGrounded() && !_playerWasGroundedLastFrame)
        {
            _playerHasLandedOnGround = true;
            _playerHasPermissionToMove = true;
        }

        // Detect landing from jump
        if (PlayerIsJumping() && !_playerWasGroundedLastFrame && PlayerIsGrounded())
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
        PlayerIsJumping();
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _jumpForce, _rigidbody.velocity.z);
    }

    private void CheckFall()
    {
        if (transform.position.y < _fallThreshold)
        {
            PlayerIsFalling();

            //! Coroutines pour déclencher l'animation Falling Flat

            Debug.Log($"PlayerController: CHECKFALL 1 - Player is falling = {PlayerIsFalling()}");
            Debug.Log($"PlayerController: CHECKFALL 1 - Player is falling flat = {PlayerIsFallingFlat()}");

            ResetPlayerPosition();
            ResetAnimationsState();

            Debug.Log($"PlayerController: CHECKFALL 2 - Player is falling = {PlayerIsFalling()}");
            Debug.Log($"PlayerController: CHECKFALL 2 - Player is falling flat = {PlayerIsFallingFlat()}");
        }
    }

    // Methods to check the player's state
    private bool PlayerIsGrounded()
    {
        bool playerIsGrounded = Physics.CheckSphere(_groundContactPoint.position, 0.1f, _ground);
        return playerIsGrounded;
    }

    private bool PlayerIsJumping()
    {
        _playerIsJumping = _rigidbody.velocity.y > 1f;
        return _playerIsJumping;
    }

    private bool PlayerIsFalling()
    {
        _playerIsFalling = !PlayerIsGrounded() && _rigidbody.velocity.y < -1f;
        return _playerIsFalling;    
    }

    private bool PlayerIsFallingFlat()
    {
        _playerIsFallingFlat = PlayerIsGrounded() && _playerWasGroundedLastFrame && _rigidbody.velocity.y < 0.5f && _rigidbody.velocity.y > -0.5f;
        return _playerIsFallingFlat;
    }

    // Animation management
    private void UpdateAnimations()
    {
        if (_animator != null)
        {
            _animator.SetBool("isRunning", _playerIsRunning && !PlayerIsJumping() && ! PlayerIsFalling()); 
            _animator.SetBool("isJumping", PlayerIsJumping()); 
            _animator.SetBool("isFalling", PlayerIsFalling());      
        }
        else
        {
            Debug.Log($"PlayerController: animator not assigned");
        }
    }

    // Methods to reset 
    private void ResetPlayerPosition()
    {
        transform.position = _resetPosition;
    }

    private void ResetAnimationsState()
    {
        _playerIsRunning = false;
        _playerIsJumping = false;
        // _playerIsFalling = false;
    }
}