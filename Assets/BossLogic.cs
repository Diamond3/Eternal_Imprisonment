using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLogic : MonoBehaviour
{
    [SerializeField] float _speed = 3f;
    [SerializeField] float _minRotateDist = 0.1f;
    [SerializeField] float _longAttackStopDistance = 1f;
    [SerializeField] float _shortAttackStopDistance = 1.5f;
    [SerializeField] float _betweenAttacksTime = 1f;

    [SerializeField] Transform _attackPoint;
    [SerializeField] Vector2 _shortAttackSize;
    [SerializeField] float _shortAttackDamage = 5f;

    [SerializeField] Vector2 _longAttackSize;
    [SerializeField] float _longAttackDamage = 3f;

    HealthManager _healthManager;
    float _nextAttack = 0f;
    Transform _player;
    Animator _anim;
    bool _isFacingRight = true;

    enum Phases { Slow, Fast, Mad }

    // Start is called before the first frame update
    void Start()
    {
        _healthManager = GetComponent<HealthManager>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _anim = GetComponent<Animator>();

        _nextAttack = Time.time + _betweenAttacksTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (_healthManager.IsDead) return;
        LookAtPlayer();
        if (!IsPlayerInAttackRange() && !_anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Attack") && !_anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Smash"))
        {
            Walk();
        }
        else
        {
            if (Time.time >= _nextAttack)
            {
                _anim.SetBool("Walk", false);
                Attack();
                _nextAttack = Time.time + _betweenAttacksTime;
            }
        }
        UpdateTimers();
    }

    private void Attack()
    {
        if (IsPlayerInShortAttackRange())
        {
            _anim.SetTrigger("Smash_Attack");
        }
        else
        {
            _anim.SetTrigger("Attack");
        }
    }
    bool IsPlayerInShortAttackRange()
    {
        var sqrDist = (transform.position - _player.transform.position).sqrMagnitude;
        return sqrDist < _shortAttackStopDistance * _shortAttackStopDistance;
    }
    bool IsPlayerInAttackRange()
    {
        var sqrDist = (transform.position - _player.transform.position).sqrMagnitude;
        return sqrDist < _longAttackStopDistance * _longAttackStopDistance;
    }

    private void UpdateTimers()
    {
        
    }

    private void Walk()
    {
        float move = _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector2(_player.position.x, transform.position.y), move);
        _anim.SetBool("Walk", true);
        _anim.ResetTrigger("Smash_Attack");
        _anim.ResetTrigger("Attack");
    }

    private void LookAtPlayer()
    {
        if (_isFacingRight && _player.position.x <= transform.position.x - _minRotateDist)
        {
            transform.localScale = new Vector3(1, 1, 1);
            _isFacingRight = false;
        }
        else if (!_isFacingRight && _player.position.x >= transform.position.x + _minRotateDist)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            _isFacingRight = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _longAttackStopDistance);
        Gizmos.DrawLine((Vector2)_attackPoint.position - _longAttackSize/2, (Vector2)_attackPoint.position + _longAttackSize/2);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _shortAttackStopDistance);
        Gizmos.DrawLine((Vector2)transform.position - _shortAttackSize/2, (Vector2)transform.position + _shortAttackSize/2);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _minRotateDist);
    }

    void DebugDrawBox(Vector2 point, Vector2 size, float angle, Color color, float duration = 1f)
    {

        var orientation = Quaternion.Euler(0, 0, angle);

        // Basis vectors, half the size in each direction from the center.
        Vector2 right = orientation * Vector2.right * size.x / 2f;
        Vector2 up = orientation * Vector2.up * size.y / 2f;

        // Four box corners.
        var topLeft = point + up - right;
        var topRight = point + up + right;
        var bottomRight = point - up + right;
        var bottomLeft = point - up - right;

        // Now we've reduced the problem to drawing lines.
        Debug.DrawLine(topLeft, topRight, color, duration);
        Debug.DrawLine(topRight, bottomRight, color, duration);
        Debug.DrawLine(bottomRight, bottomLeft, color, duration);
        Debug.DrawLine(bottomLeft, topLeft, color, duration);
    }

    private void DoAttack()
    {
        var colliders = Physics2D.OverlapBoxAll(_attackPoint.position, _longAttackSize, 1 << LayerMask.NameToLayer("Default"));
        DebugDrawBox(_attackPoint.position, _longAttackSize, 0f, Color.yellow);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                col.GetComponent<HealthManager>().TakeDamage(_longAttackDamage);
                return;
            }
        }
    }

    private void DoSmash()
    {
        var colliders = Physics2D.OverlapBoxAll(transform.position, _shortAttackSize, 1 << LayerMask.NameToLayer("Default"));
        DebugDrawBox(transform.position, _shortAttackSize, 0f, Color.blue);
        foreach (var col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                col.GetComponent<HealthManager>().TakeDamage(_shortAttackDamage);
                return;
            }
        }
    }
}
