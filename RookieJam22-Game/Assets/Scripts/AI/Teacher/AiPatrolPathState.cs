using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPatrolPathState : AiState
{

    int currentWayPoint = 0;
    public void Enter(AiAgent agent)
    {
        currentWayPoint = 0;
        agent.navMeshAgent.speed = agent.teacher.config.walkSpeed;
        agent.navMeshAgent.stoppingDistance = 0.2f;
    }

    public void Exit(AiAgent agent)
    {

    }

    public AiStateId GetId()
    {
        return AiStateId.PatrolPath;
    }

    public void Update(AiAgent agent)
    {

        if ((agent.transform.position - agent.teacher.wayPoints[currentWayPoint].position).magnitude >= 1.1f)
            agent.navMeshAgent.SetDestination(agent.teacher.wayPoints[currentWayPoint].position);
        else
            currentWayPoint = ++currentWayPoint % agent.teacher.wayPoints.Length;
    }
}
