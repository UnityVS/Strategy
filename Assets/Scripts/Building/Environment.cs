using UnityEngine;

public class Environment : Building
{
    [SerializeField] bool _static;
    Vector2Int _point;
    [SerializeField] Building _prefabOnlyBuild;
    [SerializeField] bool _onlyCurrentBuild;
    public override void Start()
    {
        //base.Start();
        _selectCirle.SetActive(false);
        Vector3 point = transform.position / BuildingPlacer.Instance.CellSize;
        //Привет Илья, тут есть магия. Не понятно, зачем мы тут сделали -2, но это работает. Загадка от Жака Фреско:)
        int x = Mathf.RoundToInt(point.x) - (CheckSize().x / 2 - 2);
        int z = Mathf.RoundToInt(point.z) - (CheckSize().y / 2 - 2);
        //int x = Mathf.RoundToInt(point.x);
        //int z = Mathf.RoundToInt(point.z);
        _point = new Vector2Int(x, z);
        BuildingPlacer.Instance.InstallBuilding(x, z, this);
        if (_static)
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        if (!_static)
        {
            BuildingPlacer.Instance.DeleteBuilding(_point.x, _point.y, this);
        }
    }
    private void Awake() { }
    //public override void OnHover() { }
    //public override void OnSelect() { }
    //public override void OnUnselect() { }
}
