using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] int _health;
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] Image _healthBar;
    [SerializeField] float _distanceToFollow;
    [SerializeField] float _distanceToAttack;
    [SerializeField] Building _targetBuilding;
    [SerializeField] Unit _targetUnit;
    [SerializeField] Animator _animator;
    void Start()
    {
        SetEnemyState(EnemyStates.WalkToBuilding);
    }
    void Update()
    {
        if (_currentEnemyState == EnemyStates.Idle)
        {

        }
        else if (_currentEnemyState == EnemyStates.WalkToBuilding)
        {

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
                if (_targetBuilding == null) return;
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
    void FindClosestUnit()
    {

    }
    Building FindClosestBuilding()
    {
        Building[] allBuildings = FindObjectsOfType<Building>();
        float minDistance = Mathf.Infinity;
        Building closestBuilding = null;
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
