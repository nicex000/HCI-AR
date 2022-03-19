using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointComponent : MonoBehaviour
{
    [SerializeField] private WaypointComponent nextWaypoint;
    [SerializeField] private bool requiresInteraction;
    [SerializeField] private bool disableAgent;
    [SerializeField] private WaypointType type = WaypointType.WAYPOINT;

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
                DoClear();
        }
        else
        {
            DoClear();
        }
    }

    private void DoClear()
    {
        if (type == WaypointType.PICKUP)
        {
            GameObject slot = GameObject.FindGameObjectWithTag("PickupSlot");
            bool hasStuff = slot == null;
            if (hasStuff) return;
            foreach (var children in GetComponentsInChildren<Transform>())
            {
                if (children.parent != this.transform) continue;
                hasStuff = true;
                children.SetParent(slot.transform);
                children.localPosition = Vector3.zero;
            }

            if (!hasStuff) return;
        }

        if (type == WaypointType.DROP_POINT)
        {
            GameObject slot = GameObject.FindGameObjectWithTag("PickupSlot");
            bool hasStuff = slot == null;
            if (hasStuff) return;
            foreach (var children in slot.GetComponentsInChildren<Transform>())
            {
                if (children.parent != slot.transform) continue;
                hasStuff = true;
                children.SetParent(transform);
                children.localPosition = Vector3.zero;
            }

            if (!hasStuff) return;
        }

        isCleared = true;
    }
}

public enum WaypointType
{
    WAYPOINT,
    PICKUP,
    DROP_POINT
}
