using UnityEngine;

public class AICar : MonoBehaviour
{
    [Header("Driving")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float steeringSpeed = 5f;
    [SerializeField] private float waypointReachDist = 4f;
    [SerializeField] private float lookAheadDist = 6f;

    [Header("Obstacle Avoidance")]
    [SerializeField] private float stopDistance = 3f;
    [SerializeField] private float rayAngle = 45f;
    [SerializeField] private float rayLength = 6f;

    [Header("Stuck Handling")]
    [SerializeField] private float stuckTimeLimit = 2f;
    [SerializeField] private float minMoveDistance = 0.5f;

    [SerializeField] private AIWheelContoller wheelController;

    private Rigidbody rb;
    private Transform targetWaypoint;
    private float stuckTimer = 0f;
    private Vector3 lastPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Pick closest waypoint instead of random → makes paths consistent
        Waypoint[] allWaypoints = FindObjectsOfType<Waypoint>();
        float minDist = Mathf.Infinity;
        foreach (var w in allWaypoints)
        {
            float d = Vector3.Distance(transform.position, w.transform.position);
            if (d < minDist)
            {
                minDist = d;
                targetWaypoint = w.transform;
            }
        }

        lastPos = transform.position;
    }

    void FixedUpdate()
    {
        if (targetWaypoint == null) return;

        Vector3 targetPos = targetWaypoint.position;
        targetPos.y = transform.position.y;

        // Look ahead for smoother turning
        Vector3 dirToWaypoint = (targetPos - transform.position).normalized;
        Vector3 lookAhead = targetPos + dirToWaypoint * lookAheadDist;
        Vector3 dir = (lookAhead - transform.position).normalized;

        Vector3 finalDir = dir;

        // --- Obstacle Avoidance ---
        if (ObstacleCheck(out Vector3 avoidDir))
        {
            finalDir = Vector3.Lerp(dir, avoidDir, 0.7f).normalized; // blend avoidance with goal
        }

        // --- Steering Input ---
        float steerInput = Vector3.SignedAngle(transform.forward, finalDir, Vector3.up) / 45f;
        steerInput = Mathf.Clamp(steerInput, -1f, 1f);

        // --- Movement ---
        Vector3 move = transform.forward * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        Quaternion targetRot = Quaternion.LookRotation(finalDir);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, steeringSpeed * Time.fixedDeltaTime));

        wheelController.UpdateSteering(steerInput);

        // --- Waypoint Handling ---
        if (Vector3.Distance(transform.position, targetWaypoint.position) < waypointReachDist)
        {
            SwitchToNextWaypoint();
        }

        CheckStuck();
    }

    private void SwitchToNextWaypoint()
    {
        if (targetWaypoint == null) return;

        Waypoint wp = targetWaypoint.GetComponent<Waypoint>();
        if (wp != null && wp.GetNextWaypoint() != null)
        {
            targetWaypoint = wp.GetNextWaypoint();
        }
    }

    private bool ObstacleCheck(out Vector3 avoidanceDir)
    {
        avoidanceDir = transform.forward;
        Vector3 origin = transform.position + Vector3.up;

        bool frontBlocked = Physics.Raycast(origin, transform.forward, out _, stopDistance);
        bool leftClear = !Physics.Raycast(origin, Quaternion.AngleAxis(-rayAngle, Vector3.up) * transform.forward, rayLength);
        bool rightClear = !Physics.Raycast(origin, Quaternion.AngleAxis(rayAngle, Vector3.up) * transform.forward, rayLength);

        if (frontBlocked)
        {
            if (leftClear && !rightClear) avoidanceDir = Quaternion.AngleAxis(-rayAngle, Vector3.up) * transform.forward;
            else if (rightClear && !leftClear) avoidanceDir = Quaternion.AngleAxis(rayAngle, Vector3.up) * transform.forward;
            else if (leftClear && rightClear) avoidanceDir = Random.value > 0.5f ?
                Quaternion.AngleAxis(-rayAngle, Vector3.up) * transform.forward :
                Quaternion.AngleAxis(rayAngle, Vector3.up) * transform.forward;
            else avoidanceDir = -transform.forward; // dead end
            return true;
        }

        return false;
    }

    private void CheckStuck()
    {
        float moved = Vector3.Distance(transform.position, lastPos);

        if (moved < minMoveDistance)
        {
            stuckTimer += Time.fixedDeltaTime;
            if (stuckTimer > stuckTimeLimit)
            {
                SwitchToNextWaypoint();
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }

        lastPos = transform.position;
    }
}
