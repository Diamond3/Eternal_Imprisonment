using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    const string LEFT = "left";
    const string RIGHT = "right";

    [SerializeField]
    Transform _castPos;
    [SerializeField]
    float _baseHorizontalCastDist;
    [SerializeField]
    float _baseVerticalCastDist;
    [SerializeField]
    float playerFollowDist;
    [SerializeField]
    float _verticalFollowDist = 0.6f;
    [SerializeField]
    float _attackRange = 0.6f;

    string _facingDirection;
    Rigidbody2D _rb2d;
    Animator _anim;
    Transform _playersTransform;

    readonly float _moveSpeed = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
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
        float vX = _moveSpeed;

        if(_facingDirection == LEFT)
        {
            vX = -_moveSpeed;
        }


        _rb2d.velocity = new Vector2(vX, 0);

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
        if (_anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Attack"))
        {
            return;
        }
        int attackNumber = UnityEngine.Random.Range(1, 4);
        _anim.Play($"Enemy Attack {attackNumber}");
    }
}
