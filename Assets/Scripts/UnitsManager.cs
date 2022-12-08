using UnityEngine;

public class UnitsManager : MonoBehaviour
{
    int _currentAttackPowerKnight = 1;
    public static UnitsManager Instance;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetAttackPowerKnight(int value)
    {
        _currentAttackPowerKnight = value;
    }
    public int GetAttackPowerKnight()
    {
        return _currentAttackPowerKnight;
    }
}
