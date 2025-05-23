using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 300f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _fallThreshold = -20f;
    [SerializeField] private Transform _groundContactPoint;
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Vector3 _resetPosition;
    [SerializeField] private Vector3 _resetRotation;
    [SerializeField] private LayerMask _ground;
    // [SerializeField] private Animator _animator;

    [SerializeField] private AudioSource _footstepsAudioSource;
    [SerializeField] private AudioSource _landingAudioSource;
    [SerializeField] private AudioClip _grassFootsteps;
    [SerializeField] private AudioClip _rockFootsteps;
    [SerializeField] private AudioClip _footstepsLandingGrass;
    [SerializeField] private AudioClip _footstepsLandingRock;
    [SerializeField] private string _groundTagGrass = "Grass";
    [SerializeField] private string _groundTagRock = "Rock";
    [SerializeField] private LayerMask _terrainLayerMask;

    private Rigidbody _rigidbody;

    private bool _playerHasPermissionToMove;
    private bool _playerHasLandedOnGround;
    private bool _playerWasGroundedLastFrame;

    private bool _playerIsRunning;
    private bool _playerIsJumping;
    private bool _playerIsFalling;
    private bool _playerIsFallingFlat;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        transform.position = _startPosition;

        _playerHasPermissionToMove = true;
        _playerHasLandedOnGround = false;
        _playerWasGroundedLastFrame = false;

        _playerIsRunning = false;
        _playerIsJumping = false;
        _playerIsFalling = false;
        _playerIsFallingFlat = false;
    }

    private void Update()
    {
        // Check if the player touched the ground for the first time
        if (PlayerIsGrounded() && !_playerWasGroundedLastFrame)
        {
            _playerHasLandedOnGround = true;
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
    }

    // Move the player based on WASD and arrows input
    private void MovePlayer()
    {
        if (_playerHasPermissionToMove)
        {
            float verticalInput = Input.GetAxisRaw("Vertical");

            // Only move and set running if there's actual input
            if (verticalInput != 0)
            {
                // Use the player's facing direction for movement
                Vector3 movementOnZ = transform.forward * verticalInput;
                Vector3 moveDirection = movementOnZ;
                moveDirection.Normalize();

                _rigidbody.velocity = new Vector3(moveDirection.x * _moveSpeed, _rigidbody.velocity.y, moveDirection.z * _moveSpeed);

                _playerIsRunning = true;
            }
            else
            {
                // If no vertical input, stop running but maintain Y velocity
                _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
                _playerIsRunning = false;
            }
        }
        else
        {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0); // The player can fall under the effect of gravity but cannot move horizontally
            _playerIsRunning = false;
        }
        PlayFootstepsSFX();
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

    // Make jump the player
    private void Jump()
    {
        _playerIsJumping = true;
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _jumpForce, _rigidbody.velocity.z);
    }

    private void CheckFall()
    {
        if (transform.position.y < _fallThreshold)
        {
            ResetPlayerPosition();
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
        return _playerIsFallingFlat;
    }

    // Methods to reset 
    private void ResetPlayerPosition()
    {
        transform.position = _resetPosition;
    }

    private void ResetPlayerRotation()
    {
        transform.position = _resetRotation;
    }
    
    // Method to detect the tag of the 2 differents ground
    private string DetectGroundTag()
    {
        RaycastHit hit;

        if (Physics.Raycast(_groundContactPoint.position, Vector3.down, out hit, 1f, _terrainLayerMask))
        {
            Debug.Log($"PlayerController - Ground tag detected is {hit.collider.tag}");
            return hit.collider.tag;
        }
        return "";
    }

    // Method to trigger footstep sound effects based on floor type
    private void PlayFootstepsSFX()
    {
        if (_playerIsRunning && PlayerIsGrounded() && !PlayerIsJumping())
        {
            string groundTag = DetectGroundTag();
            AudioClip footstepsClip = null;

            if (groundTag == _groundTagGrass)
            {
                footstepsClip = _grassFootsteps;
            }
            else if (groundTag == _groundTagRock)
            {
                footstepsClip = _rockFootsteps;
            }

            if (footstepsClip != null)
            {
                if (_footstepsAudioSource.clip != footstepsClip)
                {
                    _footstepsAudioSource.clip = footstepsClip;
                    _footstepsAudioSource.Play();
                }
                else if (!_footstepsAudioSource.isPlaying)
                {
                    // If this is already the correct clip but it is not yet played
                    _footstepsAudioSource.Play();
                }
            }
        }
        else
        {
            if (_footstepsAudioSource.isPlaying)
            {
                _footstepsAudioSource.Stop();
            }
        }
    }

    // Method to trigger landing sound effects based on floor type
    private void PlayLandingSFX()
    {
        string groundTag = DetectGroundTag();
        AudioClip landingClip = null;

        if (groundTag == _groundTagGrass)
        {
            landingClip = _footstepsLandingGrass;
        }
        else if (groundTag == _groundTagRock)
        {
            landingClip = _footstepsLandingRock;
        }

        if (landingClip != null)
        {
            _landingAudioSource.clip = landingClip;
            _landingAudioSource.Play();
            Debug.Log($"PlayerController - Landing SFX trigger");
        }
    }
}