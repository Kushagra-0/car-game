using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint[] nextWaypoints;
    
    public Transform GetNextWaypoint()
    {
        if (nextWaypoints.Length == 0) return null;
        int index = Random.Range(0, nextWaypoints.Length);
        return nextWaypoints[index].transform;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.5f);

        if (nextWaypoints != null)
        {
            Gizmos.color = Color.red;
            foreach (Waypoint wp in nextWaypoints)
            {
                if (wp != null)
                    Gizmos.DrawLine(transform.position, wp.transform.position);
            }
        }
    }
}
