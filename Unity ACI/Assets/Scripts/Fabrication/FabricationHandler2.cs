using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabricationHandler2 : MonoBehaviour {

    public enum State
    {
        Idle,
        Ongoing,
        Pass,
        Fail,
    }

    public enum Method
    {
        Tap,
        Slice,
    }

    Method method;

    public State state;
    public Camera linkedCamera;
    public LineRenderer lineRenderer;
    public List<Transform> initPoints;
    public bool sliceReversable;

    public float
        targetTapRadius,
        targetTapOffset,
        targetSliceWidth;

    List<Vector2> points;
    Vector2 previous;
    bool isValid;
    int segmentIndex;

    // Use this for initialization
    void Start()
    {
        method = Method.Tap;
        state = State.Idle;
        points = new List<Vector2>();

        if (lineRenderer)
        {
            lineRenderer.useWorldSpace = true;
        }
    }

    void CheckValidityOnStart(Vector2 v)
    {
        var w = ScreenToWorld(v);
        previous = w;

        StartLineDrawing(w);

        SetPoints();

        switch (method)
        {
            case Method.Tap:
                if ((w - points[0]).sqrMagnitude > targetTapRadius * targetTapRadius)
                    isValid = false;

                break;
            case Method.Slice:
                segmentIndex = -1;
                if (!CheckValidityForSlicing(w))
                    isValid = false;
                break;
        }
    }
    void CheckValidityOnUpdate(Vector2 v)
    {
        var w = ScreenToWorld(v);

        SetPoint(w);

        if (isValid)
            switch (method)
            {
                case Method.Tap:
                    if ((w - previous).sqrMagnitude > targetTapOffset * targetTapOffset)
                        isValid = false;
                    break;
                case Method.Slice:
                    if (!CheckValidityForSlicing(w))
                        isValid = false;
                    break;
            }

        previous = w;
    }
    void CheckValidityOnEnd(Vector2 v)
    {
        var w = ScreenToWorld(v);

        if (isValid)
            switch (method)
            {
                case Method.Tap:
                    if ((w - previous).sqrMagnitude > targetTapOffset * targetTapOffset)
                        isValid = false;
                    break;
                case Method.Slice:
                    if (!CheckValidityForSlicing(w))
                        isValid = false;
                    if (segmentIndex != points.Count - 1)
                        isValid = false;
                    break;
            }

        ResetLineRenderer();

        if (isValid)
            state = State.Pass;
        else
            state = State.Fail;
    }

    bool CheckValidityForSlicing(Vector2 v)
    {
        Vector2 p = previous;
        bool hasTransited = false;

        while (true)
        {
            if (segmentIndex == -1)
            {
                if (WithinRadius(p, points[0], targetSliceWidth))
                {
                    if (!BehindPoint(p, points[0], points[1]))
                        return false;

                    segmentIndex = 0;
                    continue;
                }
                else if (sliceReversable)
                {
                    if (WithinRadius(p, points[points.Count - 1], targetSliceWidth))
                    {
                        points.Reverse();

                        if (!BehindPoint(p, points[0], points[1]))
                            return false;

                        segmentIndex = 0;
                        continue;
                    }
                }

                float d = RayCrossingCircle(p, v, points[0], targetSliceWidth);
                if (d != -1.0f)
                {
                    p += (v - p).normalized * d;

                    if (!BehindPoint(p, points[0], points[1]))
                        return false;

                    segmentIndex = 0;
                    hasTransited = true;
                    continue;
                }
                else if (sliceReversable)
                {
                    d = RayCrossingCircle(p, v, points[points.Count - 1], targetSliceWidth);

                    if (d != -1.0f)
                    {
                        p += (v - p).normalized * d;

                        points.Reverse();

                        if (!BehindPoint(p, points[0], points[1]))
                            return false;

                        segmentIndex = 0;
                        hasTransited = true;
                        continue;
                    }
                }
            }
            else if (segmentIndex < points.Count - 1)
            {
                Vector2 dir = points[segmentIndex + 1] - points[segmentIndex];

                if (hasTransited)
                {
                    if (PerpendicularAt(p, points[segmentIndex], points[segmentIndex + 1]).sqrMagnitude >
                        targetSliceWidth * targetSliceWidth)
                    {
                        if (dir.sqrMagnitude <= targetSliceWidth * targetSliceWidth)
                        {
                            ++segmentIndex;
                            AddPoint();
                            continue;
                        }

                        return false;
                    }

                    hasTransited = false;
                }

                Vector2 o = v - points[segmentIndex];

                if (Vector2.Dot(o, dir) < 0.0f)
                {
                    float d = RayLeavingCircle(p, v, points[segmentIndex], targetSliceWidth);

                    if (d != -1.0f)
                    {
                        if (d > targetSliceWidth)
                        {
                            if (dir.sqrMagnitude > targetSliceWidth * targetSliceWidth)
                                return false;

                            p += d * (v - p).normalized;
                            
                            ++segmentIndex;
                            AddPoint();
                            hasTransited = true;
                            continue;
                        }
                    }
                }
                else
                {
                    Vector2 x = PerpendicularAt(v, points[segmentIndex], points[segmentIndex + 1]);

                    if (x.sqrMagnitude > targetSliceWidth * targetSliceWidth)
                    {
                        if (dir.sqrMagnitude > targetSliceWidth * targetSliceWidth)
                            return false;

                        Vector2
                            ab = v - p,
                            db = Vector2.Dot(ab, dir) * dir / dir.sqrMagnitude,
                            ad = ab - db,
                            xc = targetSliceWidth * ad.normalized,
                            ax = points[segmentIndex] - p,
                            ac = ax + xc;

                        float ai2 = ab.sqrMagnitude * ac.sqrMagnitude / ad.sqrMagnitude;
                        Vector2
                            ai = Mathf.Sqrt(ai2) * ab.normalized,
                            ci = ai - ac;

                        if ((dir - ci).sqrMagnitude > targetSliceWidth * targetSliceWidth)
                            return false;

                        p += ai;
                        points[segmentIndex] += ci;

                        hasTransited = true;
                        ++segmentIndex;
                        AddPoint();
                        continue;
                    }
                    else
                    {
                        Vector2 y = ProjectionAt(v, points[segmentIndex], points[segmentIndex + 1]);

                        if (y.sqrMagnitude > dir.sqrMagnitude)
                        {
                            float m = y.magnitude - dir.magnitude;

                            p += m * dir.normalized;
                            hasTransited = true;
                            ++segmentIndex;
                            AddPoint();
                            continue;
                        }

                        points[segmentIndex] += y;
                    }
                }
            }
            else
            {
                Vector2 y = ProjectionAt(v, points[points.Count - 2], points[points.Count - 1]);

                if (Vector2.Dot(y, points[points.Count - 1] - points[points.Count - 2]) <= 0.0f)
                {
                    if ((v - points[points.Count - 1]).sqrMagnitude > targetSliceWidth * targetSliceWidth)
                        return false;
                }
            }

            break;
        }

        return true;
    }

    bool WithinRadius(Vector2 v, Vector2 c, float r)
    {
        return (v - c).sqrMagnitude <= r * r;
    }
    bool BehindPoint(Vector2 v, Vector2 p1, Vector2 p2)
    {
        if (p1 == p2) return true;

        return Vector2.Dot(v - p1, p2 - p1) <= 0;
    }
    float RayCrossingCircle(Vector2 a, Vector2 b, Vector2 p, float r)
    {
        Vector2
            ab = b - a,
            ap = p - a;
        float d1 = Vector2.Dot(ap, ab);

        if (d1 < 0.0f) return -1.0f;

        Vector2
            ac = d1 * ab / ab.sqrMagnitude,
            cp = ap - ac;

        float ad = ac.magnitude - Mathf.Sqrt(targetSliceWidth * targetSliceWidth - cp.sqrMagnitude);

        if (ab.sqrMagnitude >= ad * ad)
            return ad;

        return -1.0f;
    }
    float RayLeavingCircle(Vector2 a, Vector2 b, Vector2 p, float r)
    {
        if ((b - p).sqrMagnitude > r * r) return -1.0f;
        Vector2
           ab = b - a,
           ac = Vector2.Dot(p, ab) * ab / ab.sqrMagnitude;

        float cp2 = (p - a - ac).sqrMagnitude;

        if (cp2 > r * r) return -1.0f;
        
        float ad = ac.magnitude + Mathf.Sqrt(r * r - cp2);

        if (ad * ad > ab.sqrMagnitude) return -1.0f;

        return ad;
    }
    Vector2 ProjectionAt(Vector2 v, Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        if (ab == Vector2.zero) return Vector2.zero;

        return Vector2.Dot(v - a, ab) * ab / ab.sqrMagnitude;
    }
    Vector2 PerpendicularAt(Vector2 v, Vector2 a, Vector2 b)
    {
        if (b - a == Vector2.zero) return Vector2.zero;

        return v - a - ProjectionAt(v, a, b);
    }

    void SetPoints()
    {
        points.Clear();

        foreach (var p in initPoints)
        {
            if (points.Count == 0)
                points.Add(p.position);
            else if (points[points.Count - 1] != (Vector2)p.position)
                points.Add(p.position);
        }

        if (points.Count == 1)
            method = Method.Tap;
        else if (points.Count > 1)
            method = Method.Slice;
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

        if (lineRenderer.positionCount >= points.Count) return;

        var v = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
        lineRenderer.SetPosition(lineRenderer.positionCount - 1,
            points[lineRenderer.positionCount - 1]);
        ++lineRenderer.positionCount;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, v);
    }
    void ResetLineRenderer()
    {
        if (!lineRenderer) return;

        lineRenderer.positionCount = 0;
    }

    Vector2 ScreenToWorld(Vector2 v)
    {
        return linkedCamera.ScreenToWorldPoint(v);
    }

    public void StartMethod(Vector2 input)
    {
        print(Camera.main.ScreenToWorldPoint(input));
        if (state != State.Ongoing)
        {
            isValid = true;
            CheckValidityOnStart(input);
            state = State.Ongoing;
        }
    }
    public void UpdateMethod(Vector2 input)
    {
        if (state == State.Ongoing)
        {
            CheckValidityOnUpdate(input);
        }
    }
    public void EndMethod(Vector2 input)
    {
        if (state == State.Ongoing)
        {
            CheckValidityOnEnd(input);
        }
    }
}
