using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DisplaysFormationMoveOnCombatMap : MonoBehaviour
{
    public GameObject RegimentSlot;
    public Vector3 target;
    public Vector3 newTarget;
    public Camera cam;
    public GameObject DisplayPath;
    public GameObject PathInstantiated;
    //public Vector3 formationPivot;
    public string side;
    int iterationOnPath = 0;
    bool inputMouse = false;

    void Start()
    {
        side = GetComponent<TurnManager>().turn;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        //get the ground selected by mouse pointer
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.tag == "Ground")
        {
            Ground g = hit.collider.GetComponent<Ground>();
            target = g.transform.position;
            target.y += 0.5f;
        }

        target.z = GetComponent<UsefulCombatFunctions>().CorrectTargetZ(target.z);
        target.x = GetComponent<UsefulCombatFunctions>().CorrectTargetX(target.x);

        Vector3 formationPivot = GetComponent<UsefulCombatFunctions>().FormationPivot(side);
        DisplayFormation(side, target);
        PathInstantiated = Instantiate(DisplayPath, formationPivot, Quaternion.identity);

        // Get the max speed for these units
        //get all regiments by side 
        List<GameObject> regimentsList = new List<GameObject>();
        regimentsList = GetComponent<TurnManager>().GetAllUnitsBySide(side);
        //get the minimum moves per round in the team
        int minMovesPerRound = 100;
        foreach (GameObject reg in regimentsList)
        {
            if ((int) reg.GetComponent<CombatVariables>().moveCapacityRegStat < minMovesPerRound) minMovesPerRound = (int)reg.GetComponent<CombatVariables>().moveCapacityRegStat;
            reg.GetComponent<CombatVariables>().inFormation = true;
        }
        PathInstantiated.GetComponent<PathVariables>().movesPerRound = minMovesPerRound;
        //set the target & the graph to use for the seeker (inclue layer regiments for  
        PathInstantiated.GetComponent<PathVariables>().dynamicTarget = true;
        PathInstantiated.GetComponent<PathVariables>().GraphStringToUse = "FormationGraph";

    }
    /// <summary>
    /// Launch the display of the formation slots and the path that regiments will follow
    /// </summary>
    public void LaunchScript()
    {
        Start();
        GetComponent<DisplaysFormationMoveOnCombatMap>().enabled = true;
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
        GetComponent<DisplaysFormationMoveOnCombatMap>().enabled = false;

    }

    void Update()
    {
        //check if the target has changed, if it has delete all gameObjects formationslots and recreate ones
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.tag == "Ground")
        {
            Ground g = hit.collider.GetComponent<Ground>();
            newTarget = g.transform.position;
            newTarget.y += 0.5f;
        }
        if (newTarget!= target)
        {
            iterationOnPath = 0;
            target = newTarget;

            target.z = GetComponent<UsefulCombatFunctions>().CorrectTargetZ(target.z);
            target.x = GetComponent<UsefulCombatFunctions>().CorrectTargetX(target.x);

            GameObject[] existingSlots = GameObject.FindGameObjectsWithTag("RegimentSlot");
            foreach (GameObject slot in existingSlots) GameObject.Destroy(slot);
            DisplayFormation(side, target);
        }


        //also check if the path is available and store it into each unit
        List<GameObject> regimentsList = new List<GameObject>();
        regimentsList = GetComponent<TurnManager>().GetAllUnitsBySide(side);
        Vector3 formationPivot = GetComponent<UsefulCombatFunctions>().FormationPivot(side);
        foreach (GameObject reg in regimentsList)
        {
            List<Vector3> pp = new List<Vector3>();
            if (PathInstantiated.GetComponent<PathVariables>().pathCalculated && iterationOnPath < regimentsList.Count)
            {
                iterationOnPath +=1;
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
