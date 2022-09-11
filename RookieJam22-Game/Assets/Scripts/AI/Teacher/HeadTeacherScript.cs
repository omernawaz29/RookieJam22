using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadTeacherScript : MonoBehaviour
{
    Animator myAnim;

    private void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        myAnim.SetBool("levelEnd", LevelManager.instance.levelEnd);
    }
}
