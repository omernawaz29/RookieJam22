using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiChasePlayerState : AiState
{

    float escapeCoolDown;
    float escapeDistance;

    public void Enter(AiAgent agent)
    {
        escapeCoolDown = 1f;
        escapeDistance = 10f;
        agent.teacher.chaseTimer = -10f;
        Debug.Log("Entering Chase Player State");
        agent.navMeshAgent.stoppingDistance = 1f;
        agent.playerController.AddTeacher(agent.teacher);
    }

    public void Exit(AiAgent agent)
    {
        agent.Invoke("ChaseCoolDownReset", agent.teacher.config.chaseCoolDown);
        agent.playerController.RemoveTeacher(agent.teacher);
    }

    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }

    public void Update(AiAgent agent)
    {
        agent.navMeshAgent.speed = agent.teacher.config.runSpeed;
        agent.navMeshAgent.SetDestination(agent.playerTransform.position);


        escapeCoolDown -= Time.deltaTime;
        if (escapeCoolDown <= 0)
            escapeDistance = agent.teacher.config.playerEscapeDistance;


        if ((agent.transform.position - agent.playerTransform.position).magnitude <= agent.teacher.config.playerAttackDistance)
        {
            agent.myAnim.SetTrigger("HitPlayer");
        }
        else if ((agent.transform.position - agent.playerTransform.position).magnitude >= escapeDistance)
        {
            Debug.Log("Player got away");
            agent.stateMachine.ChangeState(AiStateId.PatrolPath);
        }
    }
}

