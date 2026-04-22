using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _IndicatorPrefab;
    [SerializeField] private GameObject _bottlePrefab;
    [SerializeField] private float _boundary;

    private Vector3 _indicatorOfset = new(-0.125f, 0.39f, -0.235f);
    private int _bottleNumber;
    private List<GameObject> _bottlesTopShelf = new List<GameObject>();
    private GameObject[] _secretSequence;


    [SerializeField] private List<Color> _availableColors;
    [SerializeField] private List<float> _positions = new List<float>();

    public int CorrectPositions { get; private set; }

    private void Start()
    {
        _bottleNumber = _availableColors.Count; // Assumindo que o número de garrafas é igual ao número de cores disponíveis
        _secretSequence = new GameObject[_bottleNumber];
        GeneratePositions();
        CreateTopShelf();
        PlaceSecretSequence();
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
            Vector3 position = new Vector3(_positions[i], 0.86f, -0.5f);
            GameObject bottleObj = Instantiate(_bottlePrefab, position, Quaternion.identity);
            Bottle bottle = bottleObj.GetComponent<Bottle>();
            bottle.SetSelectable(true);


            // verifica se há cores suficientes
            Color colorToSet = i < _availableColors.Count ? _availableColors[i] : Color.white;

            bottle.SetColor(colorToSet);
            bottle.SetPosition(i);
            _bottlesTopShelf.Add(bottleObj);
        }
    }

    public void PlaceSecretSequence()
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


            GameObject bottleObj = Instantiate(_bottlePrefab, new Vector3(_positions[randomIndex], -0.6f, -0.5f), Quaternion.identity);
            Bottle bottle = bottleObj.GetComponent<Bottle>();
            bottle.SetSelectable(false); // Garrafas da sequência secreta não são selecionáveis

            bottle.SetColor(_availableColors[i]);
            bottle.SetPosition(randomIndex);

            _secretSequence[randomIndex] = bottleObj;
            indexesUsed.Add(randomIndex);
        }
    }

    public void CheckSequence()
    {
        CorrectPositions = 0;
        // Compara as garrafas do topo com a sequência secreta
        for (int i = 0; i < _bottlesTopShelf.Count; i++)
        {
            Color color1 = _bottlesTopShelf[i].GetComponent<Bottle>().Color;
            Color color2 = _secretSequence[i].GetComponent<Bottle>().Color;

            // Verifica se as cores são iguais
            if (color1 == color2)
            {
                CorrectPositions++;
            }
        }
        Debug.Log($"Correct Positions: {CorrectPositions}");
    }
    public void setIndicatorPosition(Vector3 Pos)
    {
        // Lógica para posicionar o indicador com base na posição da garrafa selecionada
        _IndicatorPrefab.SetActive(true);
        _IndicatorPrefab.gameObject.transform.position = Pos + _indicatorOfset;
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
                    _bottlesTopShelf[i] = _bottlesTopShelf[selectedIndex];
                    _bottlesTopShelf[selectedIndex] = currentBottle;

                    bottleComponent.toggleSelected();
                    bottleSelected.GetComponent<Bottle>().toggleSelected();
                    UpdatePlaces();
                    break; // Sai do loop após encontrar mais de uma garrafa selecionada
                }
                bottleSelected = _bottlesTopShelf[i];
                selectedIndex = i;
            }
        }

        CheckSequence(); // Verifica a sequência após a troca
    }

    public void UpdatePlaces()
    {
        for (int i = 0; i < _bottlesTopShelf.Count; i++)
        {
            Vector3 newPosition = new Vector3(_positions[i], 0.86f, -0.5f);
            _bottlesTopShelf[i].transform.position = newPosition;
        }
    }

}