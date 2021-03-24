using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMove : TacticsMove
{
    /*
    public GameObject formation;

    Vector3 relativeFormationPosition;
    Ground formationPivot;
    Ground unitTarget;
    public bool endOfMoveCausedByNewBlockingObject = false;

    public int actualRound = 1;
    public bool movementInRoundEnded = true;
    public int initialLengthOfPath;
    // Start is called before the first frame update
    void Start()
    {
        Init();
        formationPivot = GetTargetGround(formation);
        unitTarget = GetCurrentGround();
        relativeFormationPosition = formationPivot.transform.position - unitTarget.transform.position;
        movementInRoundEnded = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.forward);
        
        if (!turn)
        {
            GetComponent<UnitFight>().CheckFightingStatus();
            return;
        }
        GetCurrentGround();
        
        // if the unit is not moving, check the mouse and view selectable grounds
        if (!moving)
        {
            if (combatScripts.GetComponent<TurnManager>().round == 0)
            {
                GetComponent<UnitFight>().CheckFightingStatus();
                ResetPathDisplay();
                if (endOfMoveCausedByNewBlockingObject)
                {
                    endOfMoveCausedByNewBlockingObject = false;
                    functionWaitSecCalled = false;

                    if (GetComponent<CombatVariables>().chargeAndBreakFormation)
                    {
                        ResetPathDisplay();
                        FindSelectableGroundsUnit();
                        CalculateMovePerRound();
                        unitTarget = GetClosestEnemyUnit(gameObject.transform.position);
                        MoveToGround(unitTarget);
                        initialLengthOfPath = PathCount();
                    }
                }
                
            }
            


            else if (combatScripts.GetComponent<TurnManager>().round > 0)
            {
                GetComponent<UnitFight>().CheckFightingStatus();
                if (!GetComponent<UnitFight>().stuckInFight)
                {
                    if (!inFormation() && GetComponent<CombatVariables>().inFormation)
                    {
                        CalculateMovePerRound();
                        FindSelectableGroundsUnit();
                        formationPivot = formation.GetComponent<FormationMove>().targetGround;
                        unitTarget = GetUnitPositionInFormation(formationPivot, relativeFormationPosition);
                        MoveToGround(unitTarget);
                        initialLengthOfPath = PathCount();
                    }

                    if (GetComponent<CombatVariables>().chargeAndBreakFormation)
                    {
                        ResetPathDisplay();
                        CalculateMovePerRound();
                        FindSelectableGroundsUnit();
                        unitTarget = GetClosestEnemyUnit(gameObject.transform.position);
                        MoveToGround(unitTarget);
                        initialLengthOfPath = PathCount();
                    }
                }
                else
                {
                    //if my unit is stuck in fight, it can't move
                }
                
            }
            
        }
        else //if the unit is moving
        {
            //GetComponent<UnitFight>().CheckFightingStatus();
            GetComponent<UnitFight>().CheckFightingStatus();
            if (GetComponent<CombatVariables>().chargeAndBreakFormation && unitTarget != GetClosestEnemyUnit(gameObject.transform.position))
            {
                ResetPathDisplay();
                CalculateMovePerRound();
                int actualLengthOfPath = initialLengthOfPath - PathCount();
                FindSelectableGroundsUnit();
                unitTarget = GetClosestEnemyUnit(gameObject.transform.position);
                MoveToGround(unitTarget);
                unitTarget.target = true;
                initialLengthOfPath = PathCount() + actualLengthOfPath;
                int movesUsed = initialLengthOfPath - PathCount();
                actualRound = (int)Mathf.Floor((movesUsed + 0.01f) / GetComponent<CombatVariables>().movesPerRound);
                if (actualRound < combatScripts.GetComponent<TurnManager>().round)
                {
                    
                    movementInRoundEnded = false;
                    Move();
                }
                if (actualRound != 0 && actualRound >= combatScripts.GetComponent<TurnManager>().round)
                {
                    
                    movementInRoundEnded = true;
                    if (combatScripts.GetComponent<TurnManager>().round == 0)
                    {
                        moving = false;
                    }
                }
                if (PathCount() == 0)
                {
                    movementInRoundEnded = true;
                    Move(); //if there is nothing to move next, just end the moving action with reset of the round
                }
            }

            else
            {
                unitTarget.target = true;
                int movesUsed = initialLengthOfPath - PathCount();
                actualRound = (int)Mathf.Floor((movesUsed + 0.01f) / GetComponent<CombatVariables>().movesPerRound);
                if (actualRound < combatScripts.GetComponent<TurnManager>().round)
                {

                    movementInRoundEnded = false;
                    Move();
                }
                if (actualRound != 0 && actualRound >= combatScripts.GetComponent<TurnManager>().round)
                {

                    movementInRoundEnded = true;
                    if(combatScripts.GetComponent<TurnManager>().round==0)
                    {
                        moving = false;
                    }
                }
                if (PathCount() == 0)
                {

                    movementInRoundEnded = true;
                    Move(); //if there is nothing to move next, just end the moving action with reset of the round
                }

            }

            
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
                //check if the thing blocking the ray is a unit  or a formation(moveable)
                else if (hit.transform.gameObject.tag == "Unit" || hit.transform.gameObject.tag == "Formation")
                {
                    return ground;
                }

            }
            
        }
        Debug.Log("unit target is null");
        return null;
    }

    Ground GetClosestEnemyUnit(Vector3 position)
    {
        List<GameObject> enemyList = new List<GameObject>();
        if (enemy) enemyList = combatScripts.GetComponent<TurnManager>().GetAllUnitsBySide("player");
        else enemyList = combatScripts.GetComponent<TurnManager>().GetAllUnitsBySide("enemy");

        float distance = Vector3.Distance(position, enemyList[0].transform.position);
        GameObject lowestDistanceEnemy = enemyList[0];
        foreach(GameObject enemy in enemyList)
        {
            if (Vector3.Distance(position, enemy.transform.position) < distance) lowestDistanceEnemy = enemy;
        }
        Ground CurrentLowestDistanceEnemyPosition = GetTargetGround(lowestDistanceEnemy);
        return CurrentLowestDistanceEnemyPosition;

    }
    */

}
