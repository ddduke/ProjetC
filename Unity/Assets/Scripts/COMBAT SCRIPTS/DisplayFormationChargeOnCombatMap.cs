using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayFormationChargeOnCombatMap : MonoBehaviour
{
    // Start is called before the first frame update
    
    
        //get the formation pivot of team
        //Create the formation slots
        //get the nearest enemy
        //get the available positions adjacent to this enemy(write it in useful combat functions)
        //Display path to the nearest enemy
    

    public GameObject RegimentSlot;
    public Vector3 target;
    public Vector3 newTarget;
    public Camera cam;
    public GameObject DisplayPath;
    //public Vector3 formationPivot;
    public string side;
    public GameObject PathInstantiated;

    void Start()
    {
        side = GetComponent<TurnManager>().turn;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        target = GetComponent<UsefulCombatFunctions>().FormationGetNearestEnemyPositionToCharge(side);
        target.z = GetComponent<UsefulCombatFunctions>().CorrectTargetZ(target.z);
        target.x = GetComponent<UsefulCombatFunctions>().CorrectTargetX(target.x);

        Vector3 formationPivot = GetComponent<UsefulCombatFunctions>().FormationPivot(side);
        DisplayFormation(side, target);
        GameObject PathInstantiated = Instantiate(DisplayPath, formationPivot, Quaternion.identity);

        // Get the max speed for these units
        //get all regiments by side 
        List<GameObject> regimentsList = new List<GameObject>();
        regimentsList = GetComponent<TurnManager>().GetAllUnitsBySide(side);
        //get the minimum moves per round in the team
        int minMovesPerRound = 100;
        foreach (GameObject reg in regimentsList)
        {
            if ((int)reg.GetComponent<CombatVariables>().moveCapacityRegStat < minMovesPerRound) minMovesPerRound = (int)reg.GetComponent<CombatVariables>().moveCapacityRegStat;
            reg.GetComponent<CombatVariables>().inFormation = true;
            
        }
        PathInstantiated.GetComponent<PathVariables>().movesPerRound = minMovesPerRound;
        //set the target & the graph to use for the seeker (inclue layer regiments)
        PathInstantiated.GetComponent<PathVariables>().dynamicTarget = false;
        PathInstantiated.GetComponent<PathVariables>().staticTarget = target;
        PathInstantiated.GetComponent<PathVariables>().GraphStringToUse = "FormationGraph";

    }

    void Update()
    {
        //also check if the path is available and store it into each unit
        List<GameObject> regimentsList = new List<GameObject>();
        regimentsList = GetComponent<TurnManager>().GetAllUnitsBySide(side);
        Vector3 formationPivot = GetComponent<UsefulCombatFunctions>().FormationPivot(side);
        foreach (GameObject reg in regimentsList)
        {
            List<Vector3> pp = new List<Vector3>();
            if (PathInstantiated.GetComponent<PathVariables>().pathCalculated)
            {
                pp = PathInstantiated.GetComponent<PathVariables>().PathOfGO;
                List<Vector3> ppGO = new List<Vector3>();
                Vector3 relativeDistanceFromPivot = reg.transform.position - formationPivot;
                for (int i = 0; i < pp.Count; i++)
                {
                    ppGO.Add(pp[i] + relativeDistanceFromPivot);
                }
                reg.GetComponent<RegimentPath>().regimentPathList = ppGO;
            }
        }

    }

    /// <summary>
    /// Launch the display of the formation slots and the path that regiments will follow
    /// </summary>
    public void LaunchScript()
    {
        Start();
        GetComponent<DisplayFormationChargeOnCombatMap>().enabled = true;
    }

    /// <summary>
    /// Stop the display of the formation slots and the path that regiments will follow
    /// </summary>
    public void StopScript()
    {
        GameObject[] existingSlots = GameObject.FindGameObjectsWithTag("RegimentSlot");
        foreach (GameObject slot in existingSlots) GameObject.Destroy(slot);
        GameObject[] existingPathLines = GameObject.FindGameObjectsWithTag("PathLine");
        foreach (GameObject pathLine in existingPathLines) GameObject.Destroy(pathLine);
        GetComponent<DisplayFormationChargeOnCombatMap>().enabled = false;

    }

    

    /// <summary>
    /// Get to display the formation slots where the regiments can move relatively to the target
    /// </summary>
    private void DisplayFormation(string side, Vector3 target)
    {

        List<Vector3> regimentsRelativePosition = new List<Vector3>();
        regimentsRelativePosition = GetComponent<UsefulCombatFunctions>().FormationPivotRelativePosition(side);
        //Displays the regimentslots relatively to the target
        foreach (Vector3 regimentRelativePosition in regimentsRelativePosition)
        {
            Vector3 position = target - regimentRelativePosition;
            Instantiate(RegimentSlot, position, Quaternion.identity);
        }
    }
}
