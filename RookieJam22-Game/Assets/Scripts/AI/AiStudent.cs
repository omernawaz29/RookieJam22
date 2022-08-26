using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AiStudent : MonoBehaviour
{
    public LayerMask layers;
    public float scanFrequency;

    public Transform idlePosition;
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
}
