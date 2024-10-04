using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private Transform _groundContactPoint;
    [SerializeField] private float _fallThreshold = -20f;
    [SerializeField] private Transform _startPosition; 
    [SerializeField] private LayerMask _ground;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to the Player 
    }

    private void Update()
    {
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
        }
    }

    // Move the player based on WASD and arrows input
    private void MovePlayer()
    {
        float l_horizontalInput = Input.GetAxis("Horizontal"); 
        float l_verticalInput = Input.GetAxisRaw("Vertical"); 

        _rigidbody.velocity = new Vector3(l_horizontalInput * _moveSpeed, _rigidbody.velocity.y, l_verticalInput * _moveSpeed);
    }

    // Make jump the player when the space button is pressed
    private void Jump()
    {
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _jumpForce, _rigidbody.velocity.z);
    }

    // Check if the player is grounded
    private bool IsGrounded()
    {
        Debug.Log($"PlayerController - IsGrounded : { Physics.CheckSphere(_groundContactPoint.position, 0.1f, _ground)}");
        return Physics.CheckSphere(_groundContactPoint.position, 0.1f, _ground);
    }
}
