//using UnityEngine;
//using UnityEngine.UI;

//public class MusicVolumeSlider : MonoBehaviour
//{
//    [SerializeField] private Slider slider;

//    private void Start()
//    {
//        if (slider == null)
//            slider = GetComponent<Slider>();

//        slider.value = MusicManager.instance != null ? MusicManager.instance.Volume : 1f;
//        slider.onValueChanged.AddListener(OnSliderValueChanged);
//    }

//    private void OnSliderValueChanged(float value)
//    {
//        if (MusicManager.instance != null)
//            MusicManager.instance.Volume = value;
//    }
//}
