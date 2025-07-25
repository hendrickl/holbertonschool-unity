
using UnityEngine;

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
    [SerializeField] private Animator _animator;

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

    private bool _playerHasLandedOnGround;
    private bool _playerWasGroundedLastFrame;
    private bool _playerIsCurrentlyGrounded;

    private bool _playerIsRunning;
    private bool _playerIsJumping;
    private bool _playerIsFalling;
    private bool _playerShouldFallingFlat;

    private float _jumpStartTime = 0f;
    [SerializeField] float _minJumpDuration = 1.9f; // Minimum time before to be able into falling animation

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        if (_animator == null)
        {
            Debug.Log($"PlayerController - Animator not assigned");
        }

        transform.position = _startPosition;

        _playerHasLandedOnGround = false;
        _playerWasGroundedLastFrame = false;
        _playerIsCurrentlyGrounded = false;

        _playerIsRunning = false;
        _playerIsJumping = false;
        _playerIsFalling = false;
        _playerShouldFallingFlat = false;
    }

    private void Update()
    {
        _playerIsCurrentlyGrounded = PlayerIsGrounded();
        _playerIsFalling = PlayerIsFalling();

        // Check if the player touched the ground for the first time
        if (_playerIsCurrentlyGrounded && !_playerWasGroundedLastFrame)
        {
            _playerHasLandedOnGround = true;
            _playerIsFalling = false;
        }

        if (_playerHasLandedOnGround && PlayerHasPermissionToMove())
        {
            MovePlayer();
            RotatePlayer();
        }

        if (Input.GetKeyDown(KeyCode.Space) && _playerIsCurrentlyGrounded && _playerHasLandedOnGround)
        {
            Jump();
        }

        UpdateAnimatorStates();
        CheckFall();

        _playerWasGroundedLastFrame = _playerIsCurrentlyGrounded; //Update previous state
    }

    // Move the player based on WASD and arrows input
    private void MovePlayer()
    {
        if (PlayerHasPermissionToMove())
        {
            float verticalInput = Input.GetAxisRaw("Vertical");

            if (verticalInput != 0)
            {
                // Use the player's facing direction for movement
                Vector3 moveDirection = transform.forward * verticalInput;
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
            PlayFootstepsSFX();
        }
    }

    // Rotate player gradually based on horizontal input 
    private void RotatePlayer()
    {
        if (PlayerHasPermissionToMove())
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
        _jumpStartTime = Time.time;
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
    private bool PlayerHasPermissionToMove()
    {
        AnimatorStateInfo currentState = _animator.GetCurrentAnimatorStateInfo(0);
        bool isInRestrictedAnimation = currentState.IsName("Falling") || currentState.IsName("Falling Flat Impact") || currentState.IsName("Getting Up");

        return !isInRestrictedAnimation && !_playerIsFalling;
    }

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
        bool isFalling = _rigidbody.velocity.y < -9f && (Time.time - _jumpStartTime) > _minJumpDuration; // Velocity needs to be lesser than usual due jump action
        return isFalling;
    }

    // Method to manage the animator
    private void UpdateAnimatorStates()
    {
        bool isGrounded = PlayerIsGrounded();
        bool isJumping = PlayerIsJumping();
        bool isFalling = PlayerIsFalling();
        bool isFallingFlat = DetectFallingStateAnimator();

        if (transform.position == _resetPosition && isGrounded)
        {
            _animator.SetBool("isFalling", false);
            _animator.SetBool("isFallingFlat", true);
            _animator.SetBool("isJumping", false);
            _animator.SetBool("isRunning", false);
        }

        if (transform.position == _resetPosition && !isGrounded)
        {
            _animator.SetBool("isFalling", true);
            _animator.SetBool("isFallingFlat", false);
            _animator.SetBool("isJumping", false);
            _animator.SetBool("isRunning", false);
        }

        _animator.SetBool("isJumping", isJumping || !isGrounded);
        _animator.SetBool("isRunning", _playerIsRunning && isGrounded);
        _animator.SetBool("isFalling", isFalling && !isGrounded);
        _animator.SetBool("isFallingFlat", isFallingFlat && isGrounded);
    }

    // Methods to reset transform properties 
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
            return hit.collider.tag;
        }
        return "";
    }

    // Method to trigger footstep sound effects based on floor type
    private void PlayFootstepsSFX()
    {
        if (_playerIsRunning && !PlayerIsJumping())
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
    public void PlayLandingSFX()
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
            Debug.Log($"PlayerController - Landing SFX {landingClip} is trigger");
        }
    }

    // Detects if the current animation is "Falling" and sets a flag to trigger the falling flat sequence
    private bool DetectFallingStateAnimator()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
        {
            Debug.Log("PlayerController - Falling animation has started");
            _playerShouldFallingFlat = true;
        }
        else
        {
            _playerShouldFallingFlat = false;
        }

        return _playerShouldFallingFlat;
    }
}
