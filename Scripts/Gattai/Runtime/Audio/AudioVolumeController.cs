using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Gattai.Runtime.Audio
{
    public class AudioVolumeController : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer = default;
        [SerializeField] private string exposedParameterName = "Master Volume";
        [SerializeField] private Slider slider = default;
        [SerializeField] private Toggle toggle = default;
        [SerializeField] private float multiplier = 30f;
        private bool _disableToggleListener;
        private float _oldSliderValue, _defaultSliderValue;

        private void Awake()
        {
            _defaultSliderValue = slider.value;
            UpdateVolume(_defaultSliderValue);
        }

        private void Start()
        {
            AddListeners();
            LoadPlayerPrefs();
        }

        private void OnDisable()
        {
            if(exposedParameterName != null && slider != null)
            {
                PlayerPrefs.SetFloat(exposedParameterName, slider.value);
            }
        }
    
        private void LoadPlayerPrefs()
        {
            if (exposedParameterName != null && slider != null)
            {
                slider.value = PlayerPrefs.GetFloat(exposedParameterName, slider.value);
            }
        }

        private void AddListeners()
        {
            if (slider != null)
            {
                slider.onValueChanged.AddListener(UpdateVolume);
            }

            if (toggle != null)
            {
                toggle.onValueChanged.AddListener(EnableVolume);
            }
        }

        private void UpdateVolume(float volume)
        {
            audioMixer.SetFloat(exposedParameterName, Mathf.Log10(volume) * multiplier);
            _disableToggleListener = true;
            toggle.isOn = slider.value > slider.minValue;
            _disableToggleListener = false;
        }

        private void EnableVolume(bool enable)
        {
            if (_disableToggleListener) { return; }
            _oldSliderValue = enable ? _oldSliderValue : slider.value;
            _oldSliderValue = Mathf.Approximately(_oldSliderValue, slider.minValue) ? slider.maxValue : _oldSliderValue;
            slider.value = enable ? _oldSliderValue : slider.minValue;
        }

        public void ResetValues()
        {
            slider.value = _defaultSliderValue;
            _disableToggleListener = true;
            toggle.isOn = true;
            _disableToggleListener = false;
        }
    }
}
