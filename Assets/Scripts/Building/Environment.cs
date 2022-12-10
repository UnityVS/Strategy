using UnityEngine;
[DefaultExecutionOrder(1)]
public class Environment : Building
{
    //[SerializeField] bool _static;
    Vector2Int _point;
    [SerializeField] Building _prefabOnlyBuild;
    [SerializeField] bool _onlyCurrentBuild;


    public override void Start()
    {
        _selectCirle.SetActive(false);
        Vector3 point = transform.position / BuildingPlacer.Instance.CellSize;
        int x = Mathf.RoundToInt(point.x);
        int z = Mathf.RoundToInt(point.z);
        _point = new Vector2Int(x, z);
        BuildingPlacer.Instance.InstallBuilding(x, z, this);
        if (_static)
        {
            Destroy(gameObject);
        }
    }
    public override bool CheckStatic()
    {
        return _static;
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
