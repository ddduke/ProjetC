using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFight : MonoBehaviour
{
    public bool stuckInFight = false;
    public bool fighting = false;
    public List<GameObject> enemiesToFightOnSides = new List<GameObject>();
    public GameObject enemyToFightAhead;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isStuckInFight())
        {
            stuckInFight = true;
            fighting = true;
            GetComponent<UnitMove>().moving = false;

        }

        if(Fighting())
        {
            fighting = true;
        }
    }


    bool isStuckInFight()
    {
        
        Ground currentGround = GetComponent<UnitMove>().GetCurrentGround();
        List<Ground> groundsToCheck = new List<Ground>();
        int maxHeight = 1;
        int range = 1;
        groundsToCheck = currentGround.FindNeighborsGroundsByRange(1, maxHeight);
        List<GameObject> unitsOnGrounds = new List<GameObject>();
        unitsOnGrounds = currentGround.GetAllUnitsOnNeighborsGroundsFight(groundsToCheck);
        
        
        foreach (GameObject unit in unitsOnGrounds)
        {
            //Debug.Log(gameObject.name + " checking " + unit.name);
            if (GetComponent<UnitMove>().enemy != unit.GetComponent<UnitMove>().enemy)
            {
                //get the gameobject at put it in a list to choose for the hardest unit to fight
                return true;
            }
        }
        return false;



        //Then we get the units that are on the ground


    }

    bool Fighting()
    {

        Ground currentGround = GetComponent<UnitMove>().GetCurrentGround();
        List<Ground> groundsToCheck = new List<Ground>();
        int maxHeight = GetComponent<CombatVariables>().rangeHeight;
        int range = GetComponent<CombatVariables>().range;
        groundsToCheck = currentGround.FindNeighborsGroundsByRange(range, maxHeight);
        List<GameObject> unitsOnGrounds = new List<GameObject>();
        unitsOnGrounds = currentGround.GetAllUnitsOnNeighborsGroundsFight(groundsToCheck);


        foreach (GameObject unit in unitsOnGrounds)
        {
            //Debug.Log(gameObject.name + " checking " + unit.name);
            if (GetComponent<UnitMove>().enemy != unit.GetComponent<UnitMove>().enemy)
            {
                //get the gameobject at put it in a list to choose for the hardest unit to fight
                return true;
            }
        }
        return false;



        //Then we get the units that are on the ground


    }

    public void LaunchUnitFight()
    {
        if (GetEnemyToFightAhead() != null)
        {
            enemyToFightAhead = GetEnemyToFightAhead();
            int damagePerPeople = GetComponent<CombatVariables>().damageByPeople - enemyToFightAhead.GetComponent<CombatVariables>().defenseByPeople;
            enemyToFightAhead.GetComponent<CombatVariables>().totalHealth = enemyToFightAhead.GetComponent<CombatVariables>().totalHealth - damagePerPeople * GetComponent<CombatVariables>().people;
            Debug.Log("unit named " + gameObject.name + " is attacking unit named " + enemyToFightAhead.name + "with " + GetComponent<CombatVariables>().people + " people with "+ damagePerPeople + " damage for each");
        }
        if (GetEnemiesOnSides() != null)
        {
            enemiesToFightOnSides = GetEnemiesOnSides();

            foreach (GameObject unit in enemiesToFightOnSides)
            {
                int damagePerPeople = (int)Mathf.Ceil((GetComponent<CombatVariables>().damageByPeople - unit.GetComponent<CombatVariables>().defenseByPeople) * 0.25f);
                unit.GetComponent<CombatVariables>().totalHealth = unit.GetComponent<CombatVariables>().totalHealth - damagePerPeople * GetComponent<CombatVariables>().people;

                Debug.Log("unit named " + gameObject.name + " is attacking unit named " + unit.name + "with " + GetComponent<CombatVariables>().people + " people with " + damagePerPeople + " damage for each");

            }
        }
    }


    GameObject GetEnemyToFightAhead()
    {
        Ground currentGround = GetComponent<UnitMove>().GetCurrentGround();
        List<Ground> groundsToCheck = new List<Ground>();
        int maxHeight = GetComponent<CombatVariables>().rangeHeight;
        int range = GetComponent<CombatVariables>().range;
        groundsToCheck = currentGround.FindNeighborsGroundsByRange(range, maxHeight);
        List<GameObject> unitsOnGrounds = new List<GameObject>();
        unitsOnGrounds = currentGround.GetAllUnitsOnNeighborsGroundsFight(groundsToCheck);
        enemyToFightAhead = null;

        foreach (GameObject unit in unitsOnGrounds)
        {
            //Debug.Log(gameObject.name + " checking " + unit.name);
            if (GetComponent<UnitMove>().enemy != unit.GetComponent<UnitMove>().enemy)
            {
                //check if the unit is in front of the unit
                Vector3 relativePosition;
                relativePosition = gameObject.transform.InverseTransformPoint(unit.transform.position);
                if (relativePosition.z > 0)
                {
                    
                    if (enemyToFightAhead==null) enemyToFightAhead = unit;
                    else if (enemyToFightAhead != null && Vector3.Distance(enemyToFightAhead.transform.position,gameObject.transform.position)> Vector3.Distance(unit.transform.position,gameObject.transform.position)) enemyToFightAhead = unit;

                }
                
            }
        }

        return enemyToFightAhead;
    }

    List<GameObject> GetEnemiesOnSides()
    {
        Ground currentGround = GetComponent<UnitMove>().GetCurrentGround();
        List<Ground> groundsToCheck = new List<Ground>();
        int maxHeight = GetComponent<CombatVariables>().rangeHeight;
        int range = GetComponent<CombatVariables>().range;
        groundsToCheck = currentGround.FindNeighborsGroundsByRange(range, maxHeight);
        List<GameObject> unitsOnGrounds = new List<GameObject>();
        unitsOnGrounds = currentGround.GetAllUnitsOnNeighborsGroundsFight(groundsToCheck);
        GameObject enemyToFightOnTheRight = null;
        GameObject enemyToFightOnTheLeft = null;
        List<GameObject> enemyOnSidesList = new List<GameObject>();

        foreach (GameObject unit in unitsOnGrounds)
        {
            //Debug.Log(gameObject.name + " checking " + unit.name);
            if (GetComponent<UnitMove>().enemy != unit.GetComponent<UnitMove>().enemy)
            {
                //check if the unit is in front of the unit
                Vector3 relativePosition;
                relativePosition = gameObject.transform.InverseTransformPoint(unit.transform.position);

                if (relativePosition.z < 0.1f && relativePosition.z > -0.1f && relativePosition.x > 0)
                {
                    
                    if (enemyToFightOnTheRight == null) enemyToFightOnTheRight = unit;
                    else if (enemyToFightOnTheRight != null && Vector3.Distance(enemyToFightOnTheRight.transform.position, gameObject.transform.position) > Vector3.Distance(unit.transform.position, gameObject.transform.position)) enemyToFightOnTheRight = unit;

                }
                if (relativePosition.z < 0.1f && relativePosition.z > -0.1f && relativePosition.x < 0)
                {
                    if (enemyToFightOnTheLeft == null) enemyToFightOnTheLeft = unit;
                    else if (enemyToFightOnTheLeft != null && Vector3.Distance(enemyToFightOnTheLeft.transform.position, gameObject.transform.position) > Vector3.Distance(unit.transform.position, gameObject.transform.position)) enemyToFightOnTheLeft = unit;

                }


            }
        }
        if (enemyToFightOnTheRight == null && enemyToFightOnTheLeft == null) return null;
        else
        {
            if (enemyToFightOnTheLeft != null) enemyOnSidesList.Add(enemyToFightOnTheLeft);
            if (enemyToFightOnTheRight != null) enemyOnSidesList.Add(enemyToFightOnTheRight);
            return enemyOnSidesList;
        }
    }
}
