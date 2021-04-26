using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

//TO DEV : 
public class DisplayRegimentCharge : MonoBehaviour
{
    public GameObject Obstacle;
    public GameObject PathLine;
    public GameObject RegimentSlot;
    public int numberOfPathTested = 0;
    public float startTime;
    public float endTime;
    public float time;
    public GraphMask GraphMaskToUse;
    public string side;
    /*
     * 
     */

    public void StartScript()
    {
        startTime = Time.realtimeSinceStartup;
        side = GetComponent<TurnManager>().turn;
        RegimentChargeDisplay(side);
        endTime = Time.realtimeSinceStartup;
        time = endTime - startTime;
        Debug.Log("number of path tested at the end :" + numberOfPathTested + "Time " + time);
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
        List<Cases> casesList = new List<Cases>();

        
        // while the prioritization has not been done for all regiments 
        while (regimentsToPrioritize.Count != 0 || time > 200f)//
        {
            

            //Debug.Log("regiments to prioritize left :" + regimentsToPrioritize.Count);
            //get the regiments that has not already be prioritized ( in the "regimentsToPrioritize" list)
            for (int k = 0; k < regimentsToPrioritize.Count; k++)
            {
                GameObject regiment = regimentsToPrioritize[k];
                //Debug.Log("For Regiment " + regiment);

                List<Ground> possibleGrounds = new List<Ground>();
                //PUT IN HERE THE PREFERABLE POSITIONS TO SEE IF THERE IS ONE (EXAMPLE PIKEMEN)
                if (regiment.GetComponent<GetStandardCombatSlots>().PreferedCombatSlotsExists())
                {
                    //complete with preferable slots
                }
                else
                {
                    //get the possible combat positions for each regiment (function for each regiment, with prefered slots example archers or pikemen, check if the slot is available(i.e no unit of osbstacle)) 
                    if (regiment.GetComponent<GetStandardCombatSlots>().CombatSlotsExists())
                    {
                        //list of the possible grounds
                        possibleGrounds = regiment.GetComponent<GetStandardCombatSlots>().GetCombatSlots();
                        //Debug.Log("For Regiment " + regiment + " number of possible grounds :" + possibleGrounds.Count);
                        //Check if there is already positions booked by the selected cases list, in this case remove it
                        for (int i = 0; i < possibleGrounds.Count; i++)
                        {
                            //Debug.Log(tmp);
                            foreach (SelectedCases cs in selectedCasesList)
                            {
                                if (i < possibleGrounds.Count)
                                {
                                    //Debug.Log("index out of range");
                                    if (possibleGrounds[i] == cs.positionBooked)
                                    {
                                        //Debug.Log("position booked :" + cs.positionBooked);
                                        possibleGrounds.Remove(possibleGrounds[i]);
                                        i = i - 1;
                                        break;
                                    }
                                }

                            }
                        }

                    }
                    //if there is no combat slot for the regiment, get the possible positions most next to them, first in diagonal
                    if (possibleGrounds.Count == 0)
                    {
                        possibleGrounds = regiment.GetComponent<GetStandardCombatSlots>().GetDiagonalNextPositionsToCombatSlots();
                        //Debug.Log("For Regiment " + regiment + " number of possible grounds extended :" + possibleGrounds.Count);
                        //Check if there is already positions booked by the selected cases list, in this case remove it
                        for (int i = 0; i < possibleGrounds.Count; i++)
                        {
                            //Debug.Log(tmp);
                            foreach (SelectedCases cs in selectedCasesList)
                            {
                                if (i < possibleGrounds.Count)
                                {
                                    //Debug.Log("index out of range");
                                    if (possibleGrounds[i] == cs.positionBooked)
                                    {
                                       // Debug.Log("position booked :" + cs.positionBooked);
                                        possibleGrounds.Remove(possibleGrounds[i]);
                                        i = i - 1;
                                        break;
                                    }
                                }

                            }
                        }
                    }
                    //if there is no combat slot for the regiment, get the possible positions most next to them, then in 2nd line
                    if (possibleGrounds.Count == 0)
                    {
                        possibleGrounds = regiment.GetComponent<GetStandardCombatSlots>().GetStraight2ndNextPositionsToCombatSlots();
                        //Debug.Log("For Regiment " + regiment + " number of possible grounds extended :" + possibleGrounds.Count);
                        //Check if there is already positions booked by the selected cases list, in this case remove it
                        for (int i = 0; i < possibleGrounds.Count; i++)
                        {
                            //Debug.Log(tmp);
                            foreach (SelectedCases cs in selectedCasesList)
                            {
                                if (i < possibleGrounds.Count)
                                {
                                    //Debug.Log("index out of range");
                                    if (possibleGrounds[i] == cs.positionBooked)
                                    {
                                        //Debug.Log("position booked :" + cs.positionBooked);
                                        possibleGrounds.Remove(possibleGrounds[i]);
                                        i = i - 1;
                                        break;
                                    }
                                }

                            }
                        }
                    }
                    //if there is no combat slot for the regiment, get the possible positions most next to them, then in diagonal 2nd line
                    if (possibleGrounds.Count == 0)
                    {
                        possibleGrounds = regiment.GetComponent<GetStandardCombatSlots>().GetDiagonal2ndNextPositionsToCombatSlots();
                        //Debug.Log("For Regiment " + regiment + " number of possible grounds extended :" + possibleGrounds.Count);
                        //Check if there is already positions booked by the selected cases list, in this case remove it
                        for (int i = 0; i < possibleGrounds.Count; i++)
                        {
                            //Debug.Log(tmp);
                            foreach (SelectedCases cs in selectedCasesList)
                            {
                                if (i < possibleGrounds.Count)
                                {
                                    //Debug.Log("index out of range");
                                    if (possibleGrounds[i] == cs.positionBooked)
                                    {
                                        //Debug.Log("position booked :" + cs.positionBooked);
                                        possibleGrounds.Remove(possibleGrounds[i]);
                                        i = i - 1;
                                        break;
                                    }
                                }

                            }
                        }
                    }
                }

                

                

                //Debug.Log("For Regiment " + regiment + " possible grounds reprocessed :" + possibleGrounds.Count);
                //if there is no possibleGround for this regiment, just remove it
                

                //in order to optimize the searched path, we should order the possibleGrounds by distance to the possible ground then inject one and see if a path is created, if not, inject the following etc.
                List<possibleGroundsDistance> possibleGroundsOrdered = new List<possibleGroundsDistance>();
                foreach(Ground possibleGround in possibleGrounds)
                {
                    possibleGroundsOrdered.Add(new possibleGroundsDistance(regiment, possibleGround));
                }
                possibleGroundsOrdered = possibleGroundsOrdered.OrderBy(x => x.distanceFromRegiment).ToList();
                //possibleGroundsOrdered.Reverse();
                //then use the seeker to get the path to the possible positions
                foreach (possibleGroundsDistance pg in possibleGroundsOrdered)
                {
                    Ground possibleGround = pg.possibleGround;
                    //Debug.Log("checking possible path for "+possibleGround);
                    //if we found a valid path between the two points (avoiding booked positions in end of rounds and in end of the charge)
                    Path p = PathSearchComplete(possibleGround, regiment, selectedCasesList);
                    if (p != null)
                    {
                        //get the number of moves per round for this regiment
                        int numberOfMovesPerRound = regiment.GetComponent<CombatVariables>().moveCapacityRegStat;
                        //check at each end of round if the regiment is in a ground already booked by another regiment 
                        List<Ground> endOfRoundPosition = new List<Ground>();
                        int numberOfRounds = 0;
                        int i = 0;
                        while (i < p.vectorPath.Count)
                        {
                            //iterate for each position
                            
                            //check if we are at the end of a round (no modulo left to division of the number of rounds), and store it into the endOfRoundPosition List
                            if (i % numberOfMovesPerRound == 0)
                            {
                                endOfRoundPosition.Add(GetComponent<UsefulCombatFunctions>().GetTargetGroundVector(p.vectorPath[i] + new Vector3(0, 1, 0)));
                                numberOfRounds += 1;
                            }
                            i += 1;

                        }
                        //check if we have not forgotten the last round position that is the end position of the object
                        if (i % numberOfMovesPerRound != 0 && i < p.vectorPath.Count) endOfRoundPosition.Add(GetComponent<UsefulCombatFunctions>().GetTargetGroundVector(p.vectorPath[i] + new Vector3(0, 1, 0)));
                        casesList.Add(new Cases(regiment, possibleGround, p, numberOfRounds, endOfRoundPosition));
                        break;
                    }
                    else
                    {
                        Debug.Log("No path found for regiment " + regiment + "going to " + possibleGround);
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

            int minRounds = 1000;
            foreach(Cases cas in casesList)
            {
                if (cas.numberOfRounds < minRounds) minRounds = cas.numberOfRounds;
                //Debug.Log("minRound is " + minRounds);
            }
            //then delete all cases that are not on the optimum
            for (int i = 0; i < casesList.Count; i++)
            {
                if (casesList[i].numberOfRounds != minRounds)
                {
                    casesList.Remove(casesList[i]);
                    //as we have removed one element of the list, make sure to set i to the previous stage
                    i = i - 1;
                }
            }

            //1st bis get the one who has the less distance 

            
            float minDistance = 1000f;
            foreach (Cases cas in casesList)
            {
                if (Vector3.Distance(cas.regiment.transform.position,cas.possiblePosition.transform.position) < minDistance) minDistance = Vector3.Distance(cas.regiment.transform.position, cas.possiblePosition.transform.position);
                //Debug.Log("minDistance is " + minRounds);
            }
            //then delete all cases that are not on the optimum
            for (int i = 0; i < casesList.Count; i++)
            {
                if (Vector3.Distance(casesList[i].regiment.transform.position, casesList[i].possiblePosition.transform.position) != minDistance)
                {
                    casesList.Remove(casesList[i]);
                    //as we have removed one element of the list, make sure to set i to the previous stage
                    i = i - 1;
                }
            }
            //2nd get the right or left max

            if (side == "enemy")
            {
                int maxLeft = 37;
                //take the most left regiment
                foreach (Cases cas in casesList)
                {
                    if (cas.regiment.transform.position.x < maxLeft) maxLeft = (int)cas.regiment.transform.position.x;
                }
                //then delete all cases that are not on the optimum
                for (int i = 0; i < casesList.Count; i++)
                {
                    if (casesList[i].regiment.transform.position.x != maxLeft)
                    {
                        casesList.Remove(casesList[i]);
                        i = i - 1;
                    }
                }

            }
            else
            {
                int maxRight = -10;
                //take the most right regiment
                foreach (Cases cas in casesList)
                {
                    if (cas.regiment.transform.position.x > maxRight) maxRight = (int)cas.regiment.transform.position.x;
                }
                //then delete all cases that are not on the optimum
                for (int i = 0; i < casesList.Count; i++)
                {
                    if (casesList[i].regiment.transform.position.x != maxRight)
                    {
                        casesList.Remove(casesList[i]);
                        i = i - 1;
                    }
                }
            }
            //3rd get the one with max speed (move capacity)
            int maxSpeed = 1;
            foreach (Cases cas in casesList)
            {
                if (cas.regiment.GetComponent<CombatVariables>().moveCapacityRegStat > maxSpeed) maxSpeed = cas.regiment.GetComponent<CombatVariables>().moveCapacityRegStat;
            }
            //then delete all cases that are not on the optimum
            for (int i = 0; i < casesList.Count; i++)
            {
                if (casesList[i].regiment.GetComponent<CombatVariables>().moveCapacityRegStat != maxSpeed)
                {
                    casesList.Remove(casesList[i]);
                    i = i - 1;
                }
            }
            //4rd get the one with lower position 
            int lowerPos = 6;
            foreach (Cases cas in casesList)
            {
                if (cas.regiment.transform.position.z < lowerPos) lowerPos = (int)cas.regiment.transform.position.z;
            }
            //then delete all cases that are not on the optimum
            for (int i = 0; i < casesList.Count; i++)
            {
                if (casesList[i].regiment.transform.position.z != lowerPos)
                {
                    casesList.Remove(casesList[i]);
                    i = i - 1;
                }
            }
            //5th fuck it, get one and store it into SelectedCases List
            //Debug.Log("Getting at the end of prioritization");
            Cases cslct = casesList[0];
            //cslct.PrintData();  
            selectedCasesList.Add(new SelectedCases(cslct.regiment, cslct.possiblePosition, cslct.pathUsed, cslct.numberOfRounds, cslct.endOfRoundGrounds));
            regimentsToPrioritize.Remove(cslct.regiment);
            casesList.Clear();

        }
        // FINAL STEP : Display the path selected putting a path display on each unit , we have to fill the path display gameobject instantiated with the path used in the selected path 
        foreach (SelectedCases cas in selectedCasesList)
        {
            Debug.Log("Regiment final path is" + cas.regiment + "for ground " + cas.positionBooked + " row " + cas.positionBooked.row);
            //Instantiate a Pathline from the regiment with exact paht booked
            GameObject PathInstantiated = Instantiate(PathLine, cas.regiment.transform.position, Quaternion.identity);
            PathInstantiated.GetComponent<PathVariables>().dynamicTarget = false;
            if(side == "enemy")
            {
                PathInstantiated.GetComponent<PathVariables>().GraphStringToUse = "EnemyRegimentAloneGraph";
            }
            else
            {
                PathInstantiated.GetComponent<PathVariables>().GraphStringToUse = "PlayerRegimentAloneGraph";
            }
            //Inject the path selected
            PathInstantiated.GetComponent<PathVariables>().pathInjected = cas.pathUsed;
            //Inject the path selected
            PathInstantiated.GetComponent<PathVariables>().movesPerRound = cas.regiment.GetComponent<CombatVariables>().moveCapacityRegStat;
            //Instantiate a regiment slot on the position booked
            Vector3 position = cas.positionBooked.transform.position + new Vector3(0, 0.5f, 0);
            Instantiate(RegimentSlot, position, Quaternion.identity);
            //store the vector 3 path in regiment object 
            cas.regiment.GetComponent<RegimentPath>().regimentPathList = cas.pathUsed.vectorPath;
            cas.regiment.GetComponent<CombatVariables>().inFormation = false;
        }

        selectedCasesList.Clear();

        endTime = Time.realtimeSinceStartup;
        time = endTime - startTime;



    }

    public Path PathSearchComplete(Ground possibleGround, GameObject regiment, List<SelectedCases> selectedCasesList)
    {
        if (side == "enemy")
        {
            GraphMaskToUse = GraphMask.FromGraphName("EnemyRegimentAloneGraph");
            GetComponent<Seeker>().graphMask = GraphMaskToUse;
        }
        else
        {
            GraphMaskToUse = GraphMask.FromGraphName("PlayerRegimentAloneGraph");
            GetComponent<Seeker>().graphMask = GraphMaskToUse;
        }
        
        bool pathCompleted = false;
        Path p = null;
        //if the path don't already exists, no need to use the while loop
        Vector3 position = regiment.transform.position + new Vector3(0, 0, 0);
        Vector3 target = possibleGround.transform.position + new Vector3(0, 1, 0);
        int numberOfTests = 0;
        //if the path exists, we need to loop until the path do not go on a booked ground 
        while (!pathCompleted && numberOfTests < 100)
        {
            numberOfTests += 1;
            AstarPath.active.Scan();
            numberOfPathTested += 1;
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
                AstarPath.active.Scan();
                //Debug.Log("path found");
                //get the number of moves per round for this regiment
                int numberOfMovesPerRound = regiment.GetComponent<CombatVariables>().moveCapacityRegStat;
                //check at each end of round if the regiment is in a ground already booked by another regiment 
                List<Ground> endOfRoundPosition = new List<Ground>();
                int i = 0;
                while (i < p.vectorPath.Count)
                {
                    
                    
                    //check if we are at the end of a round (no modulo left to division of the number of rounds), and store it into the endOfRoundPosition List
                    if (i % numberOfMovesPerRound == 0) endOfRoundPosition.Add(GetComponent<UsefulCombatFunctions>().GetTargetGroundVector(p.vectorPath[i] + new Vector3(0,1,0)));
                    //iterate for each position
                    i += 1;
                }
                //check if we have not forgotten the last round position that is the end position of the object
                if (i % numberOfMovesPerRound != 0 && i < p.vectorPath.Count) endOfRoundPosition.Add(GetComponent<UsefulCombatFunctions>().GetTargetGroundVector(p.vectorPath[i] + new Vector3(0, 1, 0)));
                    
                //check if the path created has no obstacles put on it and so the path is completed 
                int obstaclesCreated = 0;
                //Check if the endOfRoundPosition List matches some of the same round's position booked by the selected cases list, and repeat it 
                
                foreach (SelectedCases cs in selectedCasesList)
                {
                    //get the max iterations (if the number of rounds diverts between the two path)
                    int maxRound = Mathf.Max(cs.endOfRoundGrounds.Count, endOfRoundPosition.Count);
                    int index1 = 0;
                    int index2 = 0;
                    for (int j = 0; j < maxRound; j++)
                    {
                        //check if j has reached the max of the index for the list, and if it has keep it at the max no upper
                        if (cs.endOfRoundGrounds.Count - 2 >= j) index1 = j;
                        else index1 = cs.endOfRoundGrounds.Count - 2;
                        if (endOfRoundPosition.Count - 2 >= j) index2 = j;
                        else index2 = endOfRoundPosition.Count - 2;
                        //check if the two index matches : beware there is instantiated obstacles at the same position
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
               
                if (obstaclesCreated == 0 && numberOfTests < 1000)
                {
                    pathCompleted = true;
                }
                if (numberOfTests >= 100)
                {
                    Debug.Log("Too much test on " + regiment + "for target " + possibleGround);
                    pathCompleted = true;
                }
            }
            

        }
        //Destroy all obstacles created for this path
        GameObject[] obs = GameObject.FindGameObjectsWithTag("Obstacle");
        int tries = 0;
        //checking if there is still obstacles in the game
        while (obs.Length != 0 || tries > 100)
        {
            foreach (GameObject ob in obs)
            {
                GameObject.DestroyImmediate(ob.gameObject);
            }
            obs = GameObject.FindGameObjectsWithTag("Obstacle");
            tries += 1;
        }
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

    public void PrintData()
    {
        Debug.Log("cases class print data called");
        Debug.Log("cases regiment :" + regiment);
        Debug.Log("cases possible position :" + possiblePosition);
        Debug.Log("cases path used :" + pathUsed);
        Debug.Log("cases numberOfRounds :" + numberOfRounds);
        Debug.Log("cases endOfRoundGrounds :" + endOfRoundGrounds.Count);

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
    public string row;
    public SelectedCases(GameObject oregiment, Ground oPosition, Path opathUsed, int onumberOfRounds, List<Ground> oendOfRoundGrounds)
    {
        regiment = oregiment;
        positionBooked= oPosition;
        pathUsed = opathUsed;
        numberOfRounds = onumberOfRounds;
        endOfRoundGrounds = oendOfRoundGrounds;
        row = positionBooked.transform.parent.name;
        

    }

    public void PrintData()
    {
        Debug.Log("cases class print data called");
        Debug.Log("cases regiment :" + regiment);
        Debug.Log("cases booked position :" + positionBooked);
        Debug.Log("cases path used :" + pathUsed);
        Debug.Log("cases numberOfRounds :" + numberOfRounds);
        Debug.Log("cases endOfRoundGrounds :" + endOfRoundGrounds.Count);

    }
}

public class possibleGroundsDistance
{
    
    public Ground possibleGround;
    public GameObject regiment;
    public float distanceFromRegiment;
    public string row;
    public possibleGroundsDistance(GameObject oregiment, Ground oPossibleGround)
    {
        regiment = oregiment;
        possibleGround = oPossibleGround;
        distanceFromRegiment = Vector3.Distance(regiment.transform.position, possibleGround.transform.position);
        row = possibleGround.transform.parent.name;
    }
}
