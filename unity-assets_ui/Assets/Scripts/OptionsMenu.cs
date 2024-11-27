using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Button _applyButton;
    [SerializeField] private Toggle _invertYButton;

    private void Start()
    {
        bool isYaxisInverted = PlayerPrefs.GetInt("Y-axisIsInverted", 0) == 1;
        Debug.Log($"Y-axis is inverted: {isYaxisInverted}");

        if (_invertYButton != null)
            _invertYButton.isOn = isYaxisInverted;

        Debug.Log($"Invert button value: {isYaxisInverted}");
    }
}
