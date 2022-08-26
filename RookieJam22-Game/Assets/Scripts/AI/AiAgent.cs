using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    public AiStateMachine stateMachine;
    public AiStateId initialState;

    [HideInInspector]
    public NavMeshAgent navMeshAgent;
    [HideInInspector]
    public Transform playerTransform;
    [HideInInspector]
    public PlayerScript  playerController;
    [HideInInspector]
    public Animator myAnim;
    [HideInInspector]
    public AiTeacher teacher;
    [HideInInspector]
    public AiStudent student;

    private void Awake()
    {
        TryGetComponent<AiTeacher>(out teacher);
        TryGetComponent<AiStudent>(out student);
    }

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        myAnim = GetComponentInChildren<Animator>();

        

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = playerTransform.GetComponent<PlayerScript>();

        stateMachine = new AiStateMachine(this);

        if(teacher != null)
        {
            stateMachine.RegisterState(new AiPatrolPathState());
            stateMachine.RegisterState(new AiChasePlayerState());
        }
        else if (student != null)
        {
            stateMachine.RegisterState(new AiFollowPlayerState());
            stateMachine.RegisterState(new AiStayIdleState());
        }

        stateMachine.ChangeState(initialState);
    }

    private void Update()
    {
        stateMachine.Update();
        myAnim.SetFloat("Speed", navMeshAgent.velocity.magnitude);
    }


    public void ChaseCoolDownReset()
    {
        teacher.chaseTimer = 0f; 
    }

}
