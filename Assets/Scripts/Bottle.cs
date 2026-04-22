using UnityEngine;

public class Bottle : BottleBase // INHERITANCE
{
    public bool IsSelected { get; private set; }
    public bool IsSelectable { get; private set; }
    private GameManager _gameManager;
    [SerializeField] private GameObject _indicator;
    [SerializeField] private Vector3 _indicatorOfset = new(-0.125f, 0.39f, -0.235f);

    private void Start()
    {
        IsSelected = false;
        _gameManager = GameManager.FindFirstObjectByType<GameManager>();
        _indicator = GameObject.Instantiate(_indicator, transform.position + _indicatorOfset, Quaternion.identity);
        _indicator.transform.SetParent(transform);
        _indicator.SetActive(false);
    }
    private void Update()
    {
        if (IsSelected)
            _indicator.SetActive(true);

        else
            _indicator.SetActive(false);

    }

    public override void OnSelect() // POLYMORPHISM
    {
        if (!IsSelectable)
            return;

        toggleSelected();
        _gameManager.SwapVases();
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
        Debug.Log(IsSelectable);
        OnSelect();
    }
    public void toggleSelected()
    {
        IsSelected = !IsSelected;
    }
    public void SetSelectable(bool selectable)
    {
        IsSelectable = selectable;
    }
}