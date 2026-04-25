using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameHUG;
    [SerializeField] private TextMeshProUGUI _correctVasesText;
    [SerializeField] private TextMeshProUGUI _swapsText;
    [SerializeField] private TextMeshProUGUI _timeText;

    [SerializeField] private GameObject _finalPanel;
    [SerializeField] private TextMeshProUGUI _FinalTimeText;
    [SerializeField] private TextMeshProUGUI _finalSwapsText;
    [SerializeField] private Button _restartButton;

    private GameManager _gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameManager = FindFirstObjectByType<GameManager>();
        _correctVasesText.text = "0";
        _swapsText.text = "0";
        _timeText.text = "0:00";
        _FinalTimeText.text = "0:00";
        _finalSwapsText.text = "0";
        _finalPanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.isGameOver)
            _gameHUG.SetActive(false);


    }
    public void UpdateCorrectVases(int CorrectVases)
    {
        _correctVasesText.text = $"{CorrectVases}";
    }
    public void UpdateSwapsText(int swapCount)
    {
        _swapsText.text = $"{swapCount}";
    }
    public void UpdateTimeText(string time)
    {
        _timeText.text = $"{time}";
    }
    public void ShowFinalPanel(string finalTime, int swapCount)
    {
        _FinalTimeText.text = $"Time: {finalTime}";
        _finalSwapsText.text = $"Swaps: {swapCount}";
        _finalPanel.SetActive(true);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}