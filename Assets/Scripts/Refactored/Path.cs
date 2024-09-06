using UnityEngine;
using UnityEngine.Splines;

public class Path : MonoBehaviour
{
    public static Path instance;
    [HideInInspector] public SplineContainer splineContainer;
    private SplineInstantiate splineInstantiate;

    private bool up;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple instances of " + name);
        }
        instance = this;
        splineContainer = GetComponent<SplineContainer>();
        splineInstantiate = GetComponent<SplineInstantiate>();
    }

    private void Update()
    {
        var posOffset = splineInstantiate.MaxPositionOffset;
        float y;
        if (up)
        {
            y = posOffset.y + 0.005f;
            if (y >= 0.25f)
            {
                up = false;
            }
        }
        else
        {
            y = posOffset.y - 0.005f;
            if (y <= 0f)
            {
                up = true;
            }
        }
        splineInstantiate.MaxPositionOffset = new Vector3(posOffset.x, y, posOffset.z);
        splineInstantiate.UpdateInstances();
    }

    public Vector3 GetPointOnSpline(float t)
    {
        return splineContainer.Spline.EvaluatePosition(t);
    }
    public Vector3 GetStartPoint()
    {
        return GetPointOnSpline(0f);
    }

    public Quaternion GetStartRotation()
    {
        return Quaternion.Euler(GetPointOnSpline(0.001f) - GetStartPoint());
    }
}
