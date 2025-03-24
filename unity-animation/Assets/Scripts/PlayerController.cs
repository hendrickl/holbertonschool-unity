using UnityEngine;

// This script manages the behaviour of the player
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private Transform _groundContactPoint;
    [SerializeField] private float _fallThreshold = -20f;
    [SerializeField] private Vector3 _startPosition; 
    [SerializeField] private Vector3 _resetPosition; 
    [SerializeField] private LayerMask _ground;

    private Rigidbody _rigidbody;
    private bool _playerHasPermissionToMove;
    private bool _hasLandedOnGround;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); 
        transform.position = _startPosition;
        _playerHasPermissionToMove = false;
        _hasLandedOnGround = false;
    }

    private void Update()
    {
        // We check if the player touched the ground for the first time
        if (!_hasLandedOnGround && IsGrounded())
        {
            _hasLandedOnGround = true;
            _playerHasPermissionToMove = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && _hasLandedOnGround)
        {
            Jump();
        }

        MovePlayer();
        CheckFall();
    }

    // Move the player based on WASD and arrows input
    private void MovePlayer()
    {
        if (_playerHasPermissionToMove)
        {
            float l_horizontalInput = Input.GetAxis("Horizontal"); 
            float l_verticalInput = Input.GetAxisRaw("Vertical"); 

            _rigidbody.velocity = new Vector3(l_horizontalInput * _moveSpeed, _rigidbody.velocity.y, l_verticalInput * _moveSpeed);
        }
        else 
        {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0); // The player can fall under the effect of gravity but cannot move horizontally
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
        Debug.Log($"PlayerController - IsGrounded : {isGrounded}");
        return isGrounded;
    }

    private void CheckFall()
    {
        if (transform.position.y < _fallThreshold)
        {
            ResetPlayerPosition();

            // If the player falls the control state is reset
            if (_hasLandedOnGround) 
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
        _hasLandedOnGround = false;
        _playerHasPermissionToMove = false;
    }
}