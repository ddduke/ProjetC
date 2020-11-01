using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
//class used to manage moving and selecting ground for every unit in combat
public class TacticsMove : MonoBehaviour
{
    //get all the selectable grounds in the map
    List<Ground> selectableGround = new List<Ground>();
    GameObject[] grounds;

    //used to stack the path for the ground
    Stack<Ground> path = new Stack<Ground>();

    Ground currentGround;

    //Variables for the movement of the unit
    public bool moving = false;
    public int move = 5;
    public float jumpHeight = 2;
    public float moveSpeed = 2;

    //to settle next
    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    float halfHeight = 0;
    
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

            selectableGround.Add(g);
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
    // to finish
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
}
