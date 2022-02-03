using UnityEngine;
using UnityEngine.UI;

namespace Gattai.Runtime.Audio
{
    [RequireComponent(typeof(AudioSource), typeof(Button))]
    public class ButtonSfxPlayer : MonoBehaviour
    {
        [SerializeField] private SfxAudioEvent sfxAudioEvent;
        private AudioSource _audioSource;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _audioSource = GetComponent<AudioSource>();
            _button.onClick.AddListener(() =>
            {
                sfxAudioEvent.Play(_audioSource);
            });
        }
    }
}