using System.Collections;
using System.Collections.Generic;
//using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;
//class used to manage moving and selecting ground for every unit in combat
public class TacticsMove : MonoBehaviour
{

    public bool turn;
    public bool enemy;


    public GameObject combatScripts;
    //get all the selectable grounds in the map
    List<Ground> selectableGrounds = new List<Ground>();
    GameObject[] grounds;

    //used to stack the path for the ground
    Stack<Ground> path = new Stack<Ground>();

    //used to list all the units in formation for the formation move
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
        halfHeight = 0.001f;
        CalculateMovePerRound();
    }

    public void CalculateMovePerRound()
    {
        if (gameObject.tag == "Formation")
        {
            GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
            foreach (GameObject unit in units)
            {
                if (unit.GetComponent<CombatVariables>().inFormation) UnitsInFormation.Add(unit);
            }
            //maybe integrate the relative position of the units here ? 
            move = UnitsInFormation[0].GetComponent<CombatVariables>().moveCapacity;
            foreach (GameObject unit in UnitsInFormation)
            {
                if (move >= unit.GetComponent<CombatVariables>().moveCapacity) move = unit.GetComponent<CombatVariables>().moveCapacity;
            }
            move = move * numberOfRounds;
            GetComponent<FormationMove>().InFormationMoveCapacity = move/numberOfRounds;
        }
        else if (gameObject.tag == "Unit")
        {
            if (GetComponent<CombatVariables>().inFormation)
            {
                GameObject formation = GetComponent<UnitMove>().formation;
                move = formation.GetComponent<FormationMove>().InFormationMoveCapacity * numberOfRounds;
                GetComponent<CombatVariables>().movesPerRound = formation.GetComponent<FormationMove>().InFormationMoveCapacity;
            }
            else
            {
                move = GetComponent<CombatVariables>().moveCapacity;
                GetComponent<CombatVariables>().movesPerRound = GetComponent<CombatVariables>().moveCapacity;
                move = move * numberOfRounds;
            }
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
        LayerMask layer_mask = LayerMask.GetMask("Ground");
        RaycastHit hit;
        Ground ground = null; 
        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1, layer_mask))
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

    public int PathCount()
    {
        Ground[] tmp;
        tmp = path.ToArray();
        int number = 0;
        foreach (Ground g in tmp)
        {
            number += 1;
        }
        return number;
    }

    public void ResetPathDisplay()
    {
        Ground[] tmp;
        tmp = path.ToArray();
        foreach (Ground g in tmp)
        {
            g.path = false;
        }
    }

    public void MoveToGround(Ground ground)
    {
        path.Clear();
        moving = true;

        Ground next = ground;
        while (next!= null)
        {
            path.Push(next);
            next = next.parent;
        }
        path.Pop();
    }

    public void Move()
    {

        if (path.Count > 0)
        {
            //display the path on the map
            foreach (Ground pathGround in path)
            {
                pathGround.path = true;
            }
            //reset the display of current grounds
            foreach (GameObject ground in grounds)
            {
                ground.GetComponent<Ground>().ResetCurrent();
            }
            //GetCurrentGround();
            Ground g = path.Peek();

            //check if there is a unit on the next ground
            GameObject unitOnNextGround = g.CheckGroundUnit(g);
            if (unitOnNextGround!=null && gameObject.tag == "Unit")
            {
                StartCoroutine(WaitSecs(unitOnNextGround));
                
            }
            else
            {
                Debug.Log("there is no unit on the next target that has ended the round, cheerz " + gameObject.name);
                //no units in next ground
                Vector3 target = g.transform.position;
                //Debug.Log("target : " + g.name + "of game Object" + gameObject.name);


                //calculate the units position on top of the targeted ground
                target.y += halfHeight + g.GetComponent<Collider>().bounds.extents.y;
                //if we are distant to the target
                if (Vector3.Distance(transform.position, target) >= 0.05f)
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
                    if (enemy) heading = new Vector3(-1, 0, 0);
                    else heading = new Vector3(1, 0, 0);
                    transform.forward = heading;
                    path.Pop();
                    g.path = false;
                }

            }

        }
        else
        {
            if (gameObject.tag == "Unit")
            {
                gameObject.GetComponent<UnitMove>().actualRound = 0;
                GameObject formationlaunchnewfindselectablegrounds = gameObject.GetComponent<UnitMove>().formation;
                formationlaunchnewfindselectablegrounds.GetComponent<FormationMove>().iterationFindSelectableGrounds = 1;
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
            //ground.Reset();
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
        if (gameObject.tag == "Unit") yield return new WaitForSeconds(1);
        transform.forward = heading;
        //SMOOTH MOVING transform.position += velocity * Time.deltaTime;
        transform.position = target;
        
    }

    public bool functionWaitSecCalled = false;
    IEnumerator WaitSecs(GameObject unitOnNextGround)
    {
        Debug.Log("for info " + gameObject.name + "is blocked by " + unitOnNextGround.name + "at round " + combatScripts.GetComponent<TurnManager>().round);

        yield return new WaitForSeconds(1);
        

        //check if the unit has finished its turn, if its the case its blocked until next round and we add the moves that remains on the round to the initialpathcount to set the unit to the next round
        if (unitOnNextGround.GetComponent<UnitMove>().movementInRoundEnded && combatScripts.GetComponent<TurnManager>().round == 4 && !functionWaitSecCalled)
        {
            functionWaitSecCalled = true;
            Debug.Log("Unit moving set to false");
            moving = false;// if this is the end of the 4 rounds, the unit has to set up for next order and stop moving
            RemoveSelectableGrounds();
            GetComponent<UnitMove>().endOfMoveCausedByNewBlockingObject = true;

        }
        if (unitOnNextGround.GetComponent<UnitMove>().movementInRoundEnded && combatScripts.GetComponent<TurnManager>().round != 4 && (GetComponent<UnitMove>().actualRound + 1) == combatScripts.GetComponent<TurnManager>().round)
        {

            Debug.Log("there is a unit on the next target that has ended the round, cheerz " + gameObject.name + "see by yourself that " + unitOnNextGround.name + "has the ended round value of " + unitOnNextGround.GetComponent<UnitMove>().movementInRoundEnded);
            int movesUsed = GetComponent<UnitMove>().initialLengthOfPath - PathCount();
            int movesRemainsInRound = (int)Mathf.Floor(((GetComponent<UnitMove>().actualRound + 1) * GetComponent<CombatVariables>().movesPerRound) - movesUsed);
            Debug.Log("Moves added : " + movesRemainsInRound);
            GetComponent<UnitMove>().initialLengthOfPath += movesRemainsInRound;

        }
        //else just return void to avoid moving and wait for the unit to move
        else
        {
            Debug.Log("there is a unit on the next target that has not ended the round, cheerz " + gameObject.name + "see by yourself" + unitOnNextGround.GetComponent<UnitMove>().movementInRoundEnded);
            // do nothing
        }
    }
}

