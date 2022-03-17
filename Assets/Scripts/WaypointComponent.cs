using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointComponent : MonoBehaviour
{
    [SerializeField] private WaypointComponent nextWaypoint;
    [SerializeField] private bool requiresInteraction;
    [SerializeField] private bool disableAgent;

    private bool isCleared = false;

    public Vector3 position
    {
        get { return transform.position; }
    }

    public bool DisableAgent
    {
        get { return disableAgent; }
    }

    public WaypointComponent GetNextWaypoint()
    {
        return nextWaypoint;
    }

    public bool GetWaypointState()
    {
        return isCleared;
    }
    
    public void SetWaypointState(bool clearState, bool fromInteractable = false)
    {
        if (!clearState) isCleared = false;
        else if (requiresInteraction)
        {
            if (fromInteractable)
                isCleared = clearState;
        }
        else
        {
            isCleared = clearState;
        }
    }
}
