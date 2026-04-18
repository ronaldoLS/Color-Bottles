using UnityEngine;

public class Bottle : BottleBase // INHERITANCE
{
    private bool _isSelected;
    private GameManager _gameManager;
    private void Start()
    {
        _isSelected = false;
        _gameManager = GameManager.FindFirstObjectByType<GameManager>();
    }

    public override void OnSelect() // POLYMORPHISM
    {
        _isSelected = !_isSelected;
        Debug.Log($"Bottle at position {_position} selected: {_isSelected}");

    }

    public void SetPosition(int newPosition)
    {
        _position = newPosition;
    }
    public void SetColor(Color color)
    {
        _color = color;
        MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
        mesh.material.color = color;
    }
    void OnMouseDown()
    {
        Debug.Log("Objeto clicado: " + gameObject.name);
        _gameManager.setIndicatorPosition(transform.position);
    }
}