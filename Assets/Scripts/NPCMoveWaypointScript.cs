using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMoveWaypointScript : MonoBehaviour
{

    [SerializeField] private Transform pos;

    [SerializeField] private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent.SetDestination(pos.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
