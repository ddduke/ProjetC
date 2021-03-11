﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pathfinding;


public class UsefulCombatFunctions : MonoBehaviour
{

    public int combatMapMaxX = 27;
    public int combatMapMinX = -10;
    public int combatMapMaxZ = 6;
    public int combatMapMinZ = -6;
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
    /// <summary>
    /// returns a Vector3 position of formation pivot
    /// </summary>
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
            Debug.Log("Hey for maxZ regimentz is " + regiment.z);
            if(regiment.z < maxZ) maxZ = (int)(regiment.z);

        }
        Debug.Log("Hey ! MaxZ is " + maxZ);
        return -maxZ;

    }

    public int GetMinZ(string side)
    {
        //get the formation relative position and get the max Z of it
        List<Vector3> regimentsList = new List<Vector3>();
        regimentsList = FormationPivotRelativePosition(side);

        
        int maxZ = 0;
        foreach (Vector3 regiment in regimentsList)
        {
            Debug.Log("Hey for minZ regimentz is " + regiment.z);
            if (regiment.z > maxZ) maxZ = (int)(regiment.z);
        }
        Debug.Log("Hey ! MinZ is " + maxZ);
        return -maxZ;

    }
    /// <summary>
    /// correct z of a target to ensure the target is reachable for the formation pivot without going out of the frontiers of the map
    /// </summary>
    public float CorrectTargetZ(float target)
    {
        string side = GetComponent<TurnManager>().turn;
        //check if the target is reachable or recalculate it based on max
        //check if reaches the top of the map, and if it is, correct the target 
        if (target + GetMaxZ(side) > combatMapMaxZ) target = combatMapMaxZ - GetMaxZ(side);
        //check if reaches the bottom of the map, and if it is, correct the target 
        if (target + GetMinZ(side) < combatMapMinZ) target = combatMapMinZ - GetMinZ(side);
        return target;
    }

    public int GetMaxX(string side)
    {
        //get the formation relative position and get the max Z of it
        List<Vector3> regimentsList = new List<Vector3>();
        regimentsList = FormationPivotRelativePosition(side);


        int maxX = 0;
        foreach (Vector3 regiment in regimentsList)
        {
            Debug.Log("Hey for maxX regimentz is " + regiment.x);
            if (regiment.x < maxX) maxX = (int)(regiment.x);

        }
        Debug.Log("Hey ! MaxZ is " + maxX);
        return -maxX;

    }

    public int GetMinX(string side)
    {
        //get the formation relative position and get the max Z of it
        List<Vector3> regimentsList = new List<Vector3>();
        regimentsList = FormationPivotRelativePosition(side);


        int maxX = 0;
        foreach (Vector3 regiment in regimentsList)
        {
            Debug.Log("Hey for minZ regimentz is " + regiment.x);
            if (regiment.x > maxX) maxX = (int)(regiment.x);
        }
        Debug.Log("Hey ! MinZ is " + maxX);
        return -maxX;

    }

    /// <summary>
    /// correct X of a target to ensure the target is reachable for the formation pivot without going out of the frontiers of the map
    /// </summary>
    public float CorrectTargetX(float target)
    {
        string side = GetComponent<TurnManager>().turn;
        //check if the target is reachable or recalculate it based on max
        //check if reaches the top of the map, and if it is, correct the target 
        if (target + GetMaxX(side) > combatMapMaxX) target = combatMapMaxX - GetMaxX(side);
        //check if reaches the bottom of the map, and if it is, correct the target 
        if (target + GetMinX(side) < combatMapMinX) target = combatMapMinX - GetMinX(side);
        return target;
    }

    public Vector3 FormationGetNearestEnemyPositionToCharge(string side)
    {
        GetComponent<Seeker>().graphMask = GraphMask.FromGraphName("FormationGraph");
        //get all the regiments of the formation
        //get all the enemy regiments
        //simulate using seeker a path between each enemy regiment for each regiment of our formation
        //store the values in a list with the player regiment, the enemy regiment and the number of nodes to get to it
        //select the minimum distance and extract the couple enemy - player, reuse the seeker to get the path from player formation pivot
        //set the position to return to the 4 slots next to the enemy (up, down, left and right) and return the nearest one

        return new Vector3(0, 0, 0);
    }

}
