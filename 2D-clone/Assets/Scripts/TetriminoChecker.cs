using UnityEngine;

public class TetriminoChecker : MonoBehaviour
{
    public TetriminoMovement TetriminoMovement;

    public bool canMove;
    
    private void OnTriggerEnter(Collider p_other)
    {
        if (p_other.gameObject.CompareTag("Tetrimino"))
        {
            Debug.Log($"TetriminoChecker: {gameObject} is collisioned with {p_other.name}");
            canMove = false;
        }
    }
}
