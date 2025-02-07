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
        StartCoroutine("SpawnTetriminoAfterDelay");
    }

    private void Update()
    {
        SpawnTetriminoAfterDelay();
    }

    private void OnTriggerEnter(Collider p_other)
    {
        if (p_other.gameObject == _currentTetrimino)
        {
            Debug.Log($"Spawner - OnTriggerEnter: other is {p_other}");
            OnTetriminoLanded();
        }
    }

    private IEnumerator SpawnTetriminoAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        SpawnTetriminos();
    }

    private void SpawnTetriminos()
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
            
            Debug.Log($"Spawner - SpawnTetriminos: current tetrimino is {_currentTetrimino}");
        }
    }

    private void OnTetriminoLanded()
    {
        _currentTetrimino = null;

        Debug.Log($"Spawner - OnTetriminoLanded: current tetrimino is {_currentTetrimino}");
        SpawnTetriminos();
    }
}
