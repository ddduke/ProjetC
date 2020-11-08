using System.Collections;
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

    //used to list all the
    List<GameObject> UnitsInFormation = new List<GameObject>();

    Ground currentGround;

    //Variables for the movement of the unit
    public bool moving = false;
    float move;
    public float maxHeightDifference = 2;
    int numberOfRounds = 4;
    //get the speed of the velocity for smooth speed
    float moveSpeed = 2;


    //to settle next
    Vector3 velocity = new Vector3();
    Vector3 heading = new Vector3();

    float halfHeight = 0;
    //get the point where we are on the edge
    
    //Init function called by the unit to get all the variables used in its function
    protected void Init()
    {
        grounds = GameObject.FindGameObjectsWithTag("Ground");
        halfHeight = GetComponent<Collider>().bounds.extents.y;
        
        if (gameObject.tag == "Formation")
        {
            GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
            foreach (GameObject unit in units)
            {
                if (unit.GetComponent<CombatVariables>().inFormation) UnitsInFormation.Add(unit);
            }
            //maybe integrate the relative position of the units here ? 
            move = UnitsInFormation[0].GetComponent<CombatVariables>().movesPerRound;
            foreach (GameObject unit in UnitsInFormation)
            {
                if (move >= unit.GetComponent<CombatVariables>().movesPerRound) move = unit.GetComponent<CombatVariables>().movesPerRound;
            }
            move = move * numberOfRounds;
        }
        else if (gameObject.tag == "Unit")
        {
            move = GetComponent<CombatVariables>().movesPerRound;
            move = move * numberOfRounds;
        }

    }

    //function to get the current ground of the gameObject
    public Ground GetCurrentGround()
    {
        currentGround = GetTargetGround(gameObject);
        currentGround.current = true;
        return currentGround;

    }
    //function called to get the round under a gameobject
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
            g.FindNeighbors(maxHeightDifference);
        }
    }

    //set all the selectable grounds variables to "selectable" and enqueue all the paths for the target
    public void FindSelectableGroundsFormation()
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

    public void FindSelectableGroundsUnit()
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


            //calculate the units position on top of the targeted ground
            target.y += halfHeight + g.GetComponent<Collider>().bounds.extents.y;
            //if we are distant to the target
            if (Vector3.Distance(transform.position, target)>=0.05f)
            {
                CalculateHeading(target);
                SetHorizontalVelocity();
                //start coroutine to pause every move
                StartCoroutine(Move1Secs(target));
                

            }
            else
            {
                //ground center reached 
                transform.position = target;
                heading = new Vector3(1, 0, 0);
                transform.forward = heading;
                path.Pop();
                if (gameObject.tag == "Unit")
                {
                    GetComponent<UnitMove>().moveNumber = GetComponent<UnitMove>().moveNumber + 1;
                    if (GetComponent<UnitMove>().moveNumber == GetComponent<CombatVariables>().movesPerRound)
                    {
                        GetComponent<UnitMove>().actualRound += 1;
                        GetComponent<UnitMove>().moveNumber = 0;
                    }
                }
            }
        }
        else
        {
            if (gameObject.tag == "Unit")
            {
                gameObject.GetComponent<UnitMove>().moveNumber = -1;
                gameObject.GetComponent<UnitMove>().actualRound = 1;
            }
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
    }

    void SetHorizontalVelocity()
    {
        velocity = heading * moveSpeed;
    }

    IEnumerator Move1Secs(Vector3 target)
    {
        
        yield return new WaitForSeconds(1);

        transform.forward = heading;
        //SMOOTH MOVING transform.position += velocity * Time.deltaTime;
        transform.position = target;
        
    }
}

