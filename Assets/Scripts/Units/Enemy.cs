using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum EnemyStates
{
    Idle,
    WalkToBuilding,
    WalkToUnit,
    Attack
}
public class Enemy : MonoBehaviour
{
    EnemyStates _currentEnemyState;
    int _currentHealthValue;
    [SerializeField] int _maxHealth;
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] Image _healthBar;
    [SerializeField] float _distanceToFollow;
    [SerializeField] float _distanceToAttack;
    [SerializeField] PlayerBuildings _targetBuilding;
    [SerializeField] Unit _targetUnit;
    [SerializeField] Animator _animator;
    [SerializeField] TextMeshProUGUI _textHealth;
    [SerializeField] TextMeshProUGUI _textHealthShadow;
    float _timer = 3;
    float _maxTimer;
    void Start()
    {
        _maxTimer = _timer;
        _currentHealthValue = _maxHealth;
        UpdateUI();
        SetEnemyState(EnemyStates.WalkToBuilding);
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
            if (_navMeshAgent.destination == null && _targetBuilding != null)
            {
                _navMeshAgent.SetDestination(_targetBuilding.transform.position);
            }
            if (Vector3.Distance(_targetBuilding.transform.position, transform.position) > _distanceToFollow && _targetBuilding != null)
            {
                _currentEnemyState = EnemyStates.Idle;
                return;
            }
            if (Vector3.Distance(_targetBuilding.transform.position, _navMeshAgent.destination) > 1.5f && _targetBuilding != null)
            {
                _navMeshAgent.SetDestination(_targetBuilding.transform.position);
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
            if (_navMeshAgent.destination == null && _targetUnit != null)
            {
                _navMeshAgent.SetDestination(_targetUnit.transform.position);
            }
            if (Vector3.Distance(_targetUnit.transform.position, transform.position) > _distanceToFollow && _targetUnit != null)
            {
                _currentEnemyState = EnemyStates.Idle;
                return;
            }
            if (Vector3.Distance(_targetUnit.transform.position, _navMeshAgent.destination) > 1.5f && _targetUnit != null)
            {
                _navMeshAgent.SetDestination(_targetUnit.transform.position);
            }
            if (_timer < 0)
            {
                SetEnemyState(EnemyStates.WalkToUnit);
            }
        }
        else if (_currentEnemyState == EnemyStates.Attack)
        {

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
                _navMeshAgent.SetDestination(transform.position);
                _timer = _maxTimer;
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

        }
    }
    Unit FindClosestUnit()
    {
        Unit[] allUnits = FindObjectsOfType<Unit>();
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < allUnits.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allUnits[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                if (minDistance < _distanceToFollow)
                {
                    return allUnits[i];
                }
            }
        }
        return null;
    }
    void UpdateUI()
    {
        _textHealth.text = _currentHealthValue + "/" + _maxHealth;
        _textHealthShadow.text = _currentHealthValue + "/" + _maxHealth;
    }
    PlayerBuildings FindClosestBuilding()
    {
        PlayerBuildings[] allBuildings = FindObjectsOfType<PlayerBuildings>();
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < allBuildings.Length; i++)
        {

            float distance = Vector3.Distance(transform.position, allBuildings[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                if (minDistance < _distanceToFollow)
                {
                    return allBuildings[i];
                }
            }
        }
        return null;
    }
}
