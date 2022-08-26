using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AiStudent : MonoBehaviour
{
    public LayerMask layers;
    public float scanFrequency;

    public Transform idlePosition;
    public SkinnedMeshRenderer meshRenderer;
    public Material IdleMaterial;
    public Material FollowingMaterial;
    public AiStudentConfig config;

    [HideInInspector]
    public AiAgent agent;
    [HideInInspector]
    public bool isInDrop;


    private void Start()
    {
        agent = GetComponent<AiAgent>();
    }

    public void PlaceInClassRoom(Transform position)
    {
        idlePosition = position;
        isInDrop = true;
        agent.stateMachine.ChangeState(AiStateId.StayIdle);
        Debug.Log("Placed into Class room, at position: " + idlePosition.position);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TeacherStick")
        {
            Debug.Log("Student Got Hit");

            isInDrop = true;

            agent.playerTransform.GetComponent<PlayerScript>().LoseFollower(this);

            agent.stateMachine.ChangeState(AiStateId.StayIdle);

            Invoke("PlayerFollowReset", 1f);
        }
    }

    private void PlayerFollowReset()
    {
        isInDrop = false;
    }
}
