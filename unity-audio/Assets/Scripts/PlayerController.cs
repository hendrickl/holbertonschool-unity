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

    private bool _playerHasPermissionToMove;
    private bool _playerHasLandedOnGround;
    private bool _playerWasGroundedLastFrame;

    private bool _playerIsRunning;
    private bool _playerIsJumping;
    private bool _playerIsFalling;

    private bool _shouldTriggerFallingFlatAnimation; // Flag to track if we should trigger the animation
    private bool _playerIsInGettingUpSequence; // Flag to track when we're in the getting up sequence
    private bool _playerIsInFlattingFlatSequence; // Flag to track when we're in the flatting flat sequence

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        transform.position = _startPosition;

        _playerHasPermissionToMove = false;
        _playerHasLandedOnGround = false;
        _playerWasGroundedLastFrame = false;

        _playerIsRunning = false;
        _playerIsJumping = false;
        _playerIsFalling = false;

        _shouldTriggerFallingFlatAnimation = false;
        _playerIsInGettingUpSequence = false;
        _playerIsInFlattingFlatSequence = false;

        Debug.Log($"PlayerController/START - Player is falling = {PlayerIsFalling()}");
        Debug.Log($"PlayerController/START - Player is falling flat = {PlayerIsFallingFlat()}");

        _animator.SetBool("isGettingUpComplete", true);
    }

    private void Update()
    {
        // Check if the player touched the ground for the first time
        if (PlayerIsGrounded() && !_playerWasGroundedLastFrame)
        {
            _playerHasLandedOnGround = true;
            PlayLandingSFX();

            // Only give permission to move if we're not in the getting up & flatting flat sequences
            if (!_playerIsInGettingUpSequence && !_playerIsInFlattingFlatSequence)
            {
                _playerHasPermissionToMove = true;
            }

            // Trigger falling flat
            if (PlayerIsFalling() && !_playerIsInGettingUpSequence)
            {
                _shouldTriggerFallingFlatAnimation = true;
                StartCoroutine(TriggerFallingFlatAnimation());
            }
        }

        // Detect landing from jump
        if (PlayerIsJumping() && !_playerWasGroundedLastFrame && PlayerIsGrounded())
        {
            _playerIsJumping = false;
        }

        _playerWasGroundedLastFrame = PlayerIsGrounded(); // Remember grounded state for the next frame

        if (Input.GetKeyDown(KeyCode.Space) && PlayerIsGrounded() && _playerHasLandedOnGround && !_playerIsInGettingUpSequence)
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
        if (_playerHasPermissionToMove && !_playerIsInGettingUpSequence && !_playerIsInFlattingFlatSequence)
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
        if (_playerHasPermissionToMove && !_playerIsInGettingUpSequence && !_playerIsInFlattingFlatSequence)
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
            Debug.Log($"PlayerController: CHECKFALL - Player fell below threshold = {transform.position.y}");

            // Set the flag to trigger falling flat animation
            _shouldTriggerFallingFlatAnimation = true;

            ResetPlayerPosition();
            StartCoroutine(TriggerFallingFlatAnimation());
        }
    }

    // Methods to check the player's state
    private bool PlayerIsGrounded()
    {
        bool playerIsGrounded = Physics.CheckSphere(_groundContactPoint.position, 0.1f, _ground);
        Debug.Log($"PlayerController: Player is grounded = {playerIsGrounded}");
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
        // Return the flag that indicates we should play the falling flat animation
        return _shouldTriggerFallingFlatAnimation;
    }

    // Animation management
    private void UpdateAnimations()
    {
        if (_animator != null)
        {
            _animator.SetBool("isRunning", _playerIsRunning && PlayerIsGrounded() && !PlayerIsJumping());
            _animator.SetBool("isJumping", PlayerIsJumping());
            _animator.SetBool("isFalling", PlayerIsFalling());
            _animator.SetBool("isFallingFlat", PlayerIsFallingFlat());
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

    private void ResetPlayerRotation()
    {
        transform.position = _resetRotation;
    }

    private void ResetAnimationsState()
    {
        _playerIsRunning = false;
        _playerIsJumping = false;
        _playerIsFalling = false;
        _shouldTriggerFallingFlatAnimation = false;
    }

    // Activate the Falling Flat animation followed by Getting Up
    private IEnumerator TriggerFallingFlatAnimation()
    {
        if (_animator != null)
        {
            _playerIsInGettingUpSequence = false;
            _playerIsInFlattingFlatSequence = false;
            _playerHasPermissionToMove = false;

            // Debug.Log("PlayerController/TFFA - 1: Start the Falling Flat animation");

            // Step 1: Start the Falling Flat animation
            _animator.SetBool("isFallingFlat", true);
            _playerIsInFlattingFlatSequence = true;
            _animator.SetBool("isGettingUpComplete", false); // Reset this to ensure proper transition
            _animator.SetBool("isRunning", false);

            // Wait for the Falling Flat Impact animation
            yield return new WaitForSeconds(2.0f);

            // Debug.Log("PlayerControlle/TFFA - 2: Falling flat animation complete -> Transitioning to Getting Up");

            // Step 2: Transition to Getting Up happens automatically in the Animator, wait for its duration
            _playerIsInFlattingFlatSequence = false;
            _playerIsInGettingUpSequence = true;
            yield return new WaitForSeconds(6f); // Adjust based on the Getting Up animation duration

            // Debug.Log("PlayerControlle/TFFA - 3A: Getting Up animation should be complete -> Transitioning to Idle");

            // Step 3: Transitioning to Idle
            _animator.SetBool("isGettingUpComplete", true); //Set the flag to transition to Idle

            // Debug.Log("PlayerControlle/TFFA - 3B: Transitioning to Idle complete");

            // Reset animation flags
            _shouldTriggerFallingFlatAnimation = false;
            _animator.SetBool("isFallingFlat", false);

            // Reset other animation states
            ResetAnimationsState();

            // End the getting up sequence and allow player to move again
            _playerIsInGettingUpSequence = false;
            _playerHasPermissionToMove = true;
        }
        else
        {
            Debug.Log("PlayerController: Animator not assigned");
            yield return null;
        }
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