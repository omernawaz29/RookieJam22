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
        escapeDistance = agent.teacher.config.playerEscapeDistance;
        agent.teacher.chaseTimer = -10f;
        Debug.Log("Entering Chase Player State");
        agent.navMeshAgent.stoppingDistance = 1.25f;
        agent.playerController.AddTeacher(agent.teacher);
    }

    public void Exit(AiAgent agent)
    {
        agent.Invoke("ChaseCoolDownReset", agent.teacher.config.chaseCoolDown);
        agent.playerController.RemoveTeacher(agent.teacher);
        UIManager.instance.SetDetectionMeter(0);
    }

    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }

    public void Update(AiAgent agent)
    {
        agent.navMeshAgent.speed = agent.teacher.config.runSpeed;
        agent.navMeshAgent.SetDestination(agent.playerTransform.position);
        UIManager.instance.SetDetectionMeter(1);

        escapeCoolDown -= Time.deltaTime;
        if (escapeCoolDown <= 0)
            escapeDistance = agent.teacher.config.playerEscapeDistance;


        if ((agent.transform.position - agent.playerTransform.position).magnitude <= agent.teacher.config.playerAttackDistance)
        {
            //agent.StartCoroutine(HitColEnable(agent));
            agent.myAnim.SetTrigger("HitPlayer");
        }
        else if ((agent.transform.position - agent.playerTransform.position).magnitude >= escapeDistance)
        {
            Debug.Log("Player got away");
            agent.stateMachine.ChangeState(AiStateId.PatrolPath);
        }
    }

    IEnumerator HitColEnable(AiAgent agent)
    {
        agent.teacher.stickCol.enabled = true;
        yield return new WaitForSeconds(0.25f);
        agent.teacher.stickCol.enabled = false;
    }
}

