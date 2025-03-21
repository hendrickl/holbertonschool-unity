using UnityEngine;

public class TetriminoMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;

    private float _currentYPosition;
    private float _previousYPosition;

    private void Start()
    {
        _currentYPosition = gameObject.transform.position.y;
        _previousYPosition = gameObject.transform.position.y;

        Debug.Log($"TetriminoMovement: current position = {gameObject.transform.position.y}");
    }

    private void FixedUpdate() 
    {
        MoveDown();    
    }

    private void MoveDown()
    {
        _movementSpeed = 2f;
        gameObject.transform.position += Vector3.down * _movementSpeed * Time.deltaTime;
    }   

    private void StopMoveDown()
    {
        _movementSpeed = 0f;
    } 
}