using UnityEngine;

public class TetriminoMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;

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