using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackingUnits
{
    Idle,
    WalkToEnemy,
    Attack,
    WalkToPoint
}

public class Knight : Unit
{
    [SerializeField] Renderer _renderer;
    [SerializeField] Renderer _hat;
    [SerializeField] Color _baseColor = new Color(0, 0, 0, 0);
    Color _lerpColor = new Color(0, 0, 0, 0);
    [SerializeField] Color _hightLight;
    Coroutine _coroutine;
    bool _isHightLight;
    [SerializeField] float _distanceToFollow;
    [SerializeField] float _distanceToAttack;
    Vector3 _pointToChillWalk;
    int _attackPower = 1;
    Enemy _targetEnemy;
    float _timer;
    [SerializeField] float _maxTimer = 1.5f;
    AttackingUnits _currentState;
    //[SerializeField] Animator _animator;
    //int _timerCicleCounter;
    //int _ciclesForBackwardMove = 3;
    //[SerializeField] NavMeshAgent _navMeshAgent;
    //bool _chillWalk = false;
    private void Awake()
    {
        _attackPower = UnitsManager.Instance.GetAttackPowerKnight();
    }
    public override void Start()
    {
        _selectCirle.SetActive(false);
        _hat.material = _renderer.material;
        _timer = _maxTimer;
        SetEnemyState(AttackingUnits.Idle);
    }
    public override void OnUnhover()
    {
        StopCoroutine(_coroutine);
        _isHightLight = false;
        _coroutine = StartCoroutine(HightLightColor(false));
    }
    public override void OnHover()
    {
        _lerpColor = _baseColor;
        if (!_isHightLight)
        {
            _isHightLight = true;
            _coroutine = StartCoroutine(HightLightColor(true));
        }
    }
    public void ChangeAttackPower(int volume)
    {
        _attackPower += volume;
    }
    public void SetAttackPower(int volume)
    {
        _attackPower = volume;
    }
    IEnumerator HightLightColor(bool stutus)
    {
        float timer = 0.25f;
        for (float t = 0; t < 1f; t += Time.deltaTime / timer)
        {
            for (int i = 0; i < _renderer.materials.Length; i++)
            {
                if (stutus)
                {
                    _lerpColor = Color.Lerp(_baseColor, _hightLight, t);
                }
                else
                {
                    _lerpColor = Color.Lerp(_hightLight, _baseColor, t);
                }
                _renderer.materials[i].SetColor("_EmissionColor", _lerpColor);
            }
            yield return null;
        }
    }
    public override void WhenClickOnGround(Vector3 point)
    {
        _currentState = AttackingUnits.WalkToPoint;
        base.WhenClickOnGround(point);

    }
    void Update()
    {
        if (Vector3.Distance(_navMeshAgent.gameObject.transform.position, _navMeshAgent.destination) < 0.41f)
        {
            _animator.SetBool("Walk", false);
        }
        if (AttackingUnits.WalkToPoint == _currentState)
        {

            if (_animator.GetBool("Walk") == false)
            {
                _currentState = AttackingUnits.Idle;
            }
            return;
        }
        if (_targetEnemy != null)
        {
            transform.rotation = Quaternion.LookRotation(_targetEnemy.transform.position - transform.position, Vector3.up);
        }
        _timer -= Time.deltaTime;
        if (_currentState == AttackingUnits.Idle)
        {
            SetEnemyState(AttackingUnits.Idle);
        }
        else if (_currentState == AttackingUnits.WalkToEnemy)
        {
            if (_targetEnemy == null)
            {
                _currentState = AttackingUnits.Idle;
                return;
            }
            if (_targetEnemy != null && (Vector3.Distance(transform.position, _navMeshAgent.destination) > 1.1f))
            {
                WhenClickOnGround(_targetEnemy.transform.position);
            }
            if (Vector3.Distance(_targetEnemy.transform.position, transform.position) > _distanceToFollow && _targetEnemy != null)
            {
                _targetEnemy.UnSubscribeToAttack(this);
                _currentState = AttackingUnits.Idle;
            }
            if (Vector3.Distance(_targetEnemy.transform.position, _navMeshAgent.destination) > 1.5f && _targetEnemy != null)
            {
                WhenClickOnGround(_targetEnemy.transform.position);
            }
            if (Vector3.Distance(transform.position, _targetEnemy.transform.position) < 2f)
            {
                WhenClickOnGround(transform.position);
                _currentState = AttackingUnits.Attack;
                return;
            }
            if (_timer < 0)
            {
                SetEnemyState(AttackingUnits.WalkToEnemy);
            }
        }
        else if (_currentState == AttackingUnits.Attack)
        {
            if (_timer < 0)
            {
                if (_targetEnemy != null)
                {
                    _animator.SetTrigger("Attack");

                    SetEnemyState(AttackingUnits.Attack);
                    _timer = _maxTimer;
                    return;
                }
                else if (_pointToChillWalk != null)
                {
                    _timer = _maxTimer;
                    return;
                }
                _targetEnemy = null;
                _pointToChillWalk = Vector3.zero;
                SetEnemyState(AttackingUnits.Idle);
            }
        }
    }
    public void PullDamage()
    {
        if (_targetEnemy != null)
        {
            _targetEnemy.GetComponent<Health>().ChangeHealthSubtract(_attackPower);
        }
    }
    public void SetEnemyState(AttackingUnits enemystate)
    {
        _currentState = enemystate;
        if (_currentState == AttackingUnits.Idle)
        {
            if (_timer < 0)
            {
                if (_targetEnemy = FindClosestUnit())
                {
                    _currentState = AttackingUnits.WalkToEnemy;
                    return;
                }
                _timer = _maxTimer;
            }
        }
        else if (_currentState == AttackingUnits.WalkToEnemy)
        {
            if (_targetEnemy = FindClosestUnit())
            {
                _currentState = AttackingUnits.WalkToEnemy;
                return;
            }
            if (_targetEnemy == null)
            {
                _currentState = AttackingUnits.Idle;
                return;
            }
            _timer = _maxTimer;
        }
        else if (_currentState == AttackingUnits.Attack)
        {
            if (_timer < 0)
            {
                if (_targetEnemy = FindClosestUnit())
                {
                    if (_targetEnemy.GetComponent<Enemy>())
                    {
                        _currentState = AttackingUnits.WalkToEnemy;
                    }
                    else
                    {
                        _currentState = AttackingUnits.Idle;
                        _targetEnemy = null;
                        return;
                    }
                }
            }
            if (_targetEnemy == null)
            {
                _currentState = AttackingUnits.Idle;
                _targetEnemy = null;
                return;
            }
            _timer = _maxTimer;
        }
    }
    Enemy FindClosestUnit()
    {
        Enemy[] allUnits = FindObjectsOfType<Enemy>();
        List<Enemy> _enemyToFollow = new List<Enemy>();
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < allUnits.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, allUnits[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                if (_targetEnemy != null)
                {
                    return _targetEnemy;
                }
                if (minDistance < _distanceToFollow)
                {
                    _enemyToFollow.Add(allUnits[i]);
                    if (_enemyToFollow.Count > 1)
                    {
                        _enemyToFollow.RemoveAt(0);
                    }
                }
            }
        }
        if (_enemyToFollow.Count > 0)
        {
            _enemyToFollow[0].SubscribeToAttack(this);
            return _enemyToFollow[0];
        }
        return null;
    }
    public override void EnemyClear()
    {
        //base.EnemyClear;
        _targetEnemy = null;
        WhenClickOnGround(transform.position);
        _currentState = AttackingUnits.Idle;
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        if (_livingBuilding != null)
        {
            _livingBuilding.ReturnUnit();
            _livingBuilding.SetDeleteUnitToThisBuilding(this, false);
        }
    }
}
