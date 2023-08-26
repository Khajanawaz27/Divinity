using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomScenePhysics : MonoBehaviour
{
    public Vector3 customGravityDirection = new Vector3(0, 0, 10f);
    public float customBounceThreshold = 0f;
    public float customSleepThreshold = 1e-06f;
    public float customContactOffset = 0.0001f;
    public int customSolverIterations = 10;
    public bool customAutoSyncTransform = true;
    public bool customReuseCollisionCallbacks = false;

    private void Awake()
    {
        Physics.gravity = customGravityDirection;
        Physics.bounceThreshold = customBounceThreshold;
        Physics.sleepThreshold = customSleepThreshold;
        Physics.defaultContactOffset = customContactOffset;
        Physics.defaultSolverIterations = customSolverIterations;
        Physics.autoSyncTransforms = customAutoSyncTransform;
        Physics.reuseCollisionCallbacks = customReuseCollisionCallbacks;
    }
}
