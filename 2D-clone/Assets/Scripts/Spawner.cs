using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _tetriminos;
    [SerializeField] private Transform _spawnPoint;
    // [SerializeField] private GameObject _floor;

    private GameObject _currentTetrimino;

    private void Start()
    {
        // SpawnTetriminos();
        StartCoroutine("SpawnTetriminoAfterDelayCoroutine");
    }

    private void OnTriggerEnter(Collider p_other)
    {
        if (p_other.gameObject == _currentTetrimino)
        {
            Debug.Log($"Spawner - OnTriggerEnter: other is {p_other}");
            SpawnTetriminoWhenLanded();
        }
    }

    private IEnumerator SpawnTetriminoAfterDelayCoroutine()
    {
        yield return new WaitForSeconds(2f);
        SpawnTetrimino();
    }

    private void SpawnTetrimino()
    {
        if (_tetriminos.Length == 0)
        {
            Debug.Log($"No tetriminos are assigned to the array");
            return;
        }

        if (_currentTetrimino == null)
        {
            int randomIndex = Random.Range(0, _tetriminos.Length);
            _currentTetrimino = Instantiate(_tetriminos[randomIndex], _spawnPoint.position, Quaternion.identity);
            
            Debug.Log($"Spawner - SpawnTetrimino: current tetrimino is {_currentTetrimino}");
        }
    }

    private void SpawnTetriminoWhenLanded()
    {
        _currentTetrimino = null;
        Debug.Log($"Spawner - SpawnTetriminoWhenLanded: current tetrimino is {_currentTetrimino}");

        SpawnTetrimino();
    }
}
