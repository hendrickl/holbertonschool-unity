using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public FloorChecker FloorChecker;
    public TetriminoChecker TetriminoChecker;

    [SerializeField] private GameObject[] _tetriminos;
    [SerializeField] private Transform _spawnPoint;

    private void Start()
    {
        Debug.Log($"SpawnerManager: The tetrimino is landed : {FloorChecker.tetriminoIsLanded}");

        SpawnTetrimino();
    }

    private void OnTriggerEnter(Collider p_other)
    {
        if (p_other.gameObject == FloorChecker.currentTetrimino)
        {
            Debug.Log($"SpawnerManager/OnTriggerEnter: {gameObject} has collided with {p_other.name}");
            SpawnTetriminoWhenLanded();
        }
    }

    private void SpawnTetrimino()
    {
        if (_tetriminos.Length == 0)
        {
            Debug.Log($"SpawnerManager: No tetriminos are assigned to the array");
            return;
        }

        if (FloorChecker.tetriminoIsLanded || FloorChecker.currentTetrimino == null)
        {
            int randomIndex = Random.Range(0, _tetriminos.Length);
            FloorChecker.currentTetrimino = Instantiate(_tetriminos[randomIndex], _spawnPoint.position, Quaternion.identity);
            
            Debug.Log($"SpawnerManager/SpawnTetrimino : The current tetrimino is {FloorChecker.currentTetrimino}");
        }
    }

    private void SpawnTetriminoWhenLanded()
    {
        FloorChecker.currentTetrimino = null;
        Debug.Log($"SpawnerManager/SpawnTetriminoWhenLanded : The current tetrimino is {FloorChecker.currentTetrimino}");

        SpawnTetrimino();
        FloorChecker.tetriminoIsLanded = false;
    }
}
