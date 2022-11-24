using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObject
{
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] Animator _animator;
    [SerializeField] int _price;
    public override void WhenClickOnGround(Vector3 point)
    {
        _navMeshAgent.stoppingDistance = 0.65f;
        base.WhenClickOnGround(point);
        _navMeshAgent.SetDestination(point);
        _animator.SetBool("Walk", true);
    }
    public override void Start()
    {
        //base.Start();
        _selectCirle.SetActive(false);
    }
    private void Update()
    {
        if (Vector3.Distance(_navMeshAgent.gameObject.transform.position, _navMeshAgent.destination) < 0.15f)
        {
            _animator.SetBool("Walk", false);
        }
    }
    public int CheckPrice()
    {
        return _price;
    }
}
