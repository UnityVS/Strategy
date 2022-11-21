using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : SelectableObject
{
    [SerializeField] int _price = 10;
    [SerializeField] int _xSize = 3;
    [SerializeField] int _zSize = 3;
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
    public int CheckPrice()
    {
        return _price;
    }
}
