﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class DisplayRegimentCharge : MonoBehaviour
{
    public GameObject Obstacle;
    public GameObject PathLine;
    public GameObject RegimentSlot;

    public void StartScript()
    {
        string side = GetComponent<TurnManager>().turn;
        RegimentChargeDisplay(side);
    }

    public void StopScript()
    {
        GameObject[] existingSlots = GameObject.FindGameObjectsWithTag("RegimentSlot");
        foreach (GameObject slot in existingSlots) GameObject.Destroy(slot);
        GameObject[] existingPathLines = GameObject.FindGameObjectsWithTag("PathLine");
        foreach (GameObject pathLine in existingPathLines) GameObject.Destroy(pathLine);
    }

    public void RegimentChargeDisplay(string side)
    {
        
        //get the list of the regiments of the actual side
        List<GameObject> regimentsList = new List<GameObject>();
        regimentsList = GetComponent<TurnManager>().GetAllUnitsBySide(side);

        //list of the regiments that has to be prioritized (decreasing as things progress)
        List<GameObject> regimentsToPrioritize = new List<GameObject>();
        regimentsToPrioritize = regimentsList;

        

        //instantiate the list of selected cases
        List<SelectedCases> selectedCasesList = new List<SelectedCases>();

        //instantiate the list of selected cases
        List<Cases> CasesList = new List<Cases>();


        // while the prioritization has not been done for all regiments 
        while (regimentsToPrioritize.Count != 0)
        {
            //get the regiments that has not already be prioritized ( in the "regimentsToPrioritize" list)
            foreach(GameObject regiment in regimentsToPrioritize)
            {
                List<Ground> possibleGrounds = new List<Ground>();
                //PUT IN HERE THE PREFERABLE POSITIONS TO SEE IF THERE IS ONE (EXAMPLE PIKEMEN)

                //get the possible combat positions for each regiment (function for each regiment, with prefered slots example archers or pikemen, check if the slot is available(i.e no unit of osbstacle)) 
                if (regiment.GetComponent<GetStandardCombatSlots>().CombatSlotsExists())
                {
                    //list of the possible grounds
                    
                    possibleGrounds = regiment.GetComponent<GetStandardCombatSlots>().GetCombatSlots();

                }
                //if there is no combat slot for the regiment, get the possible positions most next to them
                if (!regiment.GetComponent<GetStandardCombatSlots>().CombatSlotsExists())
                {
                    possibleGrounds = regiment.GetComponent<GetStandardCombatSlots>().GetNextPositionsToCombatSlots();
                }

                //Check if there is already positions booked by the selected cases list, in this case remove it
                foreach (Ground tmp in possibleGrounds)
                {
                    //Debug.Log(tmp);
                    foreach (SelectedCases cs in selectedCasesList)
                    {
                        
                        if (tmp == cs.positionBooked) possibleGrounds.Remove(tmp);
                    }
                }
                //then use the seeker to get the path to the possible positions
                foreach(Ground possibleGround in possibleGrounds)
                {
                    Debug.Log(possibleGround);
                    //if we found a valid path between the two points (avoiding booked positions in end of rounds and in end of the charge)
                    if(PathSearchComplete(possibleGround,regiment,selectedCasesList) != null)
                    {
                        Path p = PathSearchComplete(possibleGround, regiment, selectedCasesList);
                        //get the number of moves per round for this regiment
                        int numberOfMovesPerRound = regiment.GetComponent<CombatVariables>().moveCapacity;
                        //check at each end of round if the regiment is in a ground already booked by another regiment 
                        List<Ground> endOfRoundPosition = new List<Ground>();
                        int numberOfRounds = 0;
                        int i = 0;
                        while (i < p.vectorPath.Count)
                        {
                            //iterate for each position
                            i += 1;
                            //check if we are at the end of a round (no modulo left to division of the number of rounds), and store it into the endOfRoundPosition List
                            if (i % numberOfMovesPerRound == 0)
                            {
                                endOfRoundPosition.Add(GetComponent<UsefulCombatFunctions>().GetTargetGroundVector(p.vectorPath[i]));
                                numberOfRounds += 1;
                            }
                            
                        }
                        //check if we have not forgotten the last round position that is the end position of the object
                        if (i % numberOfMovesPerRound != 0) endOfRoundPosition.Add(GetComponent<UsefulCombatFunctions>().GetTargetGroundVector(p.vectorPath[i]));
                        CasesList.Add(new Cases(regiment, possibleGround, p, numberOfRounds,endOfRoundPosition));
                    }



                }

            }
            //Once we have all the cases, prioritize them by using the delete method
            /*
             * 1) the ones who takes the minimum amount of rounds to get to the point, 
             * 2) then the one who is on the right for player or left for enemy,
             * 3) then the one who has the most speed
             * 4) if there is still 2 regiments that are on the list, take the one who is the lower in the map
             * 5) then just get one for god sake 
             */
            //1st get the minimum rounds in cases list

            foreach(Cases cas in CasesList)
            {
                Debug.Log("regiment still in course 0 = " + CasesList.Count);
            }
            int minRounds = 1000;
            foreach(Cases cas in CasesList)
            {
                if (cas.numberOfRounds < minRounds) minRounds = cas.numberOfRounds;
            }
            //then delete all cases that are not on the optimum
            foreach (Cases cas in CasesList)
            {
                if (cas.numberOfRounds != minRounds) CasesList.Remove(cas);
            }

            foreach (Cases cas in CasesList)
            {
                Debug.Log("regiment still in course 1 = " + CasesList.Count);
            }
            //2nd get the right or left max

            if (side == "enemy")
            {
                int maxLeft = 37;
                //take the most left regiment
                foreach (Cases cas in CasesList)
                {
                    if (cas.regiment.transform.position.x < maxLeft) maxLeft = (int)cas.regiment.transform.position.x;
                }
                //then delete all cases that are not on the optimum
                foreach (Cases cas in CasesList)
                {
                    if (cas.regiment.transform.position.x != maxLeft) CasesList.Remove(cas);
                }

            }
            else
            {
                int maxRight = -10;
                //take the most right regiment
                foreach (Cases cas in CasesList)
                {
                    if (cas.regiment.transform.position.x > maxRight) maxRight = (int)cas.regiment.transform.position.x;
                }
                //then delete all cases that are not on the optimum
                foreach (Cases cas in CasesList)
                {
                    if (cas.regiment.transform.position.x != maxRight) CasesList.Remove(cas);
                }
            }
            foreach (Cases cas in CasesList)
            {
                Debug.Log("regiment still in course 2 = " + CasesList.Count);
            }
            //3rd get the one with max speed (move capacity)
            int maxSpeed = 1;
            foreach (Cases cas in CasesList)
            {
                if (cas.regiment.GetComponent<CombatVariables>().moveCapacity > maxSpeed) maxSpeed = cas.regiment.GetComponent<CombatVariables>().moveCapacity;
            }
            //then delete all cases that are not on the optimum
            foreach (Cases cas in CasesList)
            {
                if (cas.regiment.GetComponent<CombatVariables>().moveCapacity != maxSpeed) CasesList.Remove(cas);
            }
            foreach (Cases cas in CasesList)
            {
                Debug.Log("regiment still in course 3 = " + CasesList.Count);
            }
            //4rd get the one with lower position 
            int lowerPos = 6;
            foreach (Cases cas in CasesList)
            {
                if (cas.regiment.transform.position.z < lowerPos) lowerPos = (int)cas.regiment.transform.position.z;
            }
            //then delete all cases that are not on the optimum
            foreach (Cases cas in CasesList)
            {
                if (cas.regiment.transform.position.z != lowerPos) CasesList.Remove(cas);
            }
            foreach (Cases cas in CasesList)
            {
                Debug.Log("regiment still in course 4 = " + CasesList.Count);
            }
            //5th fuck it, get one and store it into SelectedCases List
            Cases cslct = CasesList[1];
            selectedCasesList.Add(new SelectedCases(cslct.regiment, cslct.possiblePosition, cslct.pathUsed, cslct.numberOfRounds, cslct.endOfRoundGrounds));
            regimentsToPrioritize.Remove(cslct.regiment);

        }
        // FINAL STEP : Display the path selected putting a path display on each unit , we have to fill the path display gameobject instantiated with the path used in the selected path 
        foreach (SelectedCases cas in selectedCasesList)
        {
            Debug.Log("Regiment final path is" + cas.regiment + cas.positionBooked + cas.pathUsed);
            //Instantiate a Pathline from the regiment with exact paht booked
            GameObject PathInstantiated = Instantiate(PathLine, cas.regiment.transform.position, Quaternion.identity);
            PathInstantiated.GetComponent<PathVariables>().dynamicTarget = false;
            PathInstantiated.GetComponent<PathVariables>().GraphStringToUse = "FormationGraph";
            //Inject the path selected
            PathInstantiated.GetComponent<PathVariables>().pathInjected = cas.pathUsed;
            //Instantiate a regiment slot on the position booked
            Vector3 position = cas.positionBooked.transform.position + new Vector3(0, 1, 0);
            Instantiate(RegimentSlot, position, Quaternion.identity);
        }

        selectedCasesList.Clear();
        CasesList.Clear();

    }

    public Path PathSearchComplete(Ground possibleGround, GameObject regiment, List<SelectedCases> selectedCasesList)
    {
        bool pathCompleted = false;
        Path p;
        //if the path don't already exists, no need to use the while loop
        Vector3 position = regiment.transform.position + new Vector3(0, 1, 0);
        Vector3 target = possibleGround.transform.position + new Vector3(0, 1, 0);
        p = GetComponent<Seeker>().StartPath(position, target);
        p.BlockUntilCalculated();
        if (p.error)
        {
            Debug.Log("no path found");
            pathCompleted = true;
            return null;
        }
        //if the path exists, we need to loop until the path do not go on a booked ground 
        while (!pathCompleted)
        {
            p = GetComponent<Seeker>().StartPath(position, target);
            p.BlockUntilCalculated();
            if (p.error)
            {
                Debug.Log("no path found");
                pathCompleted = true;
                return null;
            }
            else
            {
                //get the number of moves per round for this regiment
                int numberOfMovesPerRound = regiment.GetComponent<CombatVariables>().moveCapacity;
                //check at each end of round if the regiment is in a ground already booked by another regiment 
                List<Ground> endOfRoundPosition = new List<Ground>();
                int i = 0;
                while (i < p.vectorPath.Count)
                {
                    //iterate for each position
                    i += 1;
                    //check if we are at the end of a round (no modulo left to division of the number of rounds), and store it into the endOfRoundPosition List
                    if (i % numberOfMovesPerRound == 0) endOfRoundPosition.Add(GetComponent<UsefulCombatFunctions>().GetTargetGroundVector(p.vectorPath[i]));
                }
                //check if we have not forgotten the last round position that is the end position of the object
                if (i % numberOfMovesPerRound != 0) endOfRoundPosition.Add(GetComponent<UsefulCombatFunctions>().GetTargetGroundVector(p.vectorPath[i]));

                //check if the path created has no obstacles put on it and so the path is completed 
                int obstaclesCreated = 0;
                //Check if the endOfRoundPosition List matches some of the same round's position booked by the selected cases list, and repeat it 
                
                foreach (SelectedCases cs in selectedCasesList)
                {
                    //get the max iterations (if the number of rounds diverts between the two path)
                    int maxRound = Mathf.Max(cs.endOfRoundGrounds.Count, endOfRoundPosition.Count);
                    int index1 = 0;
                    int index2 = 0;
                    for (int j = 0; j <= maxRound; j++)
                    {
                        //check if j has reached the max of the index for the list, and if it has keep it at the max no upper
                        if (cs.endOfRoundGrounds.Count > j) index1 = j;
                        else index1 = cs.endOfRoundGrounds.Count;
                        if (endOfRoundPosition.Count > j) index2 = j;
                        else index2 = cs.endOfRoundGrounds.Count;
                        //check if the two index matches
                        if (cs.endOfRoundGrounds[index1] == endOfRoundPosition[index2])//if at some point the two target grounds matches
                        {
                            obstaclesCreated += 1;
                            Ground g = endOfRoundPosition[index2];
                            Vector3 position2 = g.transform.position + new Vector3(0, 1, 0);
                            //we create an obstacle on this position to restart the path
                            Instantiate(Obstacle, position2, Quaternion.identity);
                        }

                    }
                        
                }  
                //at the end of the simulation, if no obstacles were put on the path of the regiment, then set the pathCompleted to true
                if (obstaclesCreated == 0) pathCompleted = true;
            }
            
        }
        //Destroy all obstacles created for this path
        GameObject[] existingObstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in existingObstacles) GameObject.Destroy(obstacle);
        return p;
    }
    /*Regiment Charge (side)
     * 
     * Variables : 
     * 
     * List "Cases" of classes with regiment, possible position, path and number of turns to get to the point
     * List "RoundPositions" of classes with round and position of the regiment at this specific round
     * List "SelectedCases" of classes with order, regiment, possible position, path and number of turns to get to the point
     * List "RegimentsToPrioritize" of gameobjects : regiments that still have to be prioritized
     * 
     * Script :
     * get the list of the regiments of the actual side
     * 
     * Start Loop :
     * get the regiments that has not already be prioritized (not in the "selected cases" list)
     * get the possible combat positions for each regiment (function for each regiment, with prefered slots example archers or pikemen,check if the slot is available (i.e no unit of osbstacle)) 
     * calculate the path for each possible combat position
     * check if the path simulated put the regiment in a cell that is already used by another regiment at the end of the same round
     * If it is the case, the previous regiment had the priority so put an obstacle on that path and rerun the path with these obstacle then delete all obstacles for next cases
     * store the case with regiment, possible position, path and number of turns to get to the point  (List cases)
     * only keep the cases with minimum number of turns
     * to Choose who go first, we have 5 rules : 
     * 1) the ones who takes the minimum amount of rounds to get to the point, 
     * 2) then the one who is on the right for player or left for enemy,
     * 3) then the one who has the most speed
     * 4) if there is still 2 regiments that are on the list, take the one who is the lower in the map
     * 5) then just get one for god sake 
     * 
     * Insert the case who go first in "selected cases" with max order + 1
     * 
     * put an obstacle on the final case used by the regiment to avoid double selection with combatSlots
     * 
     * Then the one who go first get his path registered, and also register in a list the cases where its position at the end of each round is stored (List B)
     * 
     * Loop
     * 
     * Order the list "Selected Cases" and instantiate a pathline for each of them, push the path integrated in the selected cases iteration
     * 
     * Note : Display Path Variables Script should include a script to push directly a path into the PathLine
     * 
     * 
     * 
     * 
     * 
     * 
     */



}

public class Cases
{
    
    public GameObject regiment;
    public Ground possiblePosition;
    public Path pathUsed;
    public int numberOfRounds;
    public List<Ground> endOfRoundGrounds = new List<Ground>();

    public Cases(GameObject oregiment, Ground opossiblePosition, Path opathUsed, int onumberOfRounds, List<Ground> oendOfRoundGrounds)
    {
        regiment = oregiment;
        possiblePosition = opossiblePosition;
        pathUsed = opathUsed;
        numberOfRounds = onumberOfRounds;
        endOfRoundGrounds = oendOfRoundGrounds;

    }

}

public class SelectedCases
{
    //Nota bene : use ground to store the entire position and not the exact position
    public GameObject regiment;
    public Ground positionBooked;
    public Path pathUsed;
    public int numberOfRounds;
    public List<Ground> endOfRoundGrounds = new List<Ground>();
    public SelectedCases(GameObject oregiment, Ground oPosition, Path opathUsed, int onumberOfRounds, List<Ground> oendOfRoundGrounds)
    {
        regiment = oregiment;
        positionBooked= oPosition;
        pathUsed = opathUsed;
        numberOfRounds = onumberOfRounds;
        endOfRoundGrounds = oendOfRoundGrounds;

    }
}

