using Gattai.Runtime.Pooling;
using UnityEngine;

namespace Gattai.Runtime.Audio
{
    public class SfxPlayer : MonoBehaviour
    {
        [SerializeField] private PooledMonoBehaviour sfxAudioSourcePrefab;
        private static SfxPlayer _instance;

        private void Awake()
        {
            _instance = this;
        }

        public static void Play(SfxAudioEvent sfxAudioEvent)
        {
            if (sfxAudioEvent == null) return;
            
            var audioSource = _instance.sfxAudioSourcePrefab.GetFromPool<SfxAudioSource>();
            
            if (audioSource == null) return;
            
            sfxAudioEvent.Play(audioSource.AudioSource);
        }
    }
}
