using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _tetriminos;
    [SerializeField] private Transform _spawnPoint;

    private void Start()
    {
        SpawnTetriminos();
    }

    private void SpawnTetriminos()
    {
        if (_tetriminos.Length == 0)
        {
            Debug.Log($"No tetriminos are assigned to the array");
            return;
        }

        int randomIndex = Random.Range(0, _tetriminos.Length);
        Instantiate(_tetriminos[randomIndex], _spawnPoint.position, Quaternion.identity);
    }
}
