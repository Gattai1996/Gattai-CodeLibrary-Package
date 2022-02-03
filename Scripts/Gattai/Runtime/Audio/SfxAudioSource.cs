using System.Collections;
using Gattai.Runtime.Pooling;
using UnityEngine;

namespace Gattai.Runtime.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SfxAudioSource : PooledMonoBehaviour
    {
        public AudioSource AudioSource { get; private set; }

        private void Awake()
        {
            AudioSource = GetComponent<AudioSource>();
        }
    
        private void OnEnable()
        {
            StartCoroutine(DeactivateOnStopAudioClipCoroutine());
        }
        
        private IEnumerator DeactivateOnStopAudioClipCoroutine()
        {
            yield return new WaitForEndOfFrame();
            
            if (AudioSource.clip == null) gameObject.SetActive(false);

            while (AudioSource.isPlaying)
            {
                yield return null;
            }
            
            gameObject.SetActive(false);
        }
    }
}
