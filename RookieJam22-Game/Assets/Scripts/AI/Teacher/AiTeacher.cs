using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTeacher : MonoBehaviour
{
    public Transform[] wayPoints;
    public Collider stickCol;
    [HideInInspector]
    public float chaseTimer = 0;
    public AiTeacherConfig config;
    [HideInInspector]
    public AiAgent agent;

    private void Start()
    {
        chaseTimer = 0;
        agent = GetComponent<AiAgent>();
        //stickCol.enabled = false;
    }

    public void UpdatePlayerTransform(Transform newTransform)
    {
        agent.playerTransform = newTransform;
    }
}
