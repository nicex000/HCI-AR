using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaypointObstacleScript : MonoBehaviour
{
    [SerializeField]
    private WaypointComponent waypointToClear;

    [SerializeField] private GameObject wall;
    // Start is called before the first frame update
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("SSS");
        if (other.gameObject == wall)
        {
            waypointToClear.SetWaypointState(true, true);
            NPCMoveWaypointScript ms = FindObjectOfType<NPCMoveWaypointScript>();
            ms.UpdateNextWaypoint(waypointToClear.GetNextWaypoint());
        }
       
    }

    void OnCollisionExit(Collision other)
    {
        //if (other.gameObject == wall)
        //    waypointToClear.SetWaypointState(false);
    }
}
