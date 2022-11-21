using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : SelectableObject
{
    [SerializeField] int _price = 10;
    [SerializeField] int _xSize = 3;
    [SerializeField] int _zSize = 3;
    Color _startColor;
    [SerializeField] Renderer _renderer;
    private void Awake()
    {
        _startColor = _renderer.material.color;
    }
    private void OnDrawGizmos()
    {
        if (BuildingPlacer.Instance == null) return;
        for (int x = 0; x < _xSize; x++)
        {
            for (int z = 0; z < _zSize; z++)
            {
                Gizmos.DrawWireCube(transform.position + new Vector3(x, 0, z) * BuildingPlacer.Instance.CellSize, new Vector3(1f, 0, 1f) * BuildingPlacer.Instance.CellSize);
            }
        }
    }
    public Vector2Int CheckSize()
    {
        return new Vector2Int(_xSize, _zSize);
    }
    public void ShowCorrectColorPosition()
    {
        _renderer.material.color = _startColor;
    }
    public void ShowIncorrectColorPosition()
    {
        _renderer.material.color = Color.red;
    }
    public int CheckPrice()
    {
        return _price;
    }
}
