using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource _Music;
    public bool _isMuted;
    private MainManager _mainManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        _Music = GetComponent<AudioSource>();
        _mainManager = FindAnyObjectByType<MainManager>();

        if (_mainManager != null)
            _isMuted = _mainManager.isMuted;
        else
            _isMuted = false;

        _Music.Play();

        if (_isMuted)
        {
            _Music.Pause();
        }
        else
        {
            _Music.UnPause();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void MusicSwitch()
    {
        if (_Music.isPlaying)
        {
            _Music.Pause();
        }
        else
        {
            _Music.UnPause();
        }
        _mainManager.ToggleMute();
    }
}
