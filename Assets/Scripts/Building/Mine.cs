using System.Collections;
using UnityEngine;

public enum FarmResource
{
    Gold,
    Stone,
    Wood
}

public class Mine : Building
{
    bool _isActive = false;
    [SerializeField] AddedValue _addedValue;
    [SerializeField] float _timeResource = 1.5f;
    [SerializeField] int _generatedCount = 10;
    [SerializeField] FarmResource _currentFarm;
    [SerializeField] Coroutine _coroutine;
    Resources _resources;
    private void Start()
    {
        _resources = FindObjectOfType<Resources>();
    }
    void Update()
    {
        if (_isActive == false)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            return;
        }
        if (_coroutine == null)
        {
            _coroutine = StartCoroutine(AddValue());
        }
    }
    public void MineWork()
    {
        SelectObjectStatus(false);
        _addedValue.SetStartPosition();
        _isActive = true;
    }
    IEnumerator AddValue()
    {
        while (true)
        {
            yield return new WaitForSeconds(_timeResource);
            _addedValue.AddResource(_timeResource);
            yield return new WaitForSeconds(_timeResource / 2);
            _resources.AddResources(_currentFarm, _generatedCount);
        }
    }
}
