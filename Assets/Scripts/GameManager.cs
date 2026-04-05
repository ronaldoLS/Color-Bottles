using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _bottle;
    [SerializeField] private int _bottleNumber;
    [SerializeField] private float _boundary;
    private List<Bottle> _bottlesTopShelf;
    private List<Bottle> _secretSequence = new List<Bottle>();
    [SerializeField] private List<Color> _availableColors;

    [SerializeField] private List<float> _positions = new List<float>();


    public int CorrectPositions { get; private set; } // ENCAPSULATION

    private void Start()
    {
        GeneratePositions();
        CreateBottles();

    }
    public void GeneratePositions() // ABSTRACTION
    {
        _positions.Clear();

        float gap = (float)Math.Round(_boundary * 2 / _bottleNumber, 2);
        float centerOffset = ((_bottleNumber - 1) * gap) / 2f;

        for (int i = 0; i < _bottleNumber; i++)
        {
            // Subtrai o offset para centralizar
            float pos = (i * gap) - centerOffset;
            _positions.Add((float)Math.Round(pos, 2));
        }
        Debug.Log("Positions generated");
    }
    public void ShuffleBottles(List<Bottle> bottleList) // ABSTRACTION
    {
        for (int i = 0; i < bottleList.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, bottleList.Count);
            Bottle temp = bottleList[i];
            bottleList[i] = bottleList[randomIndex];
            bottleList[randomIndex] = temp;
        }
        Debug.Log("Bottles shuffled");
    }


    public void CheckSequence()
    {
        CorrectPositions = 0;

        for (int i = 0; i < _bottlesTopShelf.Count; i++)
        {
            if (_bottlesTopShelf[i].Color == _secretSequence[i].Color)
            {
                CorrectPositions++;
            }
        }

        Debug.Log($"Correct Positions: {CorrectPositions}");
    }
    public void CreateBottles() // ABSTRACTION
    {
        _bottlesTopShelf = new List<Bottle>();
        for (int i = 0; i < _positions.Count; i++)
        {
            GameObject bottleObj = Instantiate(_bottle, new Vector3(_positions[i], 0.86f, -0.5f), Quaternion.identity);
            Bottle bottle = bottleObj.GetComponent<Bottle>();
            bottle.SetColor(_availableColors[i]);
            bottle.SetPosition(i);
            _bottlesTopShelf.Add(bottle);
        }
        Debug.Log("Bottles created");
    }

}