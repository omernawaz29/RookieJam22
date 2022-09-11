using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AiStudentConfig : ScriptableObject
{
    public float colliderRadius = 2f;
    public float runSpeed = 4.5f;
    public float playerFollowDistance = 2f;
}
