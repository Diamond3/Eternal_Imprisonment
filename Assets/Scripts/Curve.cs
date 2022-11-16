using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(LineRenderer))]
public class Curve : MonoBehaviour
{
    public Transform GroundPoint, TargetPoint, ControlPoint_0, ControlPoint_1;
    public float T = 0;
    public LineRenderer lineRenderer;

    [SerializeField] int curveCount = 0;
    [SerializeField] int layerOrder = 0;
    [SerializeField] int _segmentCount = 20;

    [Range(0, 1)] [SerializeField] float _percentage = 0.3f;
    [Range(0, 3)][SerializeField] float _controlPoint0DistPrc = 0.5f;
    [Range(0, 3)][SerializeField] float _controlPoint1DistPrc = 1f;

    public Vector3[] FirstPart;
    public Transform PlayerPoint;

    void Start()
    {
        PlayerPoint = GameObject.FindGameObjectWithTag("Player").transform;
        TargetPoint = PlayerPoint;

        FirstPart = new Vector3[_segmentCount];
        /*if (!lineRenderer)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        lineRenderer.sortingLayerID = layerOrder;
        curveCount = (int)controlPoints.Length / 3;*/

        lineRenderer = GetComponent<LineRenderer>();
    }

    Vector3 Bezier(Vector3 a0, Vector3 a1, Vector3 a2, Vector3 a3, float t)
    {
        var b0 = Vector3.Lerp(a0, a1, t);
        var b1 = Vector3.Lerp(a1, a2, t);
        var b2 = Vector3.Lerp(a2, a3, t);

        var c0 = Vector3.Lerp(b0, b1, t);
        var c1 = Vector3.Lerp(b1, b2, t);

        var d0 = Vector3.Lerp(c0, c1, t);

        return d0;
    }

    void Update()
    {
        UpdateControlPoints(ControlPoint_0, ControlPoint_1);
        var lastVector = Vector2.zero;
        for (int i = 0; i < _segmentCount; i++)
        {
            if (i > _segmentCount * _percentage)
            {
                FirstPart[i] = lastVector;
                continue;
            }
            var t = Mathf.InverseLerp(0, _segmentCount - 1, i);
            var a = Bezier(GroundPoint.position, ControlPoint_0.position, ControlPoint_1.position, TargetPoint.position, t);
            lastVector = a;
            FirstPart[i] = a;
        }
        lineRenderer.positionCount = _segmentCount;
        lineRenderer.SetPositions(FirstPart);
    }

    private void UpdateControlPoints(Transform controlPoint_0, Transform controlPoint_1)
    {
        var distToTarget = Vector2.Distance(TargetPoint.position, GroundPoint.position);

        var dirToTarget = (TargetPoint.position - GroundPoint.position).normalized;
        Debug.DrawRay(GroundPoint.position, dirToTarget * 3, Color.red);

        var perpendicularDir = Vector2.Perpendicular(dirToTarget); //Change direction if in the left side of GroundPoint
        Debug.DrawRay(TargetPoint.position, perpendicularDir * 3, Color.red);

        ControlPoint_1.position = TargetPoint.position + (Vector3)perpendicularDir * (_controlPoint0DistPrc * distToTarget);

        var vectorIn45Angle = Quaternion.AngleAxis(45, Vector3.forward) * perpendicularDir;
        ControlPoint_0.position = TargetPoint.position + vectorIn45Angle * (_controlPoint1DistPrc * distToTarget);

        Debug.DrawLine(TargetPoint.position, ControlPoint_0.position, Color.green);
        Debug.DrawLine(TargetPoint.position, ControlPoint_1.position, Color.green);
        print(distToTarget);
    }

    private void OnDrawGizmos()
    {
        if (FirstPart != null)
        {
            for (int i = 1; i < FirstPart.Length; i++)
            {
                Debug.DrawLine(FirstPart[i], FirstPart[i - 1]);
            }
            /*for (int i = 1; i < SecondPart.Length; i++)
            {
                Debug.DrawLine(SecondPart[i], SecondPart[i - 1], Color.red);
            }*/
        }
    }
}