using UnityEngine;

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

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); 
        _startPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Jump();
        }

        MovePlayer();
        CheckFall();
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
        Debug.Log($"Jump is called");
    }

    // Check if the player is grounded
    private bool IsGrounded()
    {
        Debug.Log($"PlayerController - IsGrounded : { Physics.CheckSphere(_groundContactPoint.position, 0.1f, _ground)}");
        return Physics.CheckSphere(_groundContactPoint.position, 0.1f, _ground);
    }

    private void CheckFall()
    {
        if (transform.position.y < _fallThreshold)
        {
            ResetPlayerPosition();
        }
    }

    private void ResetPlayerPosition()
    {
        transform.position = _resetPosition;
    }
}
