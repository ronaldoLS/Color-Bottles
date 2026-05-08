using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }

    public int bestScore = 0;
    public float score = 0;

    [SerializeField] private bool _isMuted;
    public bool isMuted { get { return _isMuted; } private set { _isMuted = value; } }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _isMuted = false;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToggleMute()
    {
        _isMuted = !_isMuted;
    }
    public void SetMute(bool mute)
    {
        _isMuted = mute;
    }
}
