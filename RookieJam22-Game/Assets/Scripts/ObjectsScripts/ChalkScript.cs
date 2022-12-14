using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChalkScript : MonoBehaviour
{
    [SerializeField] Collider TriggerCol;
    [SerializeField] Collider CollisionCol;


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            TriggerCol.enabled = CollisionCol.enabled = false;
        }
    }

    public void Dropped()
    {
        CollisionCol.enabled = true;
        Invoke("EnableCollectTrigger", 1f);
    }

    void EnableCollectTrigger()
    {
        TriggerCol.enabled = true;
    }
}
