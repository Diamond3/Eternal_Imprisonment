using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputManager))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] Gradient ColorGradient;
    [SerializeField] TrailRenderer TrailRenderer;
    [SerializeField] float MaxSpeed;
    [SerializeField] float MaxJumpHeight;

    [Space]
    [SerializeField] LayerMask StableGroundLayer;
    [SerializeField] Transform GroundCheckPoint;
    [SerializeField] Vector2 GroundCheckSize;

    public int score;

    /*[SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip hitClip;
    [SerializeField] AudioClip pick1Clip;
    [SerializeField] AudioClip pick2Clip;
    public AudioClip putClip;
    public AudioClip openPortalClip;*/

    public bool isGrounded;
    //LevelManager _levelManager;
    //[SerializeField] Light2D _pointLight;

    Rigidbody2D rb;
    float _currentSpeed;
    bool _jump;

    float _lastGroundedTime = 0f;
    float _lastJumpTime = 0f;
    bool _isJumping = false;
    bool _jumpInputReleased = true;

    InputManager _input;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<InputManager>();
        
    }
    private void Start()
    {

    }

    void Update()
    {
        isGrounded = false;
        if (Physics2D.OverlapBox(GroundCheckPoint.position, GroundCheckSize, 0, StableGroundLayer))
        {
            isGrounded = true;
        }
        if (isGrounded && (_input.Movement.y > 0 || _input.GetKey(Action.Jump)))
        {
            _isJumping = true;

        }
    }

    private void FixedUpdate()
    {
        float yVelocity = rb.velocity.y;
        float xVelocity;

        if (_jump) //handle jump
        {
            //FindObjectOfType<SoundManager>().PlayClip(jumpClip, 0.35f);
            _jump = false;
            yVelocity = Mathf.Sqrt(-2 * Physics2D.gravity.y * rb.gravityScale * MaxJumpHeight);
        }

        //if (isGrounded)
        {
            _currentSpeed = MaxSpeed;
            xVelocity = _currentSpeed * _input.Movement.x;
        }
        rb.velocity = new Vector2(xVelocity, yVelocity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(GroundCheckPoint.position, new Vector3(GroundCheckSize.x * 2, GroundCheckSize.y * 2));
    }
    /*private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isGrounded && collision.CompareTag("Ground"))
        {
            //_currentSpeed = 0;
            isGrounded = true;
        }
    }*/
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            //_currentSpeed = 0;
            isGrounded = true;
        }
        *//*if (collision.CompareTag("Pickable"))
        {
            var clip = UnityEngine.Random.Range(0, 2) == 0 ? pick1Clip : pick2Clip;
            //FindObjectOfType<SoundManager>().PlayClip(clip, 0.35f);
            score++;
            collision.tag = "Untagged";
            //_levelManager.AddScore();
            AdjustIntensity();
            Destroy(collision.gameObject);
        }*/
    /*if (collision.CompareTag("Alien"))
    {
        //_killScore++;
        //FindObjectOfType<SoundManager>().PlayClip(hitClip, 0.15f);
        collision.tag = "Untagged";
        collision.transform.parent.GetComponent<Animator>().SetTrigger("Die");
        var vel = collision.transform.parent.GetComponent<Rigidbody2D>().velocity;
        collision.transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, vel.y);
        //_levelManager.RemoveNpc();
        AdjustSpeeds();
        Destroy(collision.gameObject, 2.5f);
    }*//*
}*/

    /*private void AdjustSpeeds()
    {
        var remaining = _levelManager.CurrentNpcsCount;
        var count = _levelManager.NpcsCount;
        var prc = _levelManager.MaxKillPercentage;

        var delta = 0f;
        if (remaining > 0) delta = (float)remaining / count;
        var col = ColorGradient.Evaluate(1 - delta);

        _pointLight.color = col;
        col.a = 140 / 255f;
        TrailRenderer.startColor = col;
        col.a = 75 / 255f;
        TrailRenderer.endColor = col;

        maxSpeed = Mathf.Lerp(1f, MaxSpeed, delta);
        jumpHeight = Mathf.Lerp(1f, MaxJumpHeight, delta);
    }*/

    /*private void AdjustIntensity()
    {
        var maxShards = _levelManager.ShardCount;
        var score = _levelManager.CurrentScore;
        _pointLight.intensity = Mathf.Lerp( 0.5f, maxIntensity, (float)score / maxShards);
        _pointLight.pointLightOuterRadius = Mathf.Lerp(4f, 7f, (float)score / maxShards);
    }*/

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }*/
}
