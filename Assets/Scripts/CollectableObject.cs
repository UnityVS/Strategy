using TMPro;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    [SerializeField] int _collectableCapacity = 100;
    int _currentCapacity;
    [SerializeField] TextMeshProUGUI _textCapacity;
    [SerializeField] TextMeshProUGUI _textCapacityShadow;
    Unit _currentWorkUnit;
    private void Start()
    {
        _currentCapacity = _collectableCapacity;
        UpdateUI();
    }
    public void ChangeCapacity()
    {
        if (_collectableCapacity > 1)
        {
            _collectableCapacity -= 1;
            UpdateUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void UpdateUI()
    {
        _textCapacity.text = _currentCapacity + "/" + _collectableCapacity;
        _textCapacityShadow.text = _currentCapacity + "/" + _collectableCapacity;
    }
    public void TrySetToWork()
    {
        for (int i = 0; i < Managment.Instance.ListObjects().Count; i++)
        {
            if (Managment.Instance.ListObjects()[i].GetComponent<Unit>() is Unit _currentWorkUnit)
            {
                _currentWorkUnit.WhenClickOnGround(transform.position + Vector3.forward);
            }
        }
    }
}
