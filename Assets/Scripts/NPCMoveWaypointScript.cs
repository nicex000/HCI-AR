using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NPCMoveWaypointScript : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private NavMeshSurface surface;
    [SerializeField] private WaypointComponent destinationWaypoint;

    [SerializeField] private LayerMask wallMask;

    // Start is called before the first frame update
    void Start()
    {
        surface.BuildNavMesh();
        agent.SetDestination(destinationWaypoint.position);

    }

    // Update is called once per frame
    void Update()
    {
        if (destinationWaypoint.GetWaypointState()) UpdateNextWaypoint(destinationWaypoint.GetNextWaypoint());
        else if (agent.enabled && !agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    destinationWaypoint.SetWaypointState(true);
                    if (destinationWaypoint.DisableAgent)
                    {
                        SetAgentParent(true);
                        agent.enabled = false;
                    }
                }
            }
        }
    }

    public void UpdateNextWaypoint(WaypointComponent waypoint)
    {
        if (destinationWaypoint.GetNextWaypoint() == waypoint && destinationWaypoint.GetWaypointState())
        {
            destinationWaypoint = waypoint;
            AsyncOperation op = surface.UpdateNavMesh(surface.navMeshData);
            op.completed += OnCompletedUpdate;

            if (agent.enabled)
            {
                agent.SetDestination(destinationWaypoint.position);
            }
        }
    }

    private void SetAgentParent(bool atFeet)
    {
        if (!atFeet) agent.gameObject.transform.parent = this.transform;
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(agent.gameObject.transform.position, Vector3.down,
                    out hit, 100, wallMask))
            {
                agent.gameObject.transform.parent = hit.transform;
            }
            
        }
    }

    private void OnCompletedUpdate(AsyncOperation op)
    {
        if (!agent.enabled && !destinationWaypoint.DisableAgent)
        {
            SetAgentParent(false);
            agent.enabled = true;
        }
        if (agent.enabled)
        {
            agent.SetDestination(destinationWaypoint.position);
        }
    }
}
