using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabricationHandler : MonoBehaviour
{

    public enum State
    {
        Idle,
        Ongoing,
        Pass,
        Fail
    }

    public enum Method
    {
        Tap,
        Slice,
    }

    public State state;
    public Method method;
    public Camera linkedCamera;
    public Transform startPoint;
    public List<Transform> segmentPoints;
    public Transform endPoint;

    public Transform debugKnob;

    public LineRenderer lineRenderer;

    List<Vector2> points = new List<Vector2>();
    Vector2 previousPos;

    public float
        targetTapRadius,
        targetTapOffset,
        targetSliceWidth;

    bool isValid;

    Vector2 startingTapPosition;

    int currentSegment;



    void Start()
    {
        state = State.Idle;
        if (lineRenderer)
        {
            lineRenderer.useWorldSpace = true;
            ResetLineRenderer();
        }
    }
    void Update()
    {

    }

    void CheckValidityAtStart(Vector2 input)
    {
        Vector2 w = linkedCamera.ScreenToWorldPoint(input);
        previousPos = w;

        StartLineDrawing(w);

        switch (method)
        {
            case Method.Tap:
                if (!IsWithinRange(w, startPoint.position, targetTapRadius))
                    isValid = false;
                startingTapPosition = w;
                break;

            case Method.Slice:
                if (!IsBehindStartPoint(w))
                    isValid = false;
                SetSlicePoints();
                currentSegment = 0;
                break;
        }
    }
    void CheckValidityAtUpdate(Vector2 input)
    {
        Vector2 w = linkedCamera.ScreenToWorldPoint(input);
        SetPoint(w);

        if (!isValid) return;

        switch (method)
        {
            case Method.Tap:
                if (!IsWithinRange(w, startingTapPosition, targetTapOffset))
                    isValid = false;
                break;
            case Method.Slice:
                if (!IsSliceValid(w))
                    isValid = false;

                if (debugKnob != null)
                {
                    if (currentSegment > 0 && currentSegment < points.Count)
                        debugKnob.position = points[currentSegment - 1];
                }
                break;
        }

        previousPos = w;
    }
    void CheckResultsAtEnd(Vector2 input)
    {
        ResetLineRenderer();

        if (isValid)
        {
            Vector2 w = linkedCamera.ScreenToWorldPoint(input);

            switch (method)
            {
                case Method.Tap:
                    if (!IsWithinRange(w, startingTapPosition, targetTapOffset))
                        isValid = false;
                    break;

                case Method.Slice:
                    if (currentSegment != points.Count)
                        isValid = false;
                    break;
            }
        }

        if (isValid) state = State.Pass;
        else state = State.Fail;
    }


    bool IsWithinRange(Vector2 p1, Vector2 p2, float d)
    {
        return (p2 - p1).magnitude <= d;
    }
    bool IsBehindStartPoint(Vector2 v)
    {
        if (segmentPoints.Count > 0)
            return ProjectionDistance(v, startPoint.position, segmentPoints[0].position) < 0.0f;
        else
            return ProjectionDistance(v, startPoint.position, endPoint.position) < 0.0f;
    }

    bool IsSliceValid(Vector2 v)
    {
        if (currentSegment == 0)
        {
            if (ProjectionDistance(v, points[0], points[1]) > 0)
                currentSegment = 1;
        }

        Vector2 p = previousPos;

        bool hasTransited = false;

        while (true)
        {
            float yv, yp;

            if (currentSegment == points.Count)
            {
                yv = ProjectionDistance(v, points[currentSegment - 2], points[currentSegment - 1]);

                if (yv < 0.0f)
                {
                    if ((v - points[currentSegment - 1]).sqrMagnitude > targetSliceWidth)
                        return false;
                }
            }
            else if (currentSegment > 0)
            {
                yv = ProjectionDistance(v, points[currentSegment - 1], points[currentSegment]);
                yp = ProjectionDistance(p, points[currentSegment - 1], points[currentSegment]);

                if (yv <= 0.0f)
                {
                    if (hasTransited)
                    {
                        hasTransited = false;

                        if (yp > 0.0f)
                        {
                            if (Mathf.Abs(PerpendicularDistance(p, points[currentSegment - 1], points[currentSegment])) > targetSliceWidth)
                                return false;
                        }
                        else
                        {
                            if ((p - points[currentSegment]).sqrMagnitude > targetSliceWidth * targetSliceWidth)
                                return false;
                        }
                    }

                    if ((v - points[currentSegment - 1]).sqrMagnitude > targetSliceWidth * targetSliceWidth)
                    {
                        if ((points[currentSegment] - points[currentSegment - 1]).sqrMagnitude >
                            targetSliceWidth * targetSliceWidth)
                            return false;

                        Vector2
                            ab = v - p,
                            ac = points[currentSegment - 1] - v,
                            ak = Vector2.Dot(ac, ab) * ab / ab.sqrMagnitude;

                        p += ak + ab.normalized *
                            Mathf.Sqrt(targetSliceWidth * targetSliceWidth - (ac - ak).sqrMagnitude);

                        hasTransited = true;
                        ++currentSegment;
                        AddPoint();
                        continue;
                    }
                }
                else
                {
                    if (hasTransited)
                    {
                        hasTransited = false;

                        if (yp > 0.0f)
                        {
                            if (PerpendicularDistance(p, points[currentSegment - 1], points[currentSegment]) > targetSliceWidth)
                                return false;
                        }
                        else
                        {
                            if ((p - points[currentSegment - 1]).sqrMagnitude > targetSliceWidth * targetSliceWidth)
                                return false;
                        }
                    }

                    if (Mathf.Abs(PerpendicularDistance(v, points[currentSegment - 1], points[currentSegment])) > targetSliceWidth)
                        return false;

                    float sl = (points[currentSegment] - points[currentSegment - 1]).magnitude;
                    if (yv >= sl)
                    {
                        p += (v - p) * sl / yv;
                        ++currentSegment;
                        AddPoint();
                        continue;
                    }

                    points[currentSegment - 1] +=
                        (points[currentSegment] - points[currentSegment - 1]).normalized * yv;
                }
            }

            break;
        }

        return true;
    }

    void SetSlicePoints()
    {
        points.Clear();
        points.Add(startPoint.position);
        foreach (var p in segmentPoints)
            points.Add(p.position);
        points.Add(endPoint.position);
    }
    void StartLineDrawing(Vector2 v)
    {
        if (!lineRenderer) return;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, new Vector3(v.x, v.y, lineRenderer.transform.localPosition.z));
        SetPoint(v);
    }
    void SetPoint(Vector2 v)
    {
        if (!lineRenderer) return;

        lineRenderer.SetPosition(
            lineRenderer.positionCount - 1, new Vector3(v.x, v.y, lineRenderer.transform.localPosition.z));
    }
    void AddPoint()
    {
        if (!lineRenderer) return;

        if (lineRenderer.positionCount >= segmentPoints.Count + 2) return;

        var v = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
        lineRenderer.SetPosition(lineRenderer.positionCount - 1,
            segmentPoints[lineRenderer.positionCount - 2].position);
        ++lineRenderer.positionCount;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, v);
    }
    void ResetLineRenderer()
    {
        if (!lineRenderer) return;

        lineRenderer.positionCount = 0;
    }
    

    float ProjectionDistance(Vector2 v, Vector2 p1, Vector2 p2)
    {
        Vector2 d = p2 - p1;
        if (d == Vector2.zero) return 0.0f;
        d.Normalize();

        return Vector2.Dot(v - p1, d);
    }
    float PerpendicularDistance(Vector2 v, Vector2 p1, Vector2 p2)
    {
        Vector2 d = p2 - p1;
        if (d == Vector2.zero) return 0.0f;
        d.Normalize();
        d = Vector3.Cross(d, Vector3.forward);

        return Vector2.Dot(v - p1, d);
    }

    public void StartMethod(Vector2 input)
    {
        if (startPoint == null) return;

        isValid = true;
        state = State.Ongoing;
        CheckValidityAtStart(input);
    }
    public void UpdateMethod(Vector2 input)
    {
        if (state == State.Ongoing)
            CheckValidityAtUpdate(input);
    }
    public void EndMethod(Vector2 input)
    {
        if (state == State.Ongoing)
            CheckResultsAtEnd(input);
    }
}
