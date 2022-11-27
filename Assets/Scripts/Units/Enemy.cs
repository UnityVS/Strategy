using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum EnemyStates
{
    Idle,
    Attack,
    WalkToBuilding,
    WalkToUnit
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
    void Start()
    {

    }
    void Update()
    {

    }
    void FindClosestUnit()
    {

    }
}
