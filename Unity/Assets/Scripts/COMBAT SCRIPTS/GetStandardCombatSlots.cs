using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetStandardCombatSlots : MonoBehaviour
{
    public GameObject CombatScripts;
    // Start function call the combat Scripts to serialize it into the function 
    public void Start()
    {
        CombatScripts = GameObject.Find("CombatScripts").transform.gameObject;
    }

    public bool PreferedCombatSlotsExists()
    {
        return false;
    }

    public bool CombatSlotsExists()
    {

        if (GetCombatSlots().Count > 0) return true;
        else return false;
    }
   
    public List<Ground> GetCombatSlots()
    {

        List<GameObject> regimentsList = new List<GameObject>();
        //get the opposite side list
        if (!GetComponent<CombatVariables>().enemy) regimentsList = CombatScripts.GetComponent<TurnManager>().GetAllUnitsBySide("enemy");
        else regimentsList = CombatScripts.GetComponent<TurnManager>().GetAllUnitsBySide("player");
        List<Ground> possiblePositions = new List<Ground>();
        foreach (GameObject reg in regimentsList)
        {
            //get the position of the regiment and its neighbors (to update in case of differents 
            Ground g = CombatScripts.GetComponent<UsefulCombatFunctions>().GetTargetGround(reg);
            List<Ground> neighborsGrounds = new List<Ground>();
            neighborsGrounds = g.StandardFindNeighborsGroundsByRange(1, 0.5f);


            //check if there is an obstacle, if not store it into possible positions
            
            foreach(Ground gr in neighborsGrounds)
            {
                if (gr.EmptyGround(gr)) possiblePositions.Add(gr);
            }

        }
        return possiblePositions;

    }

    public List<Ground> GetDiagonalNextPositionsToCombatSlots()
    {

        List<GameObject> regimentsList = new List<GameObject>();
        //get the opposite side list
        if (!GetComponent<CombatVariables>().enemy) regimentsList = CombatScripts.GetComponent<TurnManager>().GetAllUnitsBySide("enemy");
        else regimentsList = CombatScripts.GetComponent<TurnManager>().GetAllUnitsBySide("player");
        List<Ground> possiblePositions = new List<Ground>();
        foreach (GameObject reg in regimentsList)
        {
            //get the position of the regiment and its neighbors (to update in case of differents 
            Ground g = CombatScripts.GetComponent<UsefulCombatFunctions>().GetTargetGround(reg);
            List<Ground> neighborsGrounds = new List<Ground>();
            neighborsGrounds = g.DiagonalFindNeighborsGroundsByRange(1, 0.5f);


            //check if there is an obstacle, if not store it into possible positions

            foreach (Ground gr in neighborsGrounds)
            {
                if (gr.EmptyGround(gr)) possiblePositions.Add(gr);
                else
                {
                    Debug.Log("ground not available");
                }
            }
        }

        return possiblePositions;
    }

    public List<Ground> GetStraight2ndNextPositionsToCombatSlots()
    {
        List<GameObject> regimentsList = new List<GameObject>();
        //get the opposite side list
        if (!GetComponent<CombatVariables>().enemy) regimentsList = CombatScripts.GetComponent<TurnManager>().GetAllUnitsBySide("enemy");
        else regimentsList = CombatScripts.GetComponent<TurnManager>().GetAllUnitsBySide("player");
        List<Ground> possiblePositions = new List<Ground>();
        foreach (GameObject reg in regimentsList)
        {
            //get the position of the regiment and its neighbors (to update in case of differents 
            Ground g = CombatScripts.GetComponent<UsefulCombatFunctions>().GetTargetGround(reg);
            List<Ground> neighborsGrounds = new List<Ground>();
            neighborsGrounds = g.StandardFindNeighborsGroundsByRange(2, 0.5f);


            //check if there is an obstacle, if not store it into possible positions

            foreach (Ground gr in neighborsGrounds)
            {
                if (gr.EmptyGround(gr)) possiblePositions.Add(gr);
            }
        }

        return possiblePositions;
    }

    public List<Ground> GetDiagonal2ndNextPositionsToCombatSlots()
    {
        List<GameObject> regimentsList = new List<GameObject>();
        //get the opposite side list
        if (!GetComponent<CombatVariables>().enemy) regimentsList = CombatScripts.GetComponent<TurnManager>().GetAllUnitsBySide("enemy");
        else regimentsList = CombatScripts.GetComponent<TurnManager>().GetAllUnitsBySide("player");
        List<Ground> possiblePositions = new List<Ground>();
        
        foreach (GameObject reg in regimentsList)
        {
            //get the position of the regiment and its neighbors (to update in case of differents 
            Ground g = CombatScripts.GetComponent<UsefulCombatFunctions>().GetTargetGround(reg);
            List<Ground> neighborsGrounds = new List<Ground>();
            neighborsGrounds = g.DiagonalFindNeighborsGroundsByRange(2, 0.5f);


            //check if there is an obstacle, if not store it into possible positions

            foreach (Ground gr in neighborsGrounds)
            {
                if (gr.EmptyGround(gr)) possiblePositions.Add(gr);
            }
        }
        
        return possiblePositions;

    }



}
