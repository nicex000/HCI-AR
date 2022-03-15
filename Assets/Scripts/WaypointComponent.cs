using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointComponent : MonoBehaviour
{
    [SerializeField] private WaypointComponent nextWaypoint;
    private bool isCleared;

    public WaypointComponent GetNextWaypoint()
    {
        return nextWaypoint;
    }

    public bool GetWaypointState()
    {
        return isCleared;
    }

    public Vector3 GetWaypointPosition()
    {
        return transform.position;
    }

    public void SetWaypointState(bool clearState)
    {
        isCleared = clearState;
    }
}
