using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _bottle;
    [SerializeField] private int _bottleNumber;
    [SerializeField] private float _boundary;

    private List<GameObject> _bottlesTopShelf = new List<GameObject>();
    private GameObject[] _secretSequence; 


    [SerializeField] private List<Color> _availableColors;
    [SerializeField] private List<float> _positions = new List<float>();

    public int CorrectPositions { get; private set; }

    private void Start()
    {
        _secretSequence = new GameObject[_bottleNumber];
        GeneratePositions();
        CreateTopShelf();
        PlaceBottleRandom();
        CheckSequence();
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
            GameObject bottleObj = Instantiate(_bottle, new Vector3(_positions[i], 0.86f, -0.5f), Quaternion.identity);
            Bottle bottle = bottleObj.GetComponent<Bottle>();

            // Segurança: verifica se há cores suficientes
            Color colorToSet = i < _availableColors.Count ? _availableColors[i] : Color.white;

            bottle.SetColor(colorToSet);
            bottle.SetPosition(i);
            _bottlesTopShelf.Add(bottleObj);
        }
    }

    public void PlaceBottleRandom()
    {
        _secretSequence = new GameObject[_bottleNumber];
        List<int> indexesUsed = new List<int>();

        for (int i = 0; i < _bottleNumber; i++)
        {
            int randomIndex;
            // Tenta achar um índice que ainda não foi usado
            do
            {
                randomIndex = UnityEngine.Random.Range(0, _positions.Count);
            } while (indexesUsed.Contains(randomIndex));


            GameObject bottleObj = Instantiate(_bottle, new Vector3(_positions[randomIndex], -0.6f, -0.5f), Quaternion.identity);
            Bottle bottle = bottleObj.GetComponent<Bottle>();

            bottle.SetColor(_availableColors[i]);
            bottle.SetPosition(randomIndex);

            _secretSequence[randomIndex] = bottleObj;
            indexesUsed.Add(randomIndex);
        }
    }

    public void CheckSequence()
    {
        CorrectPositions = 0;
        // Importante: comparar pelo que está na prateleira vs a sequência secreta
        for (int i = 0; i < _bottlesTopShelf.Count; i++)
        {
            Color color1 = _bottlesTopShelf[i].GetComponent<Bottle>().Color;
            Color color2 = _secretSequence[i].GetComponent<Bottle>().Color;
            Debug.Log($"Comparing position {i}: Color1 = {color1}, Color2 = {color2} - igual:{color1 == color2}");
            // Lógica: A garrafa na posição X da prateleira tem a mesma cor que a garrafa X da sequência?
            if (color1 == color2)
            {
                CorrectPositions++;
            }
        }
        Debug.Log($"Correct Positions: {CorrectPositions}");
    }
}