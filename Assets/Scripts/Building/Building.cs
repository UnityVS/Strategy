using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Building : SelectableObject
{
    [SerializeField] int _goldPrice = 10;
    [SerializeField] int _woodPrice = 0;
    [SerializeField] int _stonePrice = 0;
    [SerializeField] int _xSize = 3;
    [SerializeField] int _zSize = 3;
    Color _startColor;
    [SerializeField] Renderer _renderer;
    [SerializeField] NavMeshObstacle _obstacle;
    [SerializeField] GameObject _menuObject;
    Collider _colliderStatus;
    public override void OnHover()
    {
        //base.OnHover();
    }
    public void ObstacleStatus(bool status)
    {
        _obstacle.enabled = status;
        _colliderStatus.enabled = status;
    }
    private void Awake()
    {
        _colliderStatus = _renderer.GetComponent<Collider>();
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
        //float cellSize = 0.5f;
        //Vector2 offset = new Vector2((_xSize - 1f) * 0.5f, (_zSize - 1f) * 0.5f);
        //for (int i = 0; i < _xSize; i++)
        //{
        //    for (int j = 0; j < _zSize; j++)
        //    {
        //        Gizmos.DrawWireCube(transform.position + new Vector3(i - offset.x, 0f, j - offset.y) * cellSize, new Vector3(1f, 0f, 1f) * cellSize);
        //    }
        //}
    }
    public Vector2Int CheckSize()
    {
        return new Vector2Int(_xSize, _zSize);
    }
    public virtual void ReturnUnit()
    {
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
    public int CheckPrice(string resource)
    {
        if (resource == "wood")
        {
            return _woodPrice;
        }
        if (resource == "stone")
        {
            return _stonePrice;
        }
        if (resource == "gold")
        {
            return _goldPrice;
        }
        return 0;
    }
    public void RendererStatus(bool status)
    {
        _renderer.gameObject.SetActive(status);
    }
}
