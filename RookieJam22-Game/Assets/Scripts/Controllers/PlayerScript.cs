using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerScript : MonoBehaviour
{

    [Header("References")]
    [SerializeField] Rigidbody myRigidbody;
    [SerializeField] Joystick myJoystick;
    [SerializeField] Transform chalkHolder;
    [SerializeField] Transform groundCheck;

    [Header("Movement Variables")]
    [SerializeField] float mySpeed = 5f;
    [SerializeField] float myTurnSpeed = 5f;
    [SerializeField] float stepOffset = 0.5f;
    [SerializeField] LayerMask stepMask;

    [Header("Stacking Variables")]
    [SerializeField] float stackOffset = 0.1f;
    [SerializeField] float stackHeight = 5f;
    [SerializeField] float dropChalksOnHit = 3f;


    

    Stack<Transform> collectedChalks;
    List<AiStudent> followingStudents;
    List<AiTeacher> chasingTeachers;
    int chalkRows = 0;
    bool isInDrop = false;

    Animator myAnim;
    ChalkCollectorScript chalkCollector;
    FollowersCollectorScript followerCollector;

    private bool firstTouch = true;

    private void Start()
    {
        myAnim = GetComponentInChildren<Animator>();
        collectedChalks = new Stack<Transform>();
        followingStudents = new List<AiStudent>();
        chasingTeachers = new List<AiTeacher>();

        chalkRows = 0;

        firstTouch = true;

    }
    private void FixedUpdate()
    {

        if (LevelManager.instance.levelEnd)
        {
            myRigidbody.velocity = Vector3.zero;
            myAnim.SetFloat("Speed", myRigidbody.velocity.magnitude);
            return;
        }

        myAnim.SetFloat("Speed", myRigidbody.velocity.magnitude);
        Vector3 newVelocity = new Vector3(myJoystick.Horizontal * mySpeed, myRigidbody.velocity.y, myJoystick.Vertical * mySpeed);


        if (Mathf.Abs(myJoystick.Vertical) > 0 || Mathf.Abs(myJoystick.Horizontal) > 0)
        {
            if (firstTouch)
            {
                firstTouch = false;
                LevelManager.instance.FirstTouch();
            }
                    
            Vector3 newDir = newVelocity.normalized;
            newDir.y = 0;
            transform.forward = Vector3.Lerp(transform.forward, newDir, Time.deltaTime * myTurnSpeed);
        }

        myRigidbody.velocity = newVelocity;
        HandleSteps();
    }
    private void OnCollisionEnter(Collision collision)
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ChalkDrop")
        {
            isInDrop = true;
            other.gameObject.TryGetComponent<ChalkCollectorScript>(out chalkCollector);

            StartCoroutine(DropChalk(other));
        }
        if (other.tag == "FollowerDrop")
        {
            other.gameObject.TryGetComponent<FollowersCollectorScript>(out followerCollector);
            DepositFollowers();
        }

        if (other.gameObject.tag == "Chalk")
        {

            GameObject chalkCollected = other.gameObject;
            chalkCollected.GetComponent<Rigidbody>().isKinematic = true;

            chalkCollected.transform.SetParent(chalkHolder);
            chalkCollected.transform.DOLocalRotate(new Vector3(0, 180, 0), 1f);
            chalkCollected.transform.DOLocalJump(new Vector3(0, (collectedChalks.Count % stackHeight) * stackOffset, chalkRows * -stackOffset), 2, 1, 0.5f);
            chalkCollected.transform.localRotation = Quaternion.identity;

            collectedChalks.Push(chalkCollected.transform);
            if (collectedChalks.Count != 0 && collectedChalks.Count % stackHeight == 0)
                chalkRows++;

            UIManager.instance.UpdateChalkCount(collectedChalks.Count);
        }

        if (other.tag == "TeacherStick")
        {
            Debug.Log("Got HIT");
            myAnim.SetTrigger("GetHit");
            UIManager.instance.ShowHitText();
            StartCoroutine(DropChalksOnHit());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ChalkDrop")
        {
            chalkCollector = null;
            isInDrop = false;
        }
    }


    void HandleSteps()
    {
        if(Physics.Raycast(groundCheck.position, transform.forward, 0.25f))
        {
            if(!Physics.Raycast(groundCheck.position + Vector3.up * stepOffset, transform.forward, 0.25f + 0.1f))
            {
                transform.position += Vector3.up * 0.1f;
            }
        }
    }

    IEnumerator DropChalk(Collider other)
    {
        while (isInDrop)
        {
            yield return new WaitForSeconds(0.1f);

            if (collectedChalks.Count > 0 && chalkCollector != null)
            {
                var chalk = collectedChalks.Pop();
                chalk.parent = null;
                chalkCollector.AddChalk();

                Vector3 endPos = other.transform.position + (Vector3.up * 0.5f) + (Vector3.right * UnityEngine.Random.Range(-0.5f, 0.5f)) + (Vector3.forward * UnityEngine.Random.Range(-0.5f, 0.5f));
                chalk.transform.DOJump(endPos, 1, 1, 0.1f).OnComplete(() => chalk.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.1f).OnComplete(() => Destroy(chalk.gameObject)));
                UIManager.instance.UpdateChalkCount(collectedChalks.Count);
            }
        }

        yield return null;
    }
    IEnumerator DropChalksOnHit()
    {
        for (int i = 0; i < dropChalksOnHit && collectedChalks.Count > 0; i++)
        {
            var chalk = collectedChalks.Pop();
            var chalkRB = chalk.GetComponent<Rigidbody>();

            chalk.parent = null;


            chalk.GetComponent<ChalkScript>().Dropped();
            chalkRB.isKinematic = false;
            chalkRB.AddForce(Vector3.up * 5 + transform.right * UnityEngine.Random.Range(-5f, 5f), ForceMode.Impulse);
            UIManager.instance.UpdateChalkCount(collectedChalks.Count);
        }
        yield return null;
    }
    void DepositFollowers()
    {
        while (followingStudents.Count > 0)
        {
            var follower = followingStudents[followingStudents.Count - 1];
            followingStudents.Remove(follower);
            UpdateChaserFollowTargets();
            follower.PlaceInClassRoom(followerCollector.GetFollowerIdlePosition());
            UIManager.instance.UpdateFollowerCount(followingStudents.Count);
            followerCollector.AddFollower();
        }
    }
    public void AddFollower(AiStudent student)
    {
        student.agent.playerTransform = GetFollowTransform();
        followingStudents.Add(student);
        UIManager.instance.UpdateFollowerCount(followingStudents.Count);

        UpdateChaserFollowTargets();

    }
    public void LoseFollower(AiStudent follower)
    {
        int index = followingStudents.IndexOf(follower);
        followingStudents.Remove(follower);
        for (int i = 0; i < followingStudents.Count; i++)
        {
            if(i == 0)
                followingStudents[i].agent.playerTransform = transform;
            else
                followingStudents[i].agent.playerTransform = followingStudents[i - 1].transform; 
        }
        UIManager.instance.UpdateFollowerCount(followingStudents.Count);
        UpdateChaserFollowTargets();
    }
    public Transform GetFollowTransform()
    {
        if (followingStudents.Count > 0)
            return followingStudents[followingStudents.Count - 1].transform;
        else
            return transform;
    }
    public void AddTeacher(AiTeacher teacher)
    {
        chasingTeachers.Add(teacher);
    }
    public void RemoveTeacher(AiTeacher teacher)
    {
        chasingTeachers.Remove(teacher);
    }
    void UpdateChaserFollowTargets()
    {
        foreach( AiTeacher t in chasingTeachers)
        {
            t.UpdatePlayerTransform(GetFollowTransform());
        }
    }

}


