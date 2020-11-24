using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFight : MonoBehaviour
{
    public bool stuckInFight = false;
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
            GetComponent<UnitMove>().moving = false;

        }
    }


    bool isStuckInFight()
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
}
