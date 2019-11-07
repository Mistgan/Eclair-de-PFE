using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Enemy_Movement : MonoBehaviour
{
    private Transform target;
    private Transform myTransform;

    Vector3 storeTarget;
    Vector3 newTargetPos;
    bool savePos;
    bool overrideTarget;
    Transform obstacle;
    public List<Vector3> EscapeDirections = new List<Vector3>();

    //AI Checks
    public bool chasing = false;
    public bool evading = false;

    //AI stuff
    public float rotationSpeed;
    public float maxDist;
    public float minDist;
    public float fireDist;

    //AI Speeds
    public float moveSpeed;


    //Fire
    bool canSwitch = false;
    bool waitActive = false;
    public GameObject[] shotSpawns;
    public GameObject shot;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        myTransform = this.transform;
        myTransform.position = new Vector3(Random.Range(-5000.0f, 5000.0f), Random.Range(-1000.0f, 5000.0f), Random.Range(-5000.0f, 5000.0f));
    }

    void FixedUpdate()
    {
        //Find Distance to target
        var distance = (target.position - myTransform.position).magnitude;

        if (chasing)
        {
            //Rotate to look at player
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);
            //Move towards the player
            myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;

            //Moves too far away from player
            if (distance > maxDist)
            {
                chasing = false;
            }

            //Attack if close enough
            if (distance < fireDist)
            {
                //insert attack commands here when ready
                if (!waitActive)
                {
                    StartCoroutine(Wait());
                }
                if (canSwitch)
                {
                    canSwitch = false;
                }
            }

            if (distance < minDist)
            {
                myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(myTransform.position - target.position), rotationSpeed * Time.deltaTime);
                myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (distance < maxDist)
            {
                chasing = true;
            }
        }

        ObstacleAvoidance(transform.forward, 0);


    }

    void ObstacleAvoidance(Vector3 direction, float offsetX)
    {
        RaycastHit[] hit = Rays(direction, offsetX);

        for (int i = 0; i < hit.Length - 1; i++)
        {
            //So we dont detect ourself as a hit collision
            if (hit[i].transform.root.gameObject != this.gameObject)
            {
                if (!savePos)
                {
                    storeTarget = target.position;
                    obstacle = hit[i].transform;
                    savePos = true;
                }

                FindEscapeDirections(hit[i].collider);
            }
        }

        if (EscapeDirections.Count > 0)
        {
            if (!overrideTarget)
            {
                newTargetPos = getClosests();
                overrideTarget = true;
            }
        }
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance < 5)
        {
            if (savePos)
            {
                //if we reach the target
                target.position = storeTarget;
                savePos = false;
            }
            else
            {
                //if we had waypoints
                //index++
            }
            overrideTarget = false;
            EscapeDirections.Clear();
        }
    }

    Vector3 getClosests()
    {
        Vector3 clos = EscapeDirections[0];
        float distance = Vector3.Distance(transform.position, EscapeDirections[0]);

        foreach (Vector3 dir in EscapeDirections)
        {
            float tempDistance = Vector3.Distance(transform.position, dir);
            if (tempDistance < distance)
            {
                distance = tempDistance;
                clos = dir;
            }
        }
        return clos;

    }

    void FindEscapeDirections(Collider col)
    {
        //Check for obstacles above
        RaycastHit hitUp;
        if (Physics.Raycast(col.transform.position, col.transform.up, out hitUp, col.bounds.extents.y * 2 + 5))
        {

        }
        else
        {
            //if there is something above
            Vector3 dir = col.transform.position + new Vector3(0, col.bounds.extents.y * 2 + 5, 0);

            if (!EscapeDirections.Contains(dir))
            {
                EscapeDirections.Add(dir);
            }
        }

        //Check for obstacles below
        RaycastHit hitDown;
        if (Physics.Raycast(col.transform.position, -col.transform.up, out hitDown, col.bounds.extents.y * 2 + 5))
        {

        }
        else
        {
            //if there is something below
            Vector3 dir = col.transform.position + new Vector3(0, -col.bounds.extents.y * 2 - 5, 0);

            if (!EscapeDirections.Contains(dir))
            {
                EscapeDirections.Add(dir);
            }
        }
        //Check for obstacles to the Right
        RaycastHit hitRight;
        if (Physics.Raycast(col.transform.position, col.transform.right, out hitRight, col.bounds.extents.x * 2 + 5))
        {

        }
        else
        {
            //if there is something to the right
            Vector3 dir = col.transform.position + new Vector3(col.bounds.extents.x * 2 + 5, 0, 0);

            if (!EscapeDirections.Contains(dir))
            {
                EscapeDirections.Add(dir);
            }
        }
        //Check for obstacles to the Left
        RaycastHit hitLeft;
        if (Physics.Raycast(col.transform.position, -col.transform.right, out hitLeft, col.bounds.extents.x * 2 + 5))
        {

        }
        else
        {
            //if there is something to the right
            Vector3 dir = col.transform.position + new Vector3(-col.bounds.extents.x * 2 - 5, 0, 0);

            if (!EscapeDirections.Contains(dir))
            {
                EscapeDirections.Add(dir);
            }
        }
    }

    RaycastHit[] Rays(Vector3 direction, float offsetX)
    {
        Ray ray = new Ray(transform.position + new Vector3(offsetX, 0, 0), direction);
        Debug.DrawRay(transform.position + new Vector3(offsetX, 0, 0), direction * 10 * moveSpeed, Color.red);

        float distanceToLookAhead = moveSpeed * 5;
        //Adjust 5 to proper radius around object to pick up raycast hits
        RaycastHit[] hits = Physics.SphereCastAll(ray, 5, distanceToLookAhead);

        return hits;
    }

    IEnumerator Wait()
    {
        waitActive = true;
        yield return new WaitForSeconds(2.0f);
        Fire();
        canSwitch = true;
        waitActive = false;
    }

    public void Fire()
    {
        foreach (GameObject ss in shotSpawns)
        {
            Instantiate(shot, ss.transform.position, ss.transform.rotation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Change the cube color to green.
        Debug.Log(gameObject.name + "From: " + other.name);
        Debug.Log(gameObject.name + "Enemi détruit par: " + other.name);
        Genrate_Enemy.passEnemies -= 1;
        Destroy(this.gameObject);

    }


}