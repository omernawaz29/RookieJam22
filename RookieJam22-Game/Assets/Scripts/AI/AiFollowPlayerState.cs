using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AiFollowPlayerState : AiState
{

    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.speed = agent.student.config.runSpeed;
        agent.navMeshAgent.stoppingDistance = 1;
    }

    public void Exit(AiAgent agent)
    {
    }

    public AiStateId GetId()
    {
        return AiStateId.FollowPlayer;
    }

    public void Update(AiAgent agent)
    {
        agent.navMeshAgent.SetDestination(agent.playerTransform.position);
    }

}
