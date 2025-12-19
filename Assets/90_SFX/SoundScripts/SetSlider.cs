using UnityEngine;
using UnityEngine.Audio;

public class SetSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string parameterName = "Null";

    // Start is called before the first frame update
    void Start()
    {
        audioMixer.GetFloat(parameterName, out float volume);
        gameObject.GetComponent<UnityEngine.UI.Slider>().value = Mathf.Pow(10, volume / 20);
    }
}
