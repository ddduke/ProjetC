﻿using System.Collections;
using System.Collections.Generic;
//using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
//class used to manage moving and selecting ground for every unit in combat
public class TacticsMove : MonoBehaviour
{
    //get all the selectable grounds in the map
    List<Ground> selectableGrounds = new List<Ground>();
    GameObject[] grounds;

    //used to stack the path for the ground
    Stack<Ground> path = new Stack<Ground>();

    Ground currentGround;

    //Variables for the movement of the unit
    public bool moving = false;
    public int move = 5;
    public float jumpHeight = 2;
    public float moveSpeed = 2;
    public float jumpVelocity = 4.5f;

    //to settle next
    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    float halfHeight = 0;

    bool fallingDown = false;
    bool jumpingUp = false;
    bool movingEdge = false;
    //get the point where we are on the edge
    Vector3 jumpTarget;
    
    //Init function called by the unit to get all the variables used in its function
    protected void Init()
    {
        grounds = GameObject.FindGameObjectsWithTag("Ground");
        halfHeight = GetComponent<Collider>().bounds.extents.y;

    }

    //function to set the current ground
    public void GetCurrentGround()
    {
        currentGround = GetTargetGround(gameObject);
        currentGround.current = true;

    }
    //function called to get the current ground
    public Ground GetTargetGround(GameObject target)
    {
        RaycastHit hit;
        Ground ground = null; 
        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1))
        {
            ground = hit.collider.GetComponent<Ground>();
        }

        return ground;
    }
    //lauching adjacency list for all the grounds of the map
    public void ComputeAdjacencyLists()
    {
        //GameObject[] grounds = GameObject.FindGameObjectWithTag("Ground");
        foreach (GameObject ground in grounds)
        {
            Ground g = ground.GetComponent<Ground>();
            g.FindNeighbors(jumpHeight);
        }
    }

    //set all the selectable grounds variables to "selectable" and enqueue all the paths for the target
    public void FindSelectableGrounds()
    {
        ComputeAdjacencyLists();
        GetCurrentGround();

        Queue<Ground> process = new Queue<Ground>();
        //get the current ground and set it to visited
        process.Enqueue(currentGround);
        currentGround.visited = true;

        // we get a neighbor, set the previous in parent variable to queue the path
        while (process.Count > 0)
        {
            Ground g = process.Dequeue();

            selectableGrounds.Add(g);
            g.selectable = true;

            if (g.distance < move)
            {
                foreach (Ground ground in g.adjacencyList)
                {
                    if (!ground.visited)
                    {
                        ground.parent = g;
                        ground.visited = true;
                        ground.distance = 1 + g.distance;
                        process.Enqueue(ground);
                    }
                }
            }
            
        }
    }

    public void MoveToGround(Ground ground)
    {
        path.Clear();
        ground.target = true;
        moving = true;

        Ground next = ground;
        while (next!= null)
        {
            path.Push(next);
            next = next.parent;
        }
    }

    public void Move()
    {
        if (path.Count > 0)
        {
            Ground g = path.Peek();
            Vector3 target = g.transform.position;
            Debug.Log("target :" + target);


            //calculate the units position on top of the targeted ground
            target.y += halfHeight + g.GetComponent<Collider>().bounds.extents.y;
            if (Vector3.Distance(transform.position, target)>=0.05f)
            {
                CalculateHeading(target);
                SetHorizontalVelocity();
                                
                transform.forward = heading;
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                //ground center reached 
                transform.position = target;
                path.Pop();
            }
        }
        else
        {
            RemoveSelectableGrounds();
            moving = false;
        }
    }

    protected void RemoveSelectableGrounds()
    {
        if(currentGround!=null)
        {
            currentGround.current = false;
            currentGround = null;
        }
        foreach(Ground ground in selectableGrounds)
        {
            ground.Reset();
        }

        selectableGrounds.Clear();
    }

    void CalculateHeading(Vector3 target)
    {
        heading = target - transform.position;
        heading.Normalize();
        Debug.Log("heading :" + heading);
    }

    void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }
}
