using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiStayIdleState : AiState
{
    Collider[] colliders = new Collider[50];
    int count;
    float distance;

    float scanInterval = 0;
    float scanTimer = 0;


    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.stoppingDistance = 0;
        distance = agent.student.config.colliderRadius;
        count = 0;
        scanInterval = 1.0f / agent.student.scanFrequency;

        Debug.Log("Entering the stay Idle State");
    }

    public void Exit(AiAgent agent)
    {
    }

    public AiStateId GetId()
    {
        return AiStateId.StayIdle;
    }

    public void Update(AiAgent agent)
    {

        if ((agent.transform.position - agent.student.idlePosition.position).magnitude >= 0.25f)
        {
            agent.navMeshAgent.SetDestination(agent.student.idlePosition.position);
        }

        if (agent.student.isInDrop)
            return;

        scanTimer -= Time.deltaTime;
        if (scanTimer <= 0)
        {
            scanTimer += scanInterval;
            Scan(agent);
        }
    }


    private void Scan(AiAgent agent)
    {
        count = Physics.OverlapSphereNonAlloc(agent.transform.position, distance, colliders, agent.student.layers, QueryTriggerInteraction.Collide);
        for (int i = 0; i < count; i++)
        {
            GameObject gobj = colliders[i].gameObject;
            if (gobj.tag == "Player")
            {
                agent.stateMachine.ChangeState(AiStateId.FollowPlayer);
                gobj.GetComponent<PlayerScript>().AddFollower(agent.student);
            }
        }
    }
}
