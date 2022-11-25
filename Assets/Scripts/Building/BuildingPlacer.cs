using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[ExecuteAlways]
public class BuildingPlacer : MonoBehaviour
{
    public float CellSize = 1f;
    public static BuildingPlacer Instance;
    Camera _raycastCamera;
    Plane _plane;
    Building _currentBuilding;
    Dictionary<Vector2Int, Building> _buildingDictionary = new Dictionary<Vector2Int, Building>();
    [SerializeField] Building[] _listOfPrefabs;
    [SerializeField] UISettingOnPlay[] _listOfUI;
    public List<Building> _allBuildingInScene;
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
        for (int i = 0; i < _listOfPrefabs.Length; i++)
        {
            for (int x = 0; x < _listOfUI.Length; x++)
            {
                if (_listOfPrefabs[i].name == _listOfUI[x].name)
                {
                    _listOfUI[x].SetTextPrice(_listOfPrefabs[i]);
                }
            }
        }
        _allBuildingInScene.Clear();
    }
    private void Update()
    {
        if (_currentBuilding == null) return;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _currentBuilding.transform.Rotate(0, 90, 0);
        }
        Ray ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);
        float distance;
        _plane.Raycast(ray, out distance);
        Vector3 point = ray.GetPoint(distance) / CellSize;
        int x = Mathf.RoundToInt(point.x);
        int z = Mathf.RoundToInt(point.z);
        _currentBuilding.transform.position = new Vector3(x, 0, z) * CellSize;
        if (CheckAllow(x, z, _currentBuilding))
        {
            _currentBuilding.ShowCorrectColorPosition();
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                InstallBuilding(x, z, _currentBuilding);
                if (_currentBuilding.GetComponent<Mine>() is Mine mine)
                {
                    mine.BuildingSetInScene();
                    mine.MineWork();
                    mine.ObstacleStatus(true);
                }
                else if (_currentBuilding.GetComponent<Barack>() is Barack barack)
                {
                    barack.BuildingSetInScene();
                    barack.ObstacleStatus(true);
                }
                Buy(_currentBuilding);
                _currentBuilding = null;
            }
        }
        else
        {
            _currentBuilding.ShowIncorrectColorPosition();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Destroy(_currentBuilding.gameObject);
            _currentBuilding = null;
        }
    }
    bool CheckAllow(int xPosition, int zPosition, Building building)
    {
        Vector2Int size = building.CheckSize();
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                if (_buildingDictionary.ContainsKey(coordinate))
                {
                    return false;
                }
            }
        }
        return true;
    }
    public void InstallBuilding(int xPosition, int zPosition, Building building)
    {
        Vector2Int size = building.CheckSize();
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                _buildingDictionary.Add(coordinate, building);
                _allBuildingInScene.Add(building);
            }
        }
    }

    public void TryBuy(Building buildingPrefab)
    {
        if (_currentBuilding)
            Destroy(_currentBuilding.gameObject);
        int balance = Resources.Instance.CheckBalance();
        int price = buildingPrefab.CheckPrice();
        if (balance >= price)
        {
            Building newBuilding = Instantiate(buildingPrefab);
            _currentBuilding = newBuilding;
            _currentBuilding.ObstacleStatus(false);
        }
        else
        {
            GameManager.Instance._showHint.DisplayHint("You can't buy this building. Need more money");
        }
    }
    public void Buy(Building buildingPrefab)
    {
        int balance = Resources.Instance.CheckBalance();
        int price = buildingPrefab.CheckPrice();
        Resources.Instance.UpdateResource(balance - price);
    }
    public void DeleteBuilding(int xPosition, int zPosition, Building building)
    {
        Vector2Int coordinate = new Vector2Int(xPosition, zPosition);
        _buildingDictionary.Remove(coordinate, out building);
    }
}
