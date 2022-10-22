using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] Gradient ColorGradient;
    [SerializeField] TrailRenderer TrailRenderer;
    [SerializeField] float maxIntensity;
    public int score;
    [SerializeField] float MaxSpeed;
    float maxSpeed;
    float jumpHeight;
    [SerializeField] float MaxJumpHeight;
    [SerializeField] float gravity;
    [SerializeField] LayerMask stableGroundLayer;
    float speedSmoothVelocity;
    [Range(0, 1)]
    [SerializeField] float airControlPercent = 0.2f;
    [SerializeField] float speedSmoothTime = 0.1f;

    [SerializeField] AudioClip jumpClip;
    [SerializeField] AudioClip hitClip;
    [SerializeField] AudioClip pick1Clip;
    [SerializeField] AudioClip pick2Clip;
    public AudioClip putClip;
    public AudioClip openPortalClip;

    public bool isGrounded;
    //LevelManager _levelManager;
    [SerializeField] Light2D _pointLight;

    Rigidbody2D rb;
    float _currentSpeed;
    bool _jump;

    float dir = 0f;
    float dirY = 0f;

    Vector2 _input = Vector2.zero;
    Vector2 _lastVelocity = Vector2.zero;

    [SerializeField] float Accelertation = 7f;
    [SerializeField] float Decceleration = 7f;
    [SerializeField] float VelPower = 0.9f;
    [SerializeField] float FrictionAmount = 0.2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        maxSpeed = MaxSpeed;
        jumpHeight = MaxJumpHeight;
        //_levelManager = FindObjectOfType<LevelManager>();
    }

    void Update()
    {
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (isGrounded && _input.y > 0)
        {
            _jump = true;
        }
    }

    private void FixedUpdate()
    {
        if (_jump) //handle jump
        {
            //FindObjectOfType<SoundManager>().PlayClip(jumpClip, 0.35f);
            _jump = false;
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        }

        // X movement
        float targetSpeed = _input.x * MaxSpeed;
        float speedDelta = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Accelertation : Decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDelta) * accelRate, VelPower) * Mathf.Sign(speedDelta);
        rb.AddForce(movement * Vector2.right);

        // Friction
        if (isGrounded && _input.x == 0f)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(FrictionAmount));
            amount *= Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isGrounded && collision.CompareTag("Ground"))
        {
            //_currentSpeed = 0;
            isGrounded = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            //_currentSpeed = 0;
            isGrounded = true;
        }
        /*if (collision.CompareTag("Pickable"))
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
        }*/
    }

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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
