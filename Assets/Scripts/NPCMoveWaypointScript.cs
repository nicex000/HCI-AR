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

    private Vector3 previousAgentLocalPosition;
    private Vector3 previousPosition;
    private Coroutine movingCoRef;
    private bool agentWasAlreadyDisabled;

    // Start is called before the first frame update
    void Start()
    {
        surface.BuildNavMesh();
        agent.SetDestination(destinationWaypoint.position);
        speed = agent.speed;
        previousPosition = transform.position;
        FindObjectOfType<LevelUIScript>()?.DisableRaycast();
        agentWasAlreadyDisabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (previousPosition != transform.position)
        {
            if (movingCoRef != null)
            {
                StopCoroutine(movingCoRef);
            }
            else if (!agent.enabled) agentWasAlreadyDisabled = true;

            agent.enabled = false;
            movingCoRef = StartCoroutine(UpdateMoveSurface());

        }
        else if (movingCoRef == null)
        {
            previousAgentLocalPosition = agent.transform.localPosition;
        }

        previousPosition = transform.position;

        if (destinationWaypoint == null)
        {
            FindObjectOfType<StageDestroyer>().EnableDestruction();
            FindObjectOfType<LevelUIScript>().ClearLevel();
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
    private IEnumerator UpdateMoveSurface()
    {
        yield return new WaitForSeconds(1);

        surface.BuildNavMesh();

        agent.transform.localPosition = previousAgentLocalPosition;
        if (!agentWasAlreadyDisabled) agent.enabled = true;
    }
}
