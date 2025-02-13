using UnityEngine;

public class FloorChecker : MonoBehaviour
{
    public GameObject currentTetrimino;
    
    public bool tetriminoIsLanded;

    private void OnTriggerEnter(Collider p_other)
    {
        if (p_other.gameObject == currentTetrimino)
        {
            Debug.Log($"FloorChecker/OnTriggerEnter: {gameObject} has collided with {p_other.name}");
            tetriminoIsLanded = true;
        }
    }
}
