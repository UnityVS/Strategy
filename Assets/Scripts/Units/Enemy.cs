using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {
        _currentHealthValue = _maxHealth;
        UpdateUI();
        SetEnemyState(EnemyStates.WalkToBuilding);
    }
    void Update()
    {

        if (_currentEnemyState == EnemyStates.Idle)
        {
            SetEnemyState(EnemyStates.WalkToBuilding);
        }
        else if (_currentEnemyState == EnemyStates.WalkToBuilding)
        {
            if (_targetBuilding == null)
            {
                _currentEnemyState = EnemyStates.Idle;
                return;
            }
            else
            {
                _navMeshAgent.SetDestination(_targetBuilding.transform.position);
            }
        }
        else if (_currentEnemyState == EnemyStates.WalkToUnit)
        {

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

        }
        else if (_currentEnemyState == EnemyStates.WalkToBuilding)
        {
            if (_targetBuilding = FindClosestBuilding())
            {
                if (_targetBuilding == null)
                {
                    _currentEnemyState = EnemyStates.Idle;
                    return;
                }
                _navMeshAgent.SetDestination(_targetBuilding.transform.position);
            }
            else
            {
                _currentEnemyState = EnemyStates.Idle;
                return;
            }
        }
        else if (_currentEnemyState == EnemyStates.WalkToUnit)
        {

        }
        else if (_currentEnemyState == EnemyStates.Attack)
        {

        }
    }
    void FindClosestUnit()
    {

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
        PlayerBuildings closestBuilding = null;
        for (int i = 0; i < allBuildings.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allBuildings[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                return closestBuilding = allBuildings[i];
            }
        }
        return null;
    }
}
