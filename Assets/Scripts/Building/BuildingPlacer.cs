using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    public float CellSize = 1f;
    public static BuildingPlacer Instance;
    Camera _raycastCamera;
    Plane _plane;
    Building _currentBuilding;
    Resources _resources;
    ShowHint _showHint;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        _plane = new Plane(Vector3.up, Vector3.zero);
        _raycastCamera = FindObjectOfType<Camera>();
        _resources = FindObjectOfType<Resources>();
        _showHint = FindObjectOfType<ShowHint>();
    }
    private void Update()
    {
        if (_currentBuilding == null) return;
        Ray ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);
        float distance;
        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance) / CellSize;
        int x = Mathf.RoundToInt(point.x);
        int z = Mathf.RoundToInt(point.z);
        _currentBuilding.transform.position = new Vector3(x, 0, z) * CellSize;
        if (Input.GetMouseButtonDown(0))
        {
            if (_currentBuilding.GetComponent<Mine>() is Mine mine)
            {
                mine.MineWork();
            }
            _currentBuilding = null;
        }
    }
    public void TryBuy(Building buildingPrefab)
    {
        int balance = _resources.CheckBalance();
        int price = buildingPrefab.CheckPrice();
        if (balance >= price)
        {
            _resources.UpdateResource(balance - price);
            Building newBuilding = Instantiate(buildingPrefab);
            _currentBuilding = newBuilding;
        }
        else
        {
            _showHint.DisplayHint("You can't buy this building. Need more money");
        }
    }
}
