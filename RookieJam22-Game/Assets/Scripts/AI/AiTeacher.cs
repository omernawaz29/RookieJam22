using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTeacher : MonoBehaviour
{
    public Transform[] wayPoints;
    [HideInInspector]
    public float chaseTimer = 0;
    public AiTeacherConfig config;

    private void Start()
    {
        chaseTimer = 0;
    }
}
