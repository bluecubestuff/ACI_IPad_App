using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformAlignment : MonoBehaviour {


    Vector3 previousPosition;
    Vector3 persistentDelta;

    Quaternion rotationOffset;

    public Vector3 startingOrientation;

    [Range(0.0f, 1.0f)]
    public float interpolationRate;

    public bool forcedRotationEnabled;
    public Quaternion forcedRotationQuat;

    // Use this for initialization
    void Start () {
        previousPosition = transform.localPosition;
        persistentDelta = Vector3.zero;

        rotationOffset = transform.localRotation;
        startingOrientation.y = 0;

        if (startingOrientation != Vector3.zero)
        {
            transform.localRotation *= Quaternion.FromToRotation(Vector3.right, startingOrientation);
        }
	}

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaPos = transform.localPosition - previousPosition;
        deltaPos.y = 0.0f;

        if (deltaPos == Vector3.zero)
        {
            if (forcedRotationEnabled)
            {
                persistentDelta = forcedRotationQuat * Vector3.right;
                persistentDelta.y = 0.0f;
            }
        }
        else
        {
            persistentDelta = deltaPos.normalized;
        }

        if (persistentDelta != Vector3.zero)
        {
            persistentDelta.Normalize();

            transform.localRotation = Quaternion.Lerp(
                transform.localRotation * Quaternion.Inverse(rotationOffset),
                Quaternion.FromToRotation(Vector3.right, persistentDelta),
                interpolationRate)
                * rotationOffset;

            previousPosition = transform.localPosition;
        }
    }
}
