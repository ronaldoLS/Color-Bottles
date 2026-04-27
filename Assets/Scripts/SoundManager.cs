using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] _swapClip;
    [SerializeField] private AudioClip[] _selectClip;

    private AudioSource _audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlaySwapSound()
    {
        int randomIndex = Random.Range(0, _swapClip.Length);
        _audioSource.volume = 1f;
        _audioSource.PlayOneShot(_swapClip[randomIndex]);
    }
    public void PlaySelectSound()
    {
        int randomIndex = Random.Range(0, _selectClip.Length);
        _audioSource.volume = 0.25f;
        _audioSource.PlayOneShot(_selectClip[randomIndex]);
    }
}
