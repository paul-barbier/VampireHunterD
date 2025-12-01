using UnityEngine;
using UnityEngine.UI;

public class SliderVolume : MonoBehaviour
{
    [SerializeField] private Slider slider;

    void Start()
    {
        // Charger la valeur existante
        slider.value = DontDestroy.Instance.sliderValue;

        // Écouter le changement
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        // Mettre à jour la valeur persistante
        DontDestroy.Instance.sliderValue = value;
    }
}