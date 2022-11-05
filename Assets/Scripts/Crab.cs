using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crab : MonoBehaviour
{
    const string LEFT = "left";
    const string RIGHT = "right";

    [SerializeField]
    Transform castPos;

    [SerializeField]
    float baseHorizontalCastDist;

    [SerializeField]
    float baseVerticalCastDist;

    string facingDirection;

    Vector3 baseScale;

    Rigidbody2D rb2d;
    readonly float moveSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        baseScale = transform.localScale;
        facingDirection = RIGHT;
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float vX = moveSpeed;

        if(facingDirection == LEFT)
        {
            vX = -moveSpeed;
        }


        rb2d.velocity = new Vector2(vX, 0);

        if (IsHittingWall() || IsAboutToFall())
        {
            ChangeFacingDirection();
        }
    }

    void ChangeFacingDirection()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x = -transform.localScale.x;
        transform.localScale = currentScale;

        facingDirection = facingDirection == LEFT ? RIGHT : LEFT;
    }

    bool IsHittingWall()
    {
        float castDist = facingDirection == LEFT ? -baseHorizontalCastDist : baseHorizontalCastDist;

        Vector3 targetPos = castPos.position;
        targetPos.x += castDist;

        Debug.DrawLine(castPos.position, targetPos, Color.green);

        bool val = Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground"));

        return val;
    }

    bool IsAboutToFall()
    {
        Vector3 targetPos = castPos.position;
        targetPos.y -= baseVerticalCastDist;

        Debug.DrawLine(castPos.position, targetPos, Color.green);

        bool val = !Physics2D.Linecast(castPos.position, targetPos, 1 << LayerMask.NameToLayer("Ground"));

        return val;
    }
}
