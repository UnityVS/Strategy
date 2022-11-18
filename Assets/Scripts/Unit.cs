using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObject
{
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] Animator _animator;
    public override void WhenClickOnGround(Vector3 point)
    {
        _navMeshAgent.stoppingDistance = 0.6f;
        base.WhenClickOnGround(point);
        _navMeshAgent.SetDestination(point);
        _animator.SetBool("Walk", true);
    }
    private void Update()
    {
        if (Vector3.Distance(_navMeshAgent.gameObject.transform.position, _navMeshAgent.destination) < 0.2f)
        {
            _animator.SetBool("Walk", false);
        }
    }
}
