using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    const string LEFT = "left";
    const string RIGHT = "right";

    [SerializeField] Transform _castPos;
    [SerializeField] Transform _attackPos;

    [SerializeField] float _baseHorizontalCastDist;
    [SerializeField] float _baseVerticalCastDist;
    [SerializeField] float playerFollowDist;
    [SerializeField] float _verticalFollowDist = 0.6f;
    [SerializeField] float _attackRange = 0.6f;
    [SerializeField] float _attackAoERadius = 0.5f;
    [SerializeField] float _attackDamage = 2f;
    [SerializeField] float _stopRange = 0.4f;

    string _facingDirection;
    Rigidbody2D _rb2d;
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
        _anim = GetComponent<Animator>();
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
            _rb2d.velocity = new Vector2(0, _rb2d.velocity.y); //Reiks pakeist
            return;
        }
        float vX = _moveSpeed;

        if(_facingDirection == LEFT)
        {
            vX = -_moveSpeed;
        }

        if (_anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Hit") || (transform.position - _playersTransform.position).sqrMagnitude <= _attackRange * _attackRange)
            vX = 0;

        _rb2d.velocity = new Vector2(vX, _rb2d.velocity.y);
        _anim.SetBool("Run", Mathf.Abs(_rb2d.velocity.x) > 0);

        if (IsPlayerClose())
        {
            MoveTowardsPlayer();
        }

        if(IsPlayerInAttackRange())
        {
            Attack();
        }

        if (IsHittingWall() || IsAboutToFall())
        {
            ChangeFacingDirection();
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

    private bool IsHittingWall()
    {
        float castDist = _facingDirection == LEFT ? -_baseHorizontalCastDist : _baseHorizontalCastDist;

        Vector3 targetPos = _castPos.position;
        targetPos.x += castDist;

        Debug.DrawLine(_castPos.position, targetPos, Color.green);

        bool val = Physics2D.Linecast(_castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground"));

        return val;
    }

    private bool IsAboutToFall()
    {
        Vector3 targetPos = _castPos.position;
        targetPos.y -= _baseVerticalCastDist;

        Debug.DrawLine(_castPos.position, targetPos, Color.green);

        bool val = !Physics2D.Linecast(_castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground"));

        return val;
    }

    private bool IsPlayerClose()
    {
        // crab and player are on different levels vertically so shouldn't follow even if it's close
        if (Math.Abs(_playersTransform.position.y - _rb2d.transform.position.y) > _verticalFollowDist)
        {
            return false;
        }

        return Vector2.Distance(_playersTransform.position, _rb2d.transform.position) < playerFollowDist;
    }

    private void MoveTowardsPlayer()
    {
        ChangeFacingDirection(_rb2d.transform.position.x < _playersTransform.position.x ? RIGHT : LEFT);
    }

    private bool IsPlayerInAttackRange()
    {
        return Vector2.Distance(_playersTransform.position, _rb2d.transform.position) < _attackRange;
    }

    private void Attack()
    {
        if (_anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Attack") || _anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Hit"))
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
