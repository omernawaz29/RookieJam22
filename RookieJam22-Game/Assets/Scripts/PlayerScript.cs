using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerScript : MonoBehaviour
{
    [SerializeField] Rigidbody myRigidbody;
    [SerializeField] Joystick myJoystick;
    [SerializeField] float mySpeed = 5f;
    [SerializeField] float myTurnSpeed = 5f;
    [SerializeField] float stackOffset = 0.1f;
    [SerializeField] float stackHeight = 5f;



    [SerializeField] Transform chalkHolder;

    Stack<Transform> collectedChalks;
    int chalkRows = 0;
    bool isInDrop = false;

    Animator myAnim;
    ChalkCollectorScript collectorScript;
    private void Start()
    {
        myAnim = GetComponentInChildren<Animator>();
        collectedChalks = new Stack<Transform>();
        chalkRows = 0;
        
    }
    private void FixedUpdate()
    {
        Vector3 newVelocity = new Vector3(myJoystick.Horizontal * mySpeed, myRigidbody.velocity.y, myJoystick.Vertical * mySpeed);


        if (Mathf.Abs(myJoystick.Vertical) > 0 || Mathf.Abs(myJoystick.Vertical) > 0)
        {

            Vector3 newDir = newVelocity.normalized;
            newDir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, newDir, Time.deltaTime * myTurnSpeed);
            myAnim.SetBool("isRunning", true);
        }
        else
        {
            myAnim.SetBool("isRunning", false);
        }

        myRigidbody.velocity = newVelocity;

    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "DropZone")
        {
            isInDrop = true;
            other.gameObject.TryGetComponent<ChalkCollectorScript>(out collectorScript);

            StartCoroutine(DropChalk(other));
        }

        if (other.gameObject.tag == "Chalk")
        {

            GameObject chalkCollected = other.gameObject;
            chalkCollected.GetComponent<Rigidbody>().isKinematic = true;

            chalkCollected.transform.SetParent(chalkHolder);
            chalkCollected.transform.DOLocalJump(new Vector3(0, (collectedChalks.Count % stackHeight) * stackOffset, chalkRows * -stackOffset), 1, 2, 0.1f);
            chalkCollected.transform.localRotation = Quaternion.identity;

            collectedChalks.Push(chalkCollected.transform);
            if (collectedChalks.Count != 0 && collectedChalks.Count % stackHeight == 0)
                chalkRows++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "DropZone")
        {
            collectorScript = null;
            isInDrop = false;
        }

        
    }


    IEnumerator DropChalk(Collider other)
    {
        while(isInDrop)
        {
            yield return new WaitForSeconds(0.25f);

            if(collectedChalks.Count > 0 && collectorScript != null)
            {
                var chalk = collectedChalks.Pop();
                chalk.parent = null;
                collectorScript.AddChalk();

                Vector3 endPos = other.transform.position + (Vector3.up * 0.5f) + (Vector3.right * UnityEngine.Random.Range(-0.5f, 0.5f)) + (Vector3.forward * UnityEngine.Random.Range(-0.5f, 0.5f));
                chalk.transform.DOJump(endPos, 1, 1, 0.1f).OnComplete(() => chalk.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.1f).OnComplete(() => Destroy(chalk.gameObject)));
       
            }
        }

        yield return null;
    }


}


