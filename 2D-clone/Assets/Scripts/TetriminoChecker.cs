using UnityEngine;

public class TetriminoChecker : MonoBehaviour
{
    public bool tetriminoHasCollided;
    
    private void OnTriggerEnter(Collider p_other)
    {
        if (p_other.gameObject.CompareTag("Tetrimino"))
        {
            Debug.Log($"TetriminoChecker: {gameObject} has collided with {p_other.name}");
            tetriminoHasCollided = true;
        }
    }
}
