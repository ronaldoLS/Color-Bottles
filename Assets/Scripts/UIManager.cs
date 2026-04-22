using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _correctVasesText;
    [SerializeField] private TextMeshProUGUI _swapsText;
    [SerializeField] private TextMeshProUGUI _timeText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _correctVasesText.text = "0";
        _swapsText.text = "0";
        _timeText.text = "00:00";
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void UpdateCorrectVases(int CorrectVases)
    {
        _correctVasesText.text = CorrectVases.ToString();
    }
    public void UpdateSwapsText(int swapCount)
    {
        _swapsText.text = swapCount.ToString();
    }
    public void UpdateTimeText(string time)
    {
        _timeText.text = time;
    }
}