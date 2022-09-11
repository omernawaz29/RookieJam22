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

    float shootCoolDown = 0.75f;
    float shootTimer = 0f;
    public void Enter(AiAgent agent)
    {
        agent.student.meshRenderer.materials[0].color = agent.student.IdleMaterial.color;
        agent.student.meshRenderer.materials[1].color = agent.student.IdleMaterial.color;

        agent.navMeshAgent.stoppingDistance = 0;
        distance = agent.student.config.colliderRadius;
        count = 0;
        scanInterval = 1.0f / agent.student.scanFrequency;


        shootCoolDown = 0.75f;
        shootTimer = 0f;
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

        if ((agent.transform.position - agent.student.idlePosition).magnitude >= 0.25f)
        {
            agent.navMeshAgent.SetDestination(agent.student.idlePosition);
        }

        if (LevelManager.instance.levelEnd && agent.navMeshAgent.remainingDistance <= 0.15f )
        {
            
            shootTimer -= Time.deltaTime;
            if(shootTimer <= 0f)
            {
                agent.student.ShootChalk();
                agent.myAnim.SetTrigger("HitPlayer");
                agent.transform.LookAt(agent.student.HeadTeacherPos.position);
                shootTimer = shootCoolDown;
                shootCoolDown = Random.Range(0.75f, 1.5f);

            }
            
        }
        else if (agent.student.isInDrop || LevelManager.instance.levelEnd)
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
