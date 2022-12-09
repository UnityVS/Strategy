using System.Collections;
using TMPro;
using UnityEngine;

//[DefaultExecutionOrder(1)]
public class CollectableObject : MonoBehaviour
{
    [SerializeField] int _collectableCapacity = 100;
    [SerializeField] int _randomMinCapacity;
    [SerializeField] int _randomMaxCapacity;
    [SerializeField] int _maxGenerationCapacity;
    int _currentCapacity;
    [SerializeField] TextMeshProUGUI _textCapacity;
    [SerializeField] TextMeshProUGUI _textCapacityShadow;
    Unit _currentWorkUnit;
    [SerializeField] FarmResource _currentFarm;
    Transform _nearTargetMinePosition;
    Mine _currentMine;
    bool _unitIsMoving = false;
    bool _nextPosition = false;
    [SerializeField] Building _collectableBuilding;
    [SerializeField] GameObject _canvasInformation;
    Coroutine _coroutine = null;
    private void Start()
    {
        _collectableCapacity = Random.Range(_randomMinCapacity, _randomMaxCapacity);
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
            _currentCapacity -= 1;
            _collectableBuilding.RendererStatus(false);
            _collectableBuilding.SelectObjectStatus(false);
            _canvasInformation.SetActive(false);
            _collectableBuilding.ShowHideUI(false);
        }
    }
    void UpdateUI()
    {
        _textCapacity.text = _currentCapacity + "/" + _collectableCapacity;
        _textCapacityShadow.text = _currentCapacity + "/" + _collectableCapacity;
    }
    public void TrySetToWork()
    {
        if (Managment.Instance.ListObjects().Count <= 1)
        {
            GameManager.Instance._showHint.DisplayHint("At first pick civilian to set on work");
            return;
        }
        if (FindNearBuilding() == null)
        {
            GameManager.Instance._showHint.DisplayHint("You need to create building for work");
            return;
        }
        for (int i = 0; i < Managment.Instance.ListObjects().Count; i++)
        {
            if (_currentWorkUnit = Managment.Instance.ListObjects()[i].GetComponent<Unit>())
            {
                if (!_currentWorkUnit.GetComponent<Citizen>())
                {
                    GameManager.Instance._showHint.DisplayHint("You need to pick civilian unit to set on work");
                    return;
                }
                if (_currentWorkUnit.CheckWorkStatus() == false)
                {
                    Vector3 firstPosition = transform.position + Vector3.right * 2f;
                    Vector3 secondPosition = FindNearBuilding().position;
                    secondPosition = secondPosition + Vector3.right * 2f;
                    _coroutine = StartCoroutine(CollectMaterials(firstPosition, secondPosition, _currentWorkUnit));
                }
                else
                {
                    return;
                }
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
                    _nearTargetMinePosition = _mineTarget.gameObject.transform;
                }
                else if (Vector3.Distance(transform.position, _mineTarget.transform.position) < Vector3.Distance(transform.position, _nearTargetMinePosition.position))
                {
                    _currentMine = _mineTarget;
                    return _nearTargetMinePosition = _mineTarget.gameObject.transform;
                }
            }
        }
        return _nearTargetMinePosition;
    }
    IEnumerator CollectMaterials(Vector3 startPosition, Vector3 endPosition, Unit currentUnit)
    {
        currentUnit.UnitWorkStatus(true);
        while (true)
        {
            if (currentUnit.CheckWorkStatus() == false)
            {
                if (_coroutine != null)
                {
                    StopCoroutine(_coroutine);
                }
                _unitIsMoving = false;
                _nextPosition = false;
                yield return null;
            }
            if (!_unitIsMoving)
            {
                _unitIsMoving = true;
                currentUnit.WhenInWork(startPosition);
            }
            if (currentUnit.CheckStop() && !_nextPosition)
            {
                ChangeCapacity();
                UpdateUI();
                _nextPosition = true;
            }
            if (_nextPosition)
            {
                currentUnit.WhenInWork(endPosition);
                yield return new WaitForSeconds(1f);
                //StopAllCoroutines();
            }
            if (currentUnit.CheckStop() && _nextPosition)
            {
                _currentMine.FromUnitAddValue();
                _unitIsMoving = false;
                _nextPosition = false;
                if (_currentCapacity == 0)
                {
                    currentUnit.WhenInWork(currentUnit.transform.position + Vector3.back * GetRandomDirection(Random.Range(1, 2)));
                    currentUnit.UnitWorkStatus(false);
                    Destroy(gameObject, 0.5f);
                    StopAllCoroutines();
                }
            }
            yield return new WaitForSeconds(1f);
            yield return null;
        }
    }
    float GetRandomDirection(int value)
    {
        if (value > 1)
        {
            return Random.Range(1f, 2.5f);
        }
        else
        {
            return Random.Range(-2.5f, -1f);
        }
    }
}
