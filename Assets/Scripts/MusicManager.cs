using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource _Music;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _Music = GetComponent<AudioSource>();
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

    }
}
