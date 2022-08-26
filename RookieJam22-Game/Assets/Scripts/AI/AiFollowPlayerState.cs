using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AiFollowPlayerState : AiState
{

    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.speed = agent.student.config.runSpeed;

        agent.student.meshRenderer.materials[0].color = Color.green;
        agent.student.meshRenderer.materials[1].color = Color.green;

        agent.navMeshAgent.stoppingDistance = 1;
    }

    public void Exit(AiAgent agent)
    {
        agent.student.meshRenderer.materials[0].color = Color.white;
        agent.student.meshRenderer.materials[1].color = Color.white;


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
