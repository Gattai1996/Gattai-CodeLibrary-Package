using UnityEngine;

[CreateAssetMenu(menuName = "Sfx Audio Event", fileName = "New Sfx Audio Event")]
public class SfxAudioEvent : AudioEvent
{
    public AudioClip[] audioClips;
    public RangedFloat volume;
    public RangedFloat pitch;
    
    public override void Play(AudioSource audioSource)
    {
        if (audioClips.Length == 0) return;

        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.volume = Random.Range(volume.minValue, volume.maxValue);
        audioSource.pitch = Random.Range(pitch.minValue, pitch.maxValue);
        audioSource.Play();
    }
}