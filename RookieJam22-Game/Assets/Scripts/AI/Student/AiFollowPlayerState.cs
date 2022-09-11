using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AiFollowPlayerState : AiState
{

    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.speed = agent.student.config.runSpeed;

        agent.student.meshRenderer.materials[0].color = agent.student.FollowingMaterial.color;
        agent.student.meshRenderer.materials[1].color = agent.student.FollowingMaterial.color;

        agent.navMeshAgent.stoppingDistance = agent.student.config.playerFollowDistance;
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
