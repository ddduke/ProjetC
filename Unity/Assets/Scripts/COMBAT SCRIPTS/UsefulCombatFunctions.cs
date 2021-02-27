using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UsefulCombatFunctions : MonoBehaviour
{
    /// <summary>
    /// returns the list of the regiments relative positions to formation pivot
    /// </summary>
    public List<Vector3> FormationPivotRelativePosition(string side)
    {
        //get all regiments by side 
        List<GameObject> regimentsList = new List<GameObject>();
        regimentsList = GetComponent<TurnManager>().GetAllUnitsBySide(side);


        //set the relative position to formation pivot to all regiments
        Vector3 formationPivot = FormationPivot(side);
        List<Vector3> regimentsRelativePosition = new List<Vector3>();
        foreach (GameObject regiment in regimentsList)
        {
            Vector3 relativePosition = formationPivot - regiment.transform.position;
            regimentsRelativePosition.Add(relativePosition);
        }

        return regimentsRelativePosition;
    }
    
    public Vector3 FormationPivot(string side)
    {
        //get all regiments by side 
        List<GameObject> regimentsList = new List<GameObject>();
        regimentsList = GetComponent<TurnManager>().GetAllUnitsBySide(side);

        //Get the formation pivot (center), represented by the max X (right of the formation) and average z (vertical center of the formation)
        //first get the max z
        float maxX = 0;
        if (side == "player")
        {
            maxX = -100;
            foreach (GameObject regiment in regimentsList)
            {
                if (regiment.transform.position.x > maxX) maxX = regiment.transform.position.x;
            }
        }
        else // side is enemy
        {
            maxX = 100;
            foreach (GameObject regiment in regimentsList)
            {
                if (regiment.transform.position.x < maxX) maxX = regiment.transform.position.x;
            }
        }


        //then get the average z and set round it to put it on the grid
        List<float> regimentsZ = new List<float>();
        foreach (GameObject regiment in regimentsList)
        {
            regimentsZ.Add(regiment.transform.position.z);
        }
        float averageZ = regimentsZ.Average();

        return new Vector3(Mathf.Round(maxX), 1, Mathf.Round(averageZ));
    }

    public int GetMaxZ(string side)
    {
        //get the formation relative position and get the max Z of it
        List<Vector3> regimentsList = new List<Vector3>();
        regimentsList = FormationPivotRelativePosition(side);
        
        
        int maxZ = 0;
        foreach (Vector3 regiment in regimentsList)
        {
            if(regiment.z > maxZ) maxZ = (int)(regiment.z);
        }

        return maxZ;

    }

    public int GetMinZ(string side)
    {
        //get the formation relative position and get the max Z of it
        List<Vector3> regimentsList = new List<Vector3>();
        regimentsList = FormationPivotRelativePosition(side);


        int maxZ = 0;
        foreach (Vector3 regiment in regimentsList)
        {
            if (regiment.z < maxZ) maxZ = (int)(regiment.z);
        }

        return maxZ;

    }
}
