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
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        surface.BuildNavMesh();
        agent.SetDestination(destinationWaypoint.position);
        speed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (destinationWaypoint == null)
        {
            FindObjectOfType<StageDestroyer>().EnableDestruction();
            return;
        }

        Debug.Log($"{(agent.destination - destinationWaypoint.position).magnitude < 0.1} with {destinationWaypoint.gameObject.name}");

        if (destinationWaypoint.GetWaypointState()) UpdateNextWaypoint(destinationWaypoint.GetNextWaypoint());
        else if (agent.enabled && !agent.pathPending && (agent.destination - destinationWaypoint.position).magnitude < 0.1)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    destinationWaypoint.SetWaypointState(true);
                    if (destinationWaypoint.DisableAgent)
                    {
                        SetAgentParent(true);
                        agent.speed = 0f;
                        agent.enabled = false;
                    }
                }
            }
        }
    }

    public void UpdateNextWaypoint(WaypointComponent waypoint)
    {
        if (destinationWaypoint != null && destinationWaypoint.GetNextWaypoint() == waypoint && destinationWaypoint.GetWaypointState())
        {
            destinationWaypoint = waypoint;
            AsyncOperation op = surface.UpdateNavMesh(surface.navMeshData);
            op.completed += OnCompletedUpdate;

            if (agent.enabled)
            {
                agent.speed = 0;
                if (destinationWaypoint)
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
            if (destinationWaypoint)
                agent.SetDestination(destinationWaypoint.position);
            agent.speed = speed;
        }
    }

    public void ForceUpdateSurface()
    {
        surface.UpdateNavMesh(surface.navMeshData);
    }
}
