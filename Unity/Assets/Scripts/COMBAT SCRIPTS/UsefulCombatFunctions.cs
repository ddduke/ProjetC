using System.Collections;
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
            if(regiment.z < maxZ) maxZ = (int)(regiment.z);

        }
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
            if (regiment.z > maxZ) maxZ = (int)(regiment.z);
        }
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

    /// <summary>
    /// correct z of a target to ensure the target is reachable for the formation pivot without going out of the frontiers of the map with Min and max input
    /// </summary>
    public float CorrectTargetZWithMinMax(float target, int min, int max)
    {
        //check if the target is reachable or recalculate it based on max
        //check if reaches the top of the map, and if it is, correct the target 
        if (target + max > combatMapMaxZ) target = combatMapMaxZ - max;
        //check if reaches the bottom of the map, and if it is, correct the target 
        if (target + min < combatMapMinZ) target = combatMapMinZ - min;
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
            if (regiment.x < maxX) maxX = (int)(regiment.x);

        }
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
            if (regiment.x > maxX) maxX = (int)(regiment.x);
        }
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

    /// <summary>
    /// correct X of a target to ensure the target is reachable for the formation pivot without going out of the frontiers of the map with Min and max input
    /// </summary>
    public float CorrectTargetXWithMinMax(float target, int min, int max)
    {
        
        //check if the target is reachable or recalculate it based on max
        //check if reaches the top of the map, and if it is, correct the target 
        if (target + max > combatMapMaxX) target = combatMapMaxX - max;
        //check if reaches the bottom of the map, and if it is, correct the target 
        if (target + min < combatMapMinX) target = combatMapMinX - min;
        return target;
    }

    public Vector3 FormationGetNearestEnemyPositionToCharge(string side)
    {
        Vector3 antiFormationPivotSide = new Vector3(0, 0, 0);
        if (side == "enemy")  antiFormationPivotSide = FormationPivot("player");
        if (side == "player")  antiFormationPivotSide = FormationPivot("enemy");

        return antiFormationPivotSide;
    }


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

    public Ground GetTargetGroundVector(Vector3 target)
    {
        LayerMask layer_mask = LayerMask.GetMask("Ground");
        RaycastHit hit;
        Ground ground = null;
        if (Physics.Raycast(target, -Vector3.up, out hit, 10, layer_mask))
        {
            ground = hit.collider.GetComponent<Ground>();
        }
        return ground;
    }


    /// <summary>
    /// Bool that return true if the selected regiment is in formation
    /// </summary>
    /// 
    public List<regimentFormationPosition> formationList;
    public bool IsInFormation(GameObject regiment)
    {
        formationList = new List<regimentFormationPosition>();
        //if the regiment is a player's one
        if(!regiment.GetComponent<CombatVariables>().enemy)
        {
            //first we get info on the actual formation pivot of the player formation, and get the latest update on what formation the player should be
            Vector3 playerFormationPivot = GetComponent<UsefulCombatFunctions>().FormationPivot("player");
            formationList = GetComponent<DisplayBackToFormation>().playerRegimentFormationPositions;
            foreach(regimentFormationPosition elmt in formationList)
            {
                if(regiment == elmt.regiment)
                {
                    Vector3 formationPosition = playerFormationPivot + elmt.relativePosition;
                    //if the actual ground of the regiment is the ground that is supposed to be in formation position, then the regiment is in formation
                    if(GetTargetGroundVector(formationPosition) == GetTargetGroundVector(regiment.transform.position))
                    {
                        formationList.Clear();
                        return true;

                    }
                }
            }

        }
        //if we talk about enemy formation
        if (regiment.GetComponent<CombatVariables>().enemy)
        {
            //first we get info on the actual formation pivot of the player formation, and get the latest update on what formation the player should be
            Vector3 playerFormationPivot = GetComponent<UsefulCombatFunctions>().FormationPivot("enemy");
            formationList = GetComponent<DisplayBackToFormation>().enemyRegimentFormationPositions;
            foreach (regimentFormationPosition elmt in formationList)
            {
                if (regiment == elmt.regiment)
                {
                    Vector3 formationPosition = playerFormationPivot + elmt.relativePosition;
                    //if the actual ground of the regiment is the ground that is supposed to be in formation position, then the regiment is in formation
                    if (GetTargetGroundVector(formationPosition) == GetTargetGroundVector(regiment.transform.position))
                    {
                        formationList.Clear();
                        return true;

                    }
                }
            }

        }
        formationList.Clear();
        //in any other cases the regiment is not on the formation position
        return false;
    }

    public bool sideInFormation(string side)
    {
        List<GameObject> tmpList = new List<GameObject>();
        //get All regiments of the choosen side
        tmpList = GetComponent<TurnManager>().GetAllUnitsBySide(side);
        foreach (GameObject tmp in tmpList)
        {
            //if one of the regiments are not in formation at this time, return false
            if (!tmp.GetComponent<CombatVariables>().inFormation) return false;
        }
        //if every regiments are in formation, return true
        return true;
    }
}
