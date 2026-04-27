using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _IndicatorPrefab;
    [SerializeField] private GameObject _bottlePrefab;
    [SerializeField] private float _boundary;
    [SerializeField] public bool isGameOver;

    private Vector3 _indicatorOfset = new(-0.125f, 0.39f, -0.235f);
    private int _bottleNumber;
    private List<GameObject> _bottlesTopShelf = new List<GameObject>();
    private GameObject[] _secretSequence;
    private UIManager _UIManager;
    private SoundManager _soundManager;
    private int _swapCount = 0;
    private TimeSpan _timeSpan;
    private float _elapsedTime = 0f;
    private string _timeFormat = @"m\:ss\:ff";

    [SerializeField] private List<Color> _availableColors;
    [SerializeField] private List<float> _positions = new List<float>();
    [SerializeField] private GameObject _topShelf;
    [SerializeField] private GameObject _bottomShelf;

    public int CorrectVases { get; private set; }

    private void Start()
    {
        isGameOver = false;
        _UIManager = FindFirstObjectByType<UIManager>();
        _soundManager = GameObject.FindFirstObjectByType<SoundManager>();
        _bottleNumber = _availableColors.Count;
        _secretSequence = new GameObject[_bottleNumber];
        _bottomShelf.SetActive(false);

        _elapsedTime = 0f;
        _swapCount = 0;

        GeneratePositions();
        CreateTopShelf();
        PlaceSecretSequence();
        CheckSequence();
    }

    private void Update()
    {
        if (isGameOver)
            return;

        _elapsedTime += Time.deltaTime;
        _timeSpan = TimeSpan.FromSeconds(_elapsedTime);
        _UIManager.UpdateTimeText(_timeSpan.ToString(_timeFormat));
    }

    public void GeneratePositions()
    {
        _positions.Clear();
        float gap = (float)Math.Round(_boundary * 2 / _bottleNumber, 2);
        float centerOffset = ((_bottleNumber - 1) * gap) / 2f;

        for (int i = 0; i < _bottleNumber; i++)
        {
            float pos = (i * gap) - centerOffset;
            _positions.Add((float)Math.Round(pos, 2));
        }
    }

    public void CreateTopShelf()
    {
        _bottlesTopShelf.Clear();

        for (int i = 0; i < _positions.Count; i++)
        {
            Vector3 position = new Vector3(_positions[i], 0.86f, -0.5f);
            GameObject bottleObj = Instantiate(_bottlePrefab, position, Quaternion.identity);
            bottleObj.transform.SetParent(_topShelf.transform);

            Bottle bottle = bottleObj.GetComponent<Bottle>();
            bottle.SetSelectable(true);

            Color colorToSet = i < _availableColors.Count ? _availableColors[i] : Color.white;

            bottle.SetColor(colorToSet);
            bottle.SetPosition(i);

            _bottlesTopShelf.Add(bottleObj);
        }
    }

    public void PlaceSecretSequence()
    {
        _secretSequence = new GameObject[_bottleNumber];

        List<Color> shuffledColors;

        // Garante que NÃO exista nenhum match
        do
        {
            shuffledColors = new List<Color>(_availableColors);
            Shuffle(shuffledColors);
        }
        while (HasAnyMatch(shuffledColors));

        // Instancia os vasos da prateleira de baixo
        for (int i = 0; i < _bottleNumber; i++)
        {
            GameObject bottleObj = Instantiate(
                _bottlePrefab,
                new Vector3(_positions[i], -0.6f, -0.5f),
                Quaternion.identity
            );
            bottleObj.transform.SetParent(_bottomShelf.transform);

            Bottle bottle = bottleObj.GetComponent<Bottle>();
            bottle.SetSelectable(false);

            bottle.SetColor(shuffledColors[i]);
            bottle.SetPosition(i);

            _secretSequence[i] = bottleObj;
        }
    }

    // Verifica se existe algum match (não queremos nenhum)
    bool HasAnyMatch(List<Color> shuffled)
    {
        for (int i = 0; i < shuffled.Count; i++)
        {
            Color topColor = _bottlesTopShelf[i].GetComponent<Bottle>().Color;

            if (shuffled[i] == topColor)
                return true;
        }
        return false;
    }

    // Fisher-Yates shuffle
    void Shuffle(List<Color> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    public void CheckSequence()
    {
        CorrectVases = 0;

        for (int i = 0; i < _bottlesTopShelf.Count; i++)
        {
            Color color1 = _bottlesTopShelf[i].GetComponent<Bottle>().Color;
            Color color2 = _secretSequence[i].GetComponent<Bottle>().Color;

            if (color1 == color2)
                CorrectVases++;
        }

        _UIManager.UpdateCorrectVases(CorrectVases);

        if (CorrectVases == _bottleNumber)
        {
            GameOver();
        }
    }

    public void setIndicatorPosition(Vector3 Pos)
    {
        _IndicatorPrefab.SetActive(true);
        _IndicatorPrefab.transform.position = Pos + _indicatorOfset;
    }

    public void SwapVases()
    {
        int selectedCount = 0;
        GameObject bottleSelected = null;
        int selectedIndex = -1;

        for (int i = 0; i < _bottlesTopShelf.Count; i++)
        {
            GameObject currentBottle = _bottlesTopShelf[i];
            Bottle bottleComponent = currentBottle.GetComponent<Bottle>();

            if (bottleComponent.IsSelected)
            {
                selectedCount++;

                if (selectedCount > 1)
                {
                    _soundManager.PlaySwapSound();
                    _bottlesTopShelf[i] = _bottlesTopShelf[selectedIndex];
                    _bottlesTopShelf[selectedIndex] = currentBottle;

                    bottleComponent.toggleSelected();
                    bottleSelected.GetComponent<Bottle>().toggleSelected();

                    UpdatePlaces();
                    UpdateSwaps();
                    CheckSequence();
                    return;
                }
                
                bottleSelected = _bottlesTopShelf[i];
                selectedIndex = i;
            }
        }

        
    }

    public void UpdatePlaces()
    {
        for (int i = 0; i < _bottlesTopShelf.Count; i++)
        {
            Vector3 newPosition = new Vector3(_positions[i], 0.86f, -0.5f);
            _bottlesTopShelf[i].transform.position = newPosition;
        }
    }

    public void UpdateSwaps()
    {
        _swapCount++;
        _UIManager.UpdateSwapsText(_swapCount);
    }
    private void GameOver()
    {
        isGameOver = true;
        _bottomShelf.SetActive(true);
        string finalTime = _timeSpan.ToString(_timeFormat);
        _UIManager.ShowFinalPanel(finalTime, _swapCount);
    }
    public int GetSelectedCount()
    {
        int count = 0;

        foreach (GameObject bottle in _bottlesTopShelf)
        {
            if (bottle.GetComponent<Bottle>().IsSelected)
                count++;
        }
        Debug.Log($"Selected Count: {count}");
        return count;
    }
}