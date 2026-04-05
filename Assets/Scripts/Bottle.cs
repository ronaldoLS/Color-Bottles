using UnityEngine;

public class Bottle : BottleBase // INHERITANCE
{
    private bool _isSelected;

    public override void OnSelect() // POLYMORPHISM
    {
        _isSelected = !_isSelected;
        Debug.Log($"Bottle at position {_position} selected: {_isSelected}");
    }

    public void SetPosition(int newPosition)
    {
        _position = newPosition;
    }
}