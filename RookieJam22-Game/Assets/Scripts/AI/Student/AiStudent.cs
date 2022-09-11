using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AiStudent : MonoBehaviour
{
    [Header("Variables")]
    public LayerMask layers;
    public float scanFrequency;

    [Header("Refrences")]
    [Header("Movement and Positions")]
    public GameObject ChalkProjectile;
    public Transform StartPosition;
    public Transform HeadTeacherPos;

    [Header("Visuals")]
    public SkinnedMeshRenderer meshRenderer;
    public Material IdleMaterial;
    public Material FollowingMaterial;

    [Header("Configuration")]
    public AiStudentConfig config;

    [HideInInspector]
    public AiAgent agent;
    [HideInInspector]
    public bool isInDrop;
    [HideInInspector]
    public Vector3 idlePosition;


    private void Start()
    {
        idlePosition = StartPosition.position;
        agent = GetComponent<AiAgent>();
    }

    public void PlaceInClassRoom(Transform position)
    {
        idlePosition = position.position;
        isInDrop = true;
        agent.stateMachine.ChangeState(AiStateId.StayIdle);
        Debug.Log("Placed into Class room, at position: " + idlePosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TeacherStick")
        {

            isInDrop = true;
            agent.myAnim.SetTrigger("GetHit");
            agent.playerTransform.GetComponent<PlayerScript>().LoseFollower(this);
            agent.stateMachine.ChangeState(AiStateId.StayIdle);

            Invoke("PlayerFollowReset", 1f);
        }
    }

    private void PlayerFollowReset()
    {
        isInDrop = false;
    }

    public void ShootChalk()
    {
        var chalk = Instantiate(ChalkProjectile, transform.position + Vector3.up * 1f + transform.forward * 0.5f, Quaternion.identity);
        chalk.transform.DOJump(HeadTeacherPos.position + Vector3.up * 1.5f, Random.Range(1,2.5f), 1, 0.2f);
        chalk.transform.DORotate(new Vector3(Random.Range(-90f,90f), Random.Range(-90f, 90f), Random.Range(-90f, 90f)), 0.75f);
        Destroy(chalk, 1f);
    }


}
