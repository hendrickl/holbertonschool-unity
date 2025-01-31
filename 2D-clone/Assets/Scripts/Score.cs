using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private Canvas _scoreCanvas;

    [SerializeField] private TMP_Text _scoreText;
    
    private int _scoreValue;


    private void Start()
    {
        if (_scoreCanvas != null)
            _scoreCanvas.gameObject.SetActive(true);

        _scoreValue = 0;
        _scoreText.text = _scoreValue.ToString();
    }

    private void ComputeScore()
    {
        _scoreText.text = _scoreValue.ToString();
    }
}
