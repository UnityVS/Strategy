using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Building : SelectableObject
{
    [SerializeField] int _price = 10;
    [SerializeField] int _xSize = 3;
    [SerializeField] int _zSize = 3;
    Color _startColor;
    [SerializeField] Renderer _renderer;
    [SerializeField] NavMeshObstacle _obstacle;
    [SerializeField] GameObject _menuObject;
    public override void OnHover()
    {
        //base.OnHover();
    }
    public void ObstacleStatus(bool status)
    {
        _obstacle.enabled = status;
    }
    private void Awake()
    {
        _menuObject.SetActive(false);
        _startColor = _renderer.material.color;
    }
    public override void OnSelect()
    {
        base.OnSelect();
        _menuObject.SetActive(true);
    }
    public override void OnUnselect()
    {
        if (!this) return;
        base.OnUnselect();
        _menuObject.SetActive(false);
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
    public void ShowHideUI(bool status)
    {
        _menuObject.SetActive(status);
    }
    public int CheckPrice()
    {
        return _price;
    }
}
