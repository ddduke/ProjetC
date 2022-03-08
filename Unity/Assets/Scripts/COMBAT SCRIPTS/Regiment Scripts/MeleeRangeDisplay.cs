using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeRangeDisplay : MonoBehaviour
{
    public GameObject CombatScripts;
    void OnMouseOver()
    {
        //get the position of the regiment and its neighbors 
        Ground g = CombatScripts.GetComponent<UsefulCombatFunctions>().GetTargetGround(transform.gameObject);
        List<Ground> neighborsGrounds = new List<Ground>();
        int rng = transform.gameObject.GetComponent<CombatVariables>().range; 
        neighborsGrounds = g.StandardFindNeighborsGroundsByRange(rng, 0.5f);
        foreach (Ground grnd in neighborsGrounds)
        {
            grnd.range = true;
        }
    }

    void OnMouseExit()
    {
        //get the position of the regiment and its neighbors 
        Ground g = CombatScripts.GetComponent<UsefulCombatFunctions>().GetTargetGround(transform.gameObject);
        List<Ground> neighborsGrounds = new List<Ground>();
        int rng = transform.gameObject.GetComponent<CombatVariables>().range;
        neighborsGrounds = g.StandardFindNeighborsGroundsByRange(rng, 0.5f);    
        foreach (Ground grnd in neighborsGrounds)
        {
            grnd.range = false;
        }
    }
    
}
