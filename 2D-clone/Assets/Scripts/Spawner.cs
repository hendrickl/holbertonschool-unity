using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _tetriminos;
    [SerializeField] private Transform _spawnPoint;

    private GameObject _currentTetrimino;
    private bool _tetriminoIsLanded;


    private void Start()
    {
        Debug.Log($"The tetrimino is landed : {_tetriminoIsLanded}");

        SpawnTetrimino();
        StartCoroutine("SpawnTetriminoAfterDelayCoroutine");
    }

    private void OnTriggerEnter(Collider p_other)
    {
        if (p_other.gameObject == _currentTetrimino)
        {
            Debug.Log($"Spawner - OnTriggerEnter: other is {p_other.name}");
            _tetriminoIsLanded = true;
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

        // if (_currentTetrimino.isLanded || _currentTetrimino.hasCollised)

        if (_tetriminoIsLanded || _currentTetrimino == null)
        {
            int randomIndex = Random.Range(0, _tetriminos.Length);
            _currentTetrimino = Instantiate(_tetriminos[randomIndex], _spawnPoint.position, Quaternion.identity);
            
            Debug.Log($"Spawner - SpawnTetrimino : The current tetrimino is {_currentTetrimino}");
        }
    }

    private void SpawnTetriminoWhenLanded()
    {
        _currentTetrimino = null;
        Debug.Log($"Spawner - SpawnTetriminoWhenLanded : The current tetrimino is {_currentTetrimino}");

        SpawnTetrimino();
        _tetriminoIsLanded = false;
    }
}
