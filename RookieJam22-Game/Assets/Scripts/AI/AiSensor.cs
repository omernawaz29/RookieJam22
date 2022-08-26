using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AiSensor : MonoBehaviour
{
    [SerializeField] float distance = 10;
    [SerializeField] float angle = 30f;
    [SerializeField] float height = 1f;
    [SerializeField] Color meshColor;
    [SerializeField] int scanFrequency = 30;
    [SerializeField] LayerMask layers;
    [SerializeField] LayerMask occlusionLayers;

    [SerializeField] Image detectionMeter;

    List<GameObject> Objects = new List<GameObject>();
    Collider[] colliders = new Collider[50];
    Mesh mesh;
    int count; 
    float scanInterval = 0;
    float scanTimer = 0;

    AiAgent agent;
    GameObject playerObject;
    void Start()
    {
        agent = GetComponent<AiAgent>();
        float playerEscapeDistance = agent.teacher.config.playerEscapeDistance;
        distance = playerEscapeDistance;
        scanInterval = 1.0f / scanFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        scanTimer -= Time.deltaTime;
        if(scanTimer <= 0)
        {
            scanTimer += scanInterval;
            Scan();
        }

        if (playerObject != null && IsInSight(playerObject))
            agent.teacher.chaseTimer += Time.deltaTime;
        else if (playerObject != null)
        {
            playerObject = null;
            agent.teacher.chaseTimer = 0;
        }

        if(agent.stateMachine.currentState != AiStateId.ChasePlayer)
        {
            float alpha = agent.teacher.chaseTimer / agent.teacher.config.chaseStartTime;
            UIManager.instance.SetDetectionMeter(alpha);
        }
    }

    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layers, QueryTriggerInteraction.Collide);

        Objects.Clear();

        for (int i = 0; i < count; i++)
        {
            GameObject gobj = colliders[i].gameObject;
            if (IsInSight(gobj) && gobj.tag == "Player")
            {
                if (playerObject == null)
                    playerObject = gobj;

                if (agent.teacher.chaseTimer >= agent.teacher.config.chaseStartTime)
                {
                    agent.playerTransform = agent.playerController.GetFollowTransform();
                    agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
                }
            }

            if(gobj.tag == "Follower" && agent.stateMachine.currentState == AiStateId.ChasePlayer)
            {
                if((transform.position - gobj.transform.position).magnitude <= agent.teacher.config.playerAttackDistance)
                {
                    Debug.Log("In Follower Range");
                    agent.myAnim.SetTrigger("HitPlayer");
                }
            }
        }
    }

    public bool IsInSight(GameObject gobj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = gobj.transform.position;

        Vector3 direction = dest - origin;

        if (direction.y < 0 || direction.y > height)
            return false;


        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);

        if (deltaAngle > angle)
            return false;

        origin.y += height / 2;
        dest.y = origin.y;

        if (Physics.Linecast(origin, dest, occlusionLayers))
            return false;


        return true;
    }

    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];


        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;


        int vert = 0;


        //left side 
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        //right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;


        float currentangle = -angle;
        float deltaAngle = (angle * 2) / segments;

        for(int i = 0; i< segments; ++i)
        {
            bottomLeft = Quaternion.Euler(0, currentangle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentangle + deltaAngle, 0) * Vector3.forward * distance;

            topLeft = bottomLeft + Vector3.up * height;
            topRight = bottomRight + Vector3.up * height;

            currentangle += deltaAngle;

            //far side

            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            //top 
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            //bottom 
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;


            
        }

        


        for(int i = 0; i< numVertices; ++i)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();


        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
        scanInterval = 1.0f / scanFrequency;
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, distance);

        for (int i = 0; i< count; i++)
        {
            Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
        }

        Gizmos.color = Color.green;
        foreach (var obj in Objects)
        {
            Gizmos.DrawSphere(obj.transform.position, 0.25f);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Encountered a collider");
        if (other.tag == "SafeArea" && agent.stateMachine.currentState == AiStateId.ChasePlayer)
        {
            Debug.Log("Player in Safe Area");
            agent.teacher.chaseTimer = -10f;

            agent.Invoke("ChaseCoolDownReset", agent.teacher.config.chaseCoolDown);
            agent.stateMachine.ChangeState(AiStateId.PatrolPath);
        }
    }


    

}
