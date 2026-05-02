using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField] private Button _buttonStart;
    [SerializeField] private Button _buttonMusic;
    [SerializeField] private Sprite _onMusicSprite;
    [SerializeField] private Sprite _offMusicSprite;

    private MusicManager _musicManager;
    private AudioSource _music;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _musicManager = FindFirstObjectByType<MusicManager>();
        if (_musicManager != null)
        {
            _music = _musicManager.GetComponent<AudioSource>();
        }

        _buttonMusic.GetComponent<Image>().sprite = _music != null && _music.isPlaying ? _onMusicSprite : _offMusicSprite;
    }

    // Update is called once per frame
    void Update()
    {

        UpdateMusicButton();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Main");
    }
    public void UpdateMusicButton()
    {
        if (_music != null)
        {
            _buttonMusic.GetComponent<Image>().sprite = _music.isPlaying ? _onMusicSprite : _offMusicSprite;
        }
    }
}
