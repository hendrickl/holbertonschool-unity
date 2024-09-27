using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to the Player 
    }

    private void Update()
    {
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    // Move the player based on WASD and arrows input
    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); // Get the value of the horizontal axis
        float verticalInput = Input.GetAxisRaw("Vertical"); // Get the value of the vertical axis

        _rigidbody.velocity = new Vector3(horizontalInput * moveSpeed, _rigidbody.velocity.y, verticalInput * moveSpeed);
    }

    // Make jump the player when the space button is pressed
    private void Jump()
    {
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpForce, _rigidbody.velocity.z);
    }
}
