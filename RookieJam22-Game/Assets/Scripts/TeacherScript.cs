using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeacherScript : MonoBehaviour
{
    NavMeshAgent myAgent;


    [SerializeField] bool chasingPlayer;
    [SerializeField] float walkSpeed = 1f;
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float playerEscapeDistance = 4.5f;

    [SerializeField] Transform player;
    [SerializeField] Transform[] wayPoints;
 


    
    Animator myAnim;
    int currentWayPoint = 0;

    private void Start()
    {
        currentWayPoint = 0;

        myAnim = GetComponentInChildren<Animator>();
        myAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (myAgent.isStopped && !chasingPlayer)
            Idle();
        else if (chasingPlayer && !myAgent.isStopped)
            ChasePlayer();
        else
            FollowPath();

        
    }

    void Idle()
    {
        myAnim.SetBool("isRunning", false);
        myAnim.SetBool("isWalking", false);
        myAgent.speed = walkSpeed;
    }

    void FollowPath()
    {

        myAnim.SetBool("isWalking", true);
        myAnim.SetBool("isRunning", false);
        myAgent.speed = walkSpeed;

        if ((transform.position - wayPoints[currentWayPoint].position).magnitude >= 0.25f)
            myAgent.SetDestination(wayPoints[currentWayPoint].position);
        else
            currentWayPoint = ++currentWayPoint % wayPoints.Length;
    }


    void ChasePlayer()
    {

        myAgent.speed = runSpeed;
        myAnim.SetBool("isRunning", true);
        myAnim.SetBool("isWalking", false);

        myAgent.SetDestination(player.position);

        if ((transform.position - player.position).magnitude <= 0.5f)
            Debug.Log("Try Hit Player");
        else if ((transform.position - player.position).magnitude >= playerEscapeDistance)
        {
            Debug.Log("Player got away");
            chasingPlayer = false;
        }
    }

}
