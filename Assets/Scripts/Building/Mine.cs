using System.Collections;
using UnityEngine;

public enum FarmResource
{
    Gold,
    Stone,
    Wood
}

public class Mine : MonoBehaviour
{
    bool _isActive = true;
    [SerializeField] AddedValue _addedValue;
    [SerializeField] float _timeResource = 1.5f;
    [SerializeField] FarmResource _currentFarm;
    [SerializeField]
    Coroutine _coroutine;
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
    IEnumerator AddValue()
    {
        while (true)
        {
            yield return new WaitForSeconds(_timeResource);
            _addedValue.AddResource(_timeResource);
        }
    }
}
