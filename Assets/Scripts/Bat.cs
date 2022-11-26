using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    const string LEFT = "left";
    const string RIGHT = "right";

    [SerializeField] Transform _castPos;
    [SerializeField] Transform _attackPos;

    [SerializeField] Transform _pointA;
    [SerializeField] Transform _pointB;

    [SerializeField] float _baseHorizontalCastDist;
    [SerializeField] float _baseVerticalCastDist;
    [SerializeField] float playerFollowDist;
    [SerializeField] float _verticalFollowDist = 0.6f;
    [SerializeField] float _attackRange = 0.6f;
    [SerializeField] float _attackAoERadius = 0.5f;
    [SerializeField] float _attackDamage = 2f;
    [SerializeField] float _stopRange = 0.4f;

    string _facingDirection;
    string _followingPoint;
    Rigidbody2D _rb2d;
    Collider2D _collider;
    Animator _anim;
    Transform _playersTransform;
    HealthManager _healthManager;

    [SerializeField] float _moveSpeed = 0.6f;

    // Start is called before the first frame update
    void Start()
    {
        _healthManager = GetComponent<HealthManager>();
        _facingDirection = RIGHT;
        _rb2d = GetComponent<Rigidbody2D>();
        _followingPoint = _pointA.transform.position.x < _pointB.transform.position.x ? "B" : "A"; // pradzioj judek i desinesni taska
        _anim = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        _playersTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (_healthManager.IsDead)
        {
            _rb2d.gravityScale = 1; // kad nukristu
            _collider.isTrigger = false;
            _rb2d.velocity = new Vector2(0, _rb2d.velocity.y);
            return;
        }
        float vX = _moveSpeed;

        if(_facingDirection == LEFT)
        {
            vX = -_moveSpeed;
        }

        if ((transform.position - _playersTransform.position).sqrMagnitude <= _attackRange * _attackRange)
            vX = 0;

        _rb2d.velocity = new Vector2(vX, _rb2d.velocity.y);

        if (IsPlayerClose())
        {
            MoveTowardsPlayer();

            if (IsPlayerInAttackRange())
            {
                Attack();
            }
        }
        else
        {
            FollowPoints();
        }
    }

    private void FollowPoints()
    {
        if(_followingPoint == "A")
        {
            if(Vector2.Distance(_pointA.position, _rb2d.transform.position) < 0.5)
            {
                _followingPoint = "B";
            }
            else
            {
                ChangeFacingDirection(_rb2d.transform.position.x < _pointA.position.x ? RIGHT : LEFT);
                transform.position = Vector2.MoveTowards(transform.position, _pointA.position, _moveSpeed);
            }
        }
        else
        {
            if (Vector2.Distance(_pointB.position, _rb2d.transform.position) < 0.5)
            {
                _followingPoint = "A";
            }
            else
            {
                ChangeFacingDirection(_rb2d.transform.position.x < _pointB.position.x ? RIGHT : LEFT);
                transform.position = Vector2.MoveTowards(transform.position, _pointB.position, _moveSpeed);
            }
        }
    }

    private void ChangeFacingDirection()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x = -transform.localScale.x;
        transform.localScale = currentScale;

        _facingDirection = _facingDirection == LEFT ? RIGHT : LEFT;
    }

    private void ChangeFacingDirection(string direction)
    {
        if(_facingDirection != direction)
        {
            ChangeFacingDirection();
        }
    }

    private bool IsPlayerClose()
    {
        return Vector2.Distance(_playersTransform.position, _rb2d.transform.position) < playerFollowDist;
    }

    private void MoveTowardsPlayer()
    {
        ChangeFacingDirection(_rb2d.transform.position.x < _playersTransform.position.x ? RIGHT : LEFT);
        if (!IsPlayerInAttackRange())
        {
            transform.position = Vector2.MoveTowards(transform.position, _playersTransform.position, _moveSpeed);
        }
    }

    private bool IsPlayerInAttackRange()
    {
        return Vector2.Distance(_playersTransform.position, _rb2d.transform.position) < _attackRange;
    }

    private void Attack()
    {
        if (_anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Attack"))
        {
            return;
        }
        int attackNumber = UnityEngine.Random.Range(1, 4);
        _anim.Play($"Enemy Attack {attackNumber}");
    }

    public void DoDamage()
    {
        var colliders = Physics2D.OverlapCircleAll(_attackPos.position, _attackAoERadius, 1 << LayerMask.NameToLayer("Default"));
        foreach (var col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                col.GetComponent<HealthManager>().TakeDamage(_attackDamage);
                return;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_attackPos.position, _attackAoERadius);
    }
}
