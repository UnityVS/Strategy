using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObject
{
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] Animator _animator;
    [SerializeField] int _price;
    bool _unitInWork = false;
    public override void WhenClickOnGround(Vector3 point)
    {
        //if (_animator.GetBool("Walk") == true) return;
        if (_unitInWork) return;
        _navMeshAgent.stoppingDistance = 0.4f;
        base.WhenClickOnGround(point);
        _navMeshAgent.SetDestination(point);
        _animator.SetBool("Walk", true);
    }
    public void WhenInWork(Vector3 point)
    {
        _navMeshAgent.stoppingDistance = 0.4f;
        base.WhenClickOnGround(point);
        _navMeshAgent.SetDestination(point);
        _animator.SetBool("Walk", true);
    }
    public void UnitWorkStatus(bool status)
    {
        _unitInWork = status;
    }
    public void SetPreventivDistination(Vector3 point)
    {
        _navMeshAgent.SetDestination(point);
    }
    public bool CheckWorkStatus()
    {
        return _unitInWork;
    }
    public bool CheckStop()
    {
        if (Vector3.Distance(_navMeshAgent.destination, transform.position) < _navMeshAgent.stoppingDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public override void Start()
    {
        //base.Start();
        _selectCirle.SetActive(false);
    }
    private void Update()
    {
        if (Vector3.Distance(_navMeshAgent.gameObject.transform.position, _navMeshAgent.destination) < 0.41f)
        {
            _animator.SetBool("Walk", false);
        }
    }
    public int CheckPrice()
    {
        return _price;
    }
}
