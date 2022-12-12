using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


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
    public List<Building> AllBuildingInScene;
    [SerializeField] LayerMask _layerMaskForBuilding;
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
        AllBuildingInScene.Clear();
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
        point += new Vector3(_currentBuilding.CheckSize().x / 2f, 0, _currentBuilding.CheckSize().y / 2f);
        int x = Mathf.RoundToInt(point.x) - 1;
        int z = Mathf.RoundToInt(point.z) - 1;
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
                if (Physics.CheckSphere(_currentBuilding.transform.position, 3f, _layerMaskForBuilding))
                {
                    return false;
                }
            }
        }
        return true;
    }
    public void InstallBuilding(int xPosition, int zPosition, Building building)
    {
        if (building.GetComponent<PlayerBuildings>() is PlayerBuildings turnOnPlayerBuildingComponent)
        {
            turnOnPlayerBuildingComponent.enabled = true;
        }
        Vector2Int size = building.CheckSize();
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                _buildingDictionary.Add(coordinate, building);
            }
        }
        if (!building.CheckStatic())
        {
            AllBuildingInScene.Add(building);
        }

    }

    public void TryBuy(Building buildingPrefab)
    {
        Managment.Instance.UnselectAll();
        if (_currentBuilding)
            Destroy(_currentBuilding.gameObject);
        int goldBalance = Resources.Instance.CheckBalance(FarmResource.Gold);
        int stoneBalance = Resources.Instance.CheckBalance(FarmResource.Stone);
        int woodBalance = Resources.Instance.CheckBalance(FarmResource.Wood);
        int woodPrice = buildingPrefab.CheckPrice(FarmResource.Wood);
        int goldPrice = buildingPrefab.CheckPrice(FarmResource.Gold);
        int stonePrice = buildingPrefab.CheckPrice(FarmResource.Stone);
        if (goldBalance >= goldPrice && stoneBalance >= stonePrice && woodBalance >= woodPrice)
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
        int goldBalance = Resources.Instance.CheckBalance(FarmResource.Gold);
        int stoneBalance = Resources.Instance.CheckBalance(FarmResource.Stone);
        int woodBalance = Resources.Instance.CheckBalance(FarmResource.Wood);
        int woodPrice = buildingPrefab.CheckPrice(FarmResource.Wood);
        int goldPrice = buildingPrefab.CheckPrice(FarmResource.Gold);
        int stonePrice = buildingPrefab.CheckPrice(FarmResource.Stone);
        Resources.Instance.UpdateResource(FarmResource.Gold, goldBalance - goldPrice);
        Resources.Instance.UpdateResource(FarmResource.Stone, stoneBalance - stonePrice);
        Resources.Instance.UpdateResource(FarmResource.Wood, woodBalance - woodPrice);
    }
    public void DeleteBuilding(int xPosition, int zPosition, Building building)
    {
        if (AllBuildingInScene.Remove(building)) { };
        Vector2Int size = building.CheckSize();
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                _buildingDictionary.Remove(coordinate);
            }
        }
    }
}
