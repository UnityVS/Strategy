using UnityEngine;

public class Environment : Building
{
    [SerializeField] bool _static;
    Vector2Int _point;
    private void Start()
    {
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
    private void OnDestroy()
    {
        BuildingPlacer.Instance.DeleteBuilding(_point.x, _point.y, this);
    }
}
