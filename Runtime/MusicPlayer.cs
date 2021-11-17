using UnityEngine;

/// <summary>
/// Play continously a array of musics in order or randomly.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;
    public AudioClip[] playlist = default;
    [SerializeField] private bool _randomizeFirstMusic = default;
    [SerializeField] private bool _randomizeOrder = default;
    private AudioSource _audioSource;
    private int _currentMusicIndex, _lastMusicIndex;
    private bool _pause;
    private int RandomMusicIndex => Random.Range(0, playlist.Length);

    private void Start()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = false;
        _audioSource.volume = 0.5f;

        if (_randomizeFirstMusic)
        {
            _currentMusicIndex = RandomMusicIndex;
        }
        else
        {
            _audioSource.PlayOneShot(playlist[0]);
        }
    }

    private void Update()
    {
        if (_pause || _audioSource.isPlaying) return;

        if (_randomizeOrder)
        {
            PlayRandom();
        }
        else
        {
            PlayNext();
        }
    }

    /// <summary>
    /// Play a random and non repeated music on the array of AudioClips.
    /// </summary>
    public void PlayRandom()
    {
        _currentMusicIndex = RandomMusicIndex;

        if (_currentMusicIndex == _lastMusicIndex)
        {
            _currentMusicIndex++;
        }

        _audioSource.PlayOneShot(playlist[_currentMusicIndex]);
        _lastMusicIndex = _currentMusicIndex;
    }

    /// <summary>
    /// Play the next music on the array of AudioClips.
    /// </summary>
    public void PlayNext()
    {
        _audioSource.Stop();
        _currentMusicIndex += 1;
        _currentMusicIndex %= playlist.Length;
        _audioSource.PlayOneShot(playlist[_currentMusicIndex]);
    }

    /// <summary>
    /// Pause the music.
    /// </summary>
    public void Pause()
    {
        _pause = true;
        _audioSource.Pause();
    }

    /// <summary>
    /// Removes the pause state.
    /// </summary>
    public void Play()
    {
        _pause = false;
    }
}