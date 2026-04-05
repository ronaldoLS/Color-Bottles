using UnityEngine;

public abstract class BottleBase : MonoBehaviour
{
    [SerializeField] protected int _position;
    [SerializeField] protected Color _color;

    public int Position => _position; // ENCAPSULATION
    public Color Color => _color;     // ENCAPSULATION

    public abstract void OnSelect(); // ABSTRACTION
}