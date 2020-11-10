using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMove : TacticsMove
{
    public GameObject formation;

    Vector3 relativeFormationPosition;
    Ground formationPivot;
    Ground unitTarget;

    public int actualRound = 1;
    int initialLengthOfPath;
    // Start is called before the first frame update
    void Start()
    {
        Init();
        formationPivot = GetTargetGround(formation);
        unitTarget = GetCurrentGround();
        relativeFormationPosition = formationPivot.transform.position - unitTarget.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.forward);
        // if the unit is not moving, check the mouse and view selectable grounds
        if (!moving)
        {
            if (!inFormation())
            {
                FindSelectableGroundsUnit();
                formationPivot = formation.GetComponent<FormationMove>().targetGround;
                unitTarget = GetUnitPositionInFormation(formationPivot, relativeFormationPosition);
                MoveToGround(unitTarget);
                initialLengthOfPath = PathCount();
            }
        }
        else
        {
            //gameObject.GetComponent<Animator>().Play("HumanoidRun");
            int movesUsed = initialLengthOfPath - PathCount();
            actualRound = (int)Mathf.Floor(movesUsed / GetComponent<CombatVariables>().movesPerRound);
            if (actualRound < combatScripts.GetComponent<TurnManager>().round) Move();
            if (PathCount() == 0) Move(); //if there is nothing to move next, just end the moving action with reset of the round
        }

    }

    bool inFormation()
    {
        formationPivot = formation.GetComponent<FormationMove>().targetGround;
        unitTarget = GetCurrentGround();
        Vector3 activeFormationPosition = formationPivot.transform.position - unitTarget.transform.position;
        if (activeFormationPosition != relativeFormationPosition) return false;
        else return true;
    }

    Ground GetUnitPositionInFormation(Ground formationGround, Vector3 direction)
    {
        //cube of 0.5 x and z and height of a maxHeightDifference to check if it is walkable
        Vector3 halfExtends = new Vector3(0.25f, 3f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(formationGround.transform.position - direction, halfExtends);
        //once we get all objects collided in the cube, we get the ground type and if it is walkable we add it to the adjacency list
        foreach (Collider item in colliders)
        {
            Ground ground = item.GetComponent<Ground>();
            if (ground != null && ground.walkable)
            {
                RaycastHit hit;

                //check if there is an object upwards of the ground
                if (!Physics.Raycast(ground.transform.position, Vector3.up, out hit, 1))
                {
                    return ground;
                }
                //check if the thing blocking the ray is a unit (moveable)
                else if (hit.transform.gameObject.tag == "Unit")
                {
                    return ground;
                }

            }
            
        }
        return null;
    }

}
