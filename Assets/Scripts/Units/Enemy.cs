using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates
{
    Idle,
    WalkToBuilding,
    WalkToUnit,
    Attack
}
public class Enemy : MonoBehaviour
{
    [SerializeField] ValueAddEffect _effectAddValuePrefab;
    EnemyStates _currentEnemyState;
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] float _distanceToFollow;
    [SerializeField] float _distanceToAttack;
    [SerializeField] PlayerBuildings _targetBuilding;
    [SerializeField] Unit _targetUnit;
    [SerializeField] Animator _animator;
    Vector3 _startPosition;
    float _timer;
    float _maxTimer = 2;
    int _timerCicleCounter;
    int _ciclesForBackwardMove = 3;
    List<Unit> _subscribersAttackers = new List<Unit>();
    void Start()
    {
        _startPosition = transform.position;
        _timer = _maxTimer;
        SetEnemyState(EnemyStates.WalkToBuilding);
    }
    public void SubscribeToAttack(Unit unit)
    {
        _subscribersAttackers.Add(unit);
    }
    public void ChangeDistanceToFollow()
    {
        _distanceToFollow = 1000;
    }
    public void UnSubscribeToAttack(Unit unit)
    {
        _subscribersAttackers.Remove(unit);
    }
    private void OnDestroy()
    {
        for (int i = 0; i < _subscribersAttackers.Count; i++)
        {
            if (_subscribersAttackers[i] != null)
            {
                _subscribersAttackers[i].EnemyClear();
            }
        }
        _subscribersAttackers.Clear();
        int addValue = Random.Range(1, 5);
        Resources.Instance.AddResources(FarmResource.Gold, addValue);
        ValueAddEffect effect = Instantiate(_effectAddValuePrefab, transform.position, Quaternion.Euler(60,-11,0));
        effect.SetValueMining(addValue);
        effect.AddResource(1.5f);
    }
    void Update()
    {
        _timer -= Time.deltaTime;
        if (_currentEnemyState == EnemyStates.Idle)
        {
            SetEnemyState(EnemyStates.Idle);
        }
        else if (_currentEnemyState == EnemyStates.WalkToBuilding)
        {
            if (_targetBuilding == null)
            {
                _currentEnemyState = EnemyStates.Idle;
                return;
            }
            if (_navMeshAgent.destination == null && _targetBuilding != null && (Vector3.Distance(transform.position, _navMeshAgent.destination) > 1.1f))
            {
                _navMeshAgent.SetDestination(_targetBuilding.transform.position);
            }
            if (Vector3.Distance(_targetBuilding.transform.position, transform.position) > _distanceToFollow && _targetBuilding != null)
            {
                _currentEnemyState = EnemyStates.Idle;
            }
            if (Vector3.Distance(_targetBuilding.transform.position, _navMeshAgent.destination) > 1.5f && _targetBuilding != null)
            {
                _navMeshAgent.SetDestination(_targetBuilding.transform.position);
            }
            if (Vector3.Distance(transform.position, _targetBuilding.transform.position) < 2f)
            {
                _navMeshAgent.SetDestination(transform.position);
                _currentEnemyState = EnemyStates.Attack;
                return;
            }
            if (_timer < 0)
            {
                SetEnemyState(EnemyStates.WalkToBuilding);
            }
        }
        else if (_currentEnemyState == EnemyStates.WalkToUnit)
        {
            if (_targetUnit == null)
            {
                _currentEnemyState = EnemyStates.Idle;
                return;
            }
            if (_targetUnit != null && (Vector3.Distance(transform.position, _navMeshAgent.destination) > 1.1f))
            {
                _navMeshAgent.SetDestination(_targetUnit.transform.position);
            }
            if (Vector3.Distance(_targetUnit.transform.position, transform.position) > _distanceToFollow && _targetUnit != null)
            {
                _currentEnemyState = EnemyStates.Idle;
            }
            if (Vector3.Distance(_targetUnit.transform.position, _navMeshAgent.destination) > 1.5f && _targetUnit != null)
            {
                _navMeshAgent.SetDestination(_targetUnit.transform.position);
            }
            if (Vector3.Distance(transform.position, _targetUnit.transform.position) < 1.5f)
            {
                _currentEnemyState = EnemyStates.Attack;
                return;
            }
            if (_timer < 0)
            {
                SetEnemyState(EnemyStates.WalkToUnit);
            }
        }
        else if (_currentEnemyState == EnemyStates.Attack)
        {
            if (_timer < 0)
            {
                if (_targetUnit != null)
                {
                    _targetUnit.GetComponent<Health>().ChangeHealthSubtract(1);
                    SetEnemyState(EnemyStates.Attack);
                    _timer = _maxTimer;
                    return;
                }
                else if (_targetBuilding != null)
                {
                    _targetBuilding.GetComponent<Health>().ChangeHealthSubtract(1);
                    SetEnemyState(EnemyStates.Attack);
                    _timer = _maxTimer;
                    return;
                }
                _targetUnit = null;
                _targetBuilding = null;
                SetEnemyState(EnemyStates.Idle);
            }
        }
    }
    public void SetEnemyState(EnemyStates enemystate)
    {
        _currentEnemyState = enemystate;
        if (_currentEnemyState == EnemyStates.Idle)
        {
            if (_timer < 0)
            {
                if (_targetBuilding = FindClosestBuilding())
                {
                    _currentEnemyState = EnemyStates.WalkToBuilding;
                    return;
                }
                else if (_targetUnit = FindClosestUnit())
                {
                    _currentEnemyState = EnemyStates.WalkToUnit;
                    return;
                }
                _timer = _maxTimer;
                if (_startPosition != _navMeshAgent.destination)
                {
                    _timerCicleCounter += 1;
                    if (_timerCicleCounter == _ciclesForBackwardMove)
                    {
                        _navMeshAgent.SetDestination(_startPosition);
                        _timerCicleCounter = 0;
                        return;
                    }
                    return;
                }
                _navMeshAgent.SetDestination(transform.position);
            }
        }
        else if (_currentEnemyState == EnemyStates.WalkToBuilding)
        {
            if (_targetUnit = FindClosestUnit())
            {
                _targetBuilding = null;
                _currentEnemyState = EnemyStates.WalkToUnit;
                return;
            }
            if (_targetBuilding = FindClosestBuilding())
            {
                _navMeshAgent.SetDestination(_targetBuilding.transform.position);
            }
            if (_targetBuilding == null)
            {
                _currentEnemyState = EnemyStates.Idle;
                return;
            }
            _timer = _maxTimer;
        }
        else if (_currentEnemyState == EnemyStates.WalkToUnit)
        {
            if (_targetUnit = FindClosestUnit())
            {
                _targetBuilding = null;
                _currentEnemyState = EnemyStates.WalkToUnit;
                return;
            }
            if (_targetBuilding = FindClosestBuilding())
            {
                if (_targetBuilding == null)
                {
                    _currentEnemyState = EnemyStates.Idle;
                    return;
                }
                _navMeshAgent.SetDestination(_targetBuilding.transform.position);
            }
            if (_targetUnit == null)
            {
                _currentEnemyState = EnemyStates.Idle;
                return;
            }
            _timer = _maxTimer;
        }
        else if (_currentEnemyState == EnemyStates.Attack)
        {
            if (_timer < 0)
            {
                if (_targetUnit = FindClosestUnit())
                {
                    _targetBuilding = null;
                    _currentEnemyState = EnemyStates.WalkToUnit;
                }
                else if (_targetBuilding = FindClosestBuilding())
                {
                    if (_targetBuilding == null)
                    {
                        _currentEnemyState = EnemyStates.Idle;
                        return;
                    }
                    _navMeshAgent.SetDestination(_targetBuilding.transform.position);
                }
            }
            if (_targetUnit == null)
            {
                _currentEnemyState = EnemyStates.Idle;
                return;
            }
            _timer = _maxTimer;
        }
    }
    Unit FindClosestUnit()
    {
        Unit[] allUnits = FindObjectsOfType<Unit>();
        List<Unit> _unitsToFollow = new List<Unit>();
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < allUnits.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allUnits[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                if (minDistance < _distanceToFollow)
                {
                    _unitsToFollow.Add(allUnits[i]);
                    if (_unitsToFollow.Count > 1)
                    {
                        _unitsToFollow.RemoveAt(0);
                    }
                }
            }
        }
        if (_unitsToFollow.Count > 0)
        {
            return _unitsToFollow[0];
        }
        return null;
    }
    PlayerBuildings FindClosestBuilding()
    {
        List<PlayerBuildings> buildingToFollow = new List<PlayerBuildings>();
        PlayerBuildings[] allBuildings = FindObjectsOfType<PlayerBuildings>();
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < allBuildings.Length; i++)
        {
            if (allBuildings[i].enabled)
            {
                float distance = Vector3.Distance(transform.position, allBuildings[i].transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    if (minDistance < _distanceToFollow)
                    {
                        buildingToFollow.Add(allBuildings[i]);
                        if (buildingToFollow.Count > 1)
                        {
                            buildingToFollow.RemoveAt(0);
                        }
                    }
                }
            }
        }
        if (buildingToFollow.Count > 0)
        {
            return buildingToFollow[0];
        }
        return null;
    }
}
