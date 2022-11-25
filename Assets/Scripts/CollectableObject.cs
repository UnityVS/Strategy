using System.Collections;
using TMPro;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    [SerializeField] int _collectableCapacity = 100;
    int _currentCapacity;
    [SerializeField] TextMeshProUGUI _textCapacity;
    [SerializeField] TextMeshProUGUI _textCapacityShadow;
    Unit _currentWorkUnit;
    [SerializeField] FarmResource _currentFarm;
    Transform _nearTargetMinePosition;
    Mine _currentMine;
    bool _unitIsMoving = false;
    bool _nextPosition = false;
    private void Start()
    {
        _currentCapacity = _collectableCapacity;
        UpdateUI();
    }
    public void ChangeCapacity()
    {
        if (_currentCapacity > 1)
        {
            _currentCapacity -= 1;
            UpdateUI();
        }
        else
        {
            StopAllCoroutines();
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
            if (_currentWorkUnit = Managment.Instance.ListObjects()[i].GetComponent<Unit>())
            {
                Vector3 firstPosition = transform.position + Vector3.right * 2f;
                Vector3 secondPosition = FindNearBuilding().position;
                secondPosition = secondPosition + Vector3.right * 2f;
                StartCoroutine(CollectMaterials(firstPosition, secondPosition, _currentWorkUnit));
            }
        }
    }
    Transform FindNearBuilding()
    {
        for (int i = 0; i < BuildingPlacer.Instance._allBuildingInScene.Count; i++)
        {
            if (BuildingPlacer.Instance._allBuildingInScene[i].GetComponent<Mine>() is Mine _mineTarget && _mineTarget._currentFarm == _currentFarm)
            {
                if (_nearTargetMinePosition == null)
                {
                    _currentMine = _mineTarget;
                    return _nearTargetMinePosition = _mineTarget.gameObject.transform;
                }
                else if (Vector3.Distance(_mineTarget.transform.position, _nearTargetMinePosition.position) < Vector3.Distance(transform.position, _nearTargetMinePosition.position))
                {
                    _currentMine = _mineTarget;
                    return _nearTargetMinePosition = _mineTarget.gameObject.transform;
                }
            }
        }
        return _currentWorkUnit.transform;
    }
    IEnumerator CollectMaterials(Vector3 startPosition, Vector3 endPosition, Unit currentUnit)
    {
        while (true)
        {
            if (!_unitIsMoving)
            {
                _unitIsMoving = true;
                currentUnit.WhenClickOnGround(startPosition);
            }
            if (!currentUnit.CheckStop() && !_nextPosition)
            {
                ChangeCapacity();
                UpdateUI();
                _nextPosition = true;
            }
            if (_nextPosition)
            {
                currentUnit.WhenClickOnGround(endPosition);
                yield return new WaitForSeconds(1f);
                //StopAllCoroutines();
            }
            if (!currentUnit.CheckStop() && _nextPosition)
            {
                _currentMine.FromUnitAddValue();
                _unitIsMoving = false;
                _nextPosition = false;
            }
            yield return new WaitForSeconds(1f);
            yield return null;
        }
    }
}
