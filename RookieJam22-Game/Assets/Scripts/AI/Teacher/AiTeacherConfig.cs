using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AiTeacherConfig : ScriptableObject
{
    public float playerEscapeDistance = 4.5f;
    public float playerAttackDistance = 0.5f;
    public float fieldOfView = 75f;
    public float chaseStartTime = 1f;
    public float chaseCoolDown = 1f;
    public float runSpeed = 4.5f;
    public float walkSpeed = 3f;
}
