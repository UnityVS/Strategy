using UnityEngine;

public class Environment : Building
{
    private void Start()
    {
        Vector3 point = transform.position / BuildingPlacer.Instance.CellSize;
        int x = Mathf.RoundToInt(point.x);
        int z = Mathf.RoundToInt(point.z);
        BuildingPlacer.Instance.InstallBuilding(x, z, this);
        Destroy(gameObject);
    }
}
