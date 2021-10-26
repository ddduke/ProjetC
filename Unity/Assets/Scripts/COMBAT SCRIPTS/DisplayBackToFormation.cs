using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using Pathfinding;


public class DisplayBackToFormation : MonoBehaviour
{
    public string side;
    public GameObject Obstacle;
    public GameObject PathLine;
    public GameObject regimentSlot;
    public bool inputMouse = false;
    public Camera cam;
    public Vector3 target;
    public Vector3 newTarget;
    public Vector3 playerFormationPivot;
    public Vector3 enemyFormationPivot;
    public int numberOfPathTested = 0;
    public List<regimentFormationPosition> playerRegimentFormationPositions;
    public List<regimentFormationPosition> enemyRegimentFormationPositions;
    public int playerMaxXFormation;
    public int playerMinXFormation;
    public int playerMaxZFormation;
    public int playerMinZFormation;
    public int enemyMaxXFormation;
    public int enemyMinXFormation;
    public int enemyMaxZFormation;
    public int enemyMinZFormation;

    public GraphMask GraphMaskToUse;
    // Start is called before the first frame update
    void Start()
    {
        side = GetComponent<TurnManager>().turn;
        playerRegimentFormationPositions = new List<regimentFormationPosition>();
        enemyRegimentFormationPositions = new List<regimentFormationPosition>();
        UpdateFormationInfo("enemy");
        UpdateFormationInfo("player");
        GetComponent<DisplayBackToFormation>().enabled = false;
    }

    public void LaunchScript()
    {
        side = GetComponent<TurnManager>().turn;
        GetComponent<DisplayBackToFormation>().enabled = true;
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
        inputMouse = false;
        GetComponent<Seeker>().graphMask = 1;
        GetComponent<DisplayBackToFormation>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            inputMouse = true;
        }
        //check if the target has changed, if it has delete all gameObjects formationslots and recreate ones
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.tag == "Ground")
        {
            Ground g = hit.collider.GetComponent<Ground>();
            newTarget = g.transform.position;
            newTarget.y += 0.5f;
            if(side=="player")
            {
                newTarget.z = GetComponent<UsefulCombatFunctions>().CorrectTargetZWithMinMax(side,playerMinZFormation,playerMaxZFormation)
                newTarget.x = GetComponent<UsefulCombatFunctions>().CorrectTargetXWithMinMax(side,playerMinXFormation,playerMaxXFormation)
            }
            if(side=="enemy")
            {
                newTarget.z = GetComponent<UsefulCombatFunctions>().CorrectTargetZWithMinMax(side,enemyMinZFormation,enemyMaxZFormation)
                newTarget.x = GetComponent<UsefulCombatFunctions>().CorrectTargetXWithMinMax(side,enemyMinXFormation,enemyMaxXFormation)
            }
            
        }
        if (newTarget != target && !inputMouse)
        {
            target = newTarget;
            if(side=="player")
            {
                newTarget.z = GetComponent<UsefulCombatFunctions>().CorrectTargetZWithMinMax(side,playerMinZFormation,playerMaxZFormation)
                newTarget.x = GetComponent<UsefulCombatFunctions>().CorrectTargetXWithMinMax(side,playerMinXFormation,playerMaxXFormation)
            }
            if(side=="enemy")
            {
                newTarget.z = GetComponent<UsefulCombatFunctions>().CorrectTargetZWithMinMax(side,enemyMinZFormation,enemyMaxZFormation)
                newTarget.x = GetComponent<UsefulCombatFunctions>().CorrectTargetXWithMinMax(side,enemyMinXFormation,enemyMaxXFormation)
            }
            GameObject[] existingSlots = GameObject.FindGameObjectsWithTag("RegimentSlot");
            foreach (GameObject slot in existingSlots) GameObject.Destroy(slot);
            GameObject[] existingPathLines = GameObject.FindGameObjectsWithTag("PathLine");
            foreach (GameObject pathLine in existingPathLines) GameObject.Destroy(pathLine);
            if (side=="player") DisplayPathToFormation(playerRegimentFormationPositions, target);
            if (side == "enemy") DisplayPathToFormation(enemyRegimentFormationPositions, target);


            //make a charge-like function, with priority, with possible grounds, selected BCases, etc... with another function as it is not the same possible grounds that we try to reach!
        }

        if (inputMouse)
        {
           //freeze path and combatslots then store it into each unit as for the charge function
        }

    }

    private void DisplayFormation(string side, Vector3 target)
    {

        if (side == "player")
        {
            List<Vector3> regimentsRelativePosition = new List<Vector3>();

            foreach (regimentFormationPosition elmt in playerRegimentFormationPositions)
            {
                regimentsRelativePosition.Add(elmt.relativePosition);
            }
            //Displays the regimentslots relatively to the target
            foreach (Vector3 regimentRelativePosition in regimentsRelativePosition)
            {
                Vector3 position = target - regimentRelativePosition;
                Instantiate(regimentSlot, position, Quaternion.identity);
            }
        }

        if (side == "enemy")
        {
            List<Vector3> regimentsRelativePosition = new List<Vector3>();

            foreach (regimentFormationPosition elmt in enemyRegimentFormationPositions)
            {
                regimentsRelativePosition.Add(elmt.relativePosition);
            }
            //Displays the regimentslots relatively to the target
            foreach (Vector3 regimentRelativePosition in regimentsRelativePosition)
            {
                Vector3 position = target - regimentRelativePosition;
                Instantiate(regimentSlot, position, Quaternion.identity);
            }
        }


    }


    public void UpdateFormationInfo(string side)
    {
        if (side == "player")
        {
            playerRegimentFormationPositions.Clear();
            playerFormationPivot = GetComponent<UsefulCombatFunctions>().FormationPivot(side);
            List<GameObject> regimentsList = new List<GameObject>();
            regimentsList = GetComponent<TurnManager>().GetAllUnitsBySide(side);
            GetComponent<UsefulCombatFunctions>().
            foreach (GameObject regiment in regimentsList)
            {
                Vector3 relativePosition = playerFormationPivot - regiment.transform.position;
                playerRegimentFormationPositions.Add(new regimentFormationPosition(regiment, relativePosition));
            }
            playerMaxXFormation = GetComponent<UsefulCombatFunctions>().GetMaxX(side);
            playerMinXFormation = GetComponent<UsefulCombatFunctions>().GetMinX(side);
            playerMaxZFormation = GetComponent<UsefulCombatFunctions>().GetMaxZ(side);
            playerMinZFormation = GetComponent<UsefulCombatFunctions>().GetMinZ(side);
        }

        if (side == "enemy")
        {
            enemyRegimentFormationPositions.Clear();
            enemyFormationPivot = GetComponent<UsefulCombatFunctions>().FormationPivot(side);
            List<GameObject> regimentsList = new List<GameObject>();
            regimentsList = GetComponent<TurnManager>().GetAllUnitsBySide(side);
            foreach (GameObject regiment in regimentsList)
            {
                Vector3 relativePosition = playerFormationPivot - regiment.transform.position;
                enemyRegimentFormationPositions.Add(new regimentFormationPosition(regiment, relativePosition));
            }
            enemyMaxXFormation = GetComponent<UsefulCombatFunctions>().GetMaxX(side);
            enemyMinXFormation = GetComponent<UsefulCombatFunctions>().GetMinX(side);
            enemyMaxZFormation = GetComponent<UsefulCombatFunctions>().GetMaxZ(side);
            enemyMinZFormation = GetComponent<UsefulCombatFunctions>().GetMinZ(side);
        }
    }

    public void DisplayPathToFormation(List<regimentFormationPosition> regimentFormationPositions, Vector3 target)
    {


        //list of the regiments that has to be prioritized (decreasing as things progress)
        List<GameObject> regimentsToPrioritize = new List<GameObject>();
        foreach (regimentFormationPosition elmt in regimentFormationPositions)
        {
            regimentsToPrioritize.Add(elmt.regiment);
        }




        //instantiate the list of selected BCases
        List<SelectedBCases> selectedBCasesList = new List<SelectedBCases>();

        //instantiate the list of selected BCases
        List<BCases> BCasesList = new List<BCases>();


        // while the prioritization has not been done for all regiments 
        while (regimentsToPrioritize.Count != 0)//
        {


            //get the regiments that has not already be prioritized ( in the "regimentsToPrioritize" list)
            for (int k = 0; k < regimentsToPrioritize.Count; k++)
            {
                GameObject regiment = regimentsToPrioritize[k];
                //Debug.Log("For Regiment " + regiment);

                List<Ground> possibleGrounds = new List<Ground>();

                //get the target of the regiment depending on the target of the player

                Vector3 posToGet = new Vector3 (0,0,0);

                foreach (regimentFormationPosition obj in regimentFormationPositions)
                {
                    if (obj.regiment == regiment) posToGet = target - obj.relativePosition; 
                }

                posToGet.y += 0.5f;
                //then try to get to this ground
                Ground possibleGround = GetComponent<UsefulCombatFunctions>().GetTargetGroundVector(posToGet);
                //Debug.Log("checking possible path for "+possibleGround);
                //if we found a valid path between the two points (avoiding booked positions in end of rounds and in end of the charge)
                Path p = PathSearchComplete(possibleGround, regiment, selectedBCasesList);
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
                    BCasesList.Add(new BCases(regiment, possibleGround, p, numberOfRounds, endOfRoundPosition));
                    break;
                }
                else
                {
                    UnityEngine.Debug.Log("No path found for regiment " + regiment + "going to " + possibleGround);
                }



                

            }
            //Once we have all the BCases, prioritize them by using the delete method
            /*
             * 1) the ones who takes the minimum amount of rounds to get to the point, 
             * 2) then the one who is on the right for player or left for enemy,
             * 3) then the one who has the most speed
             * 4) if there is still 2 regiments that are on the list, take the one who is the lower in the map
             * 5) then just get one for god sake 
             */
            //1st get the minimum rounds in BCases list

            int minRounds = 1000;
            foreach (BCases cas in BCasesList)
            {
                if (cas.numberOfRounds < minRounds) minRounds = cas.numberOfRounds;
                //Debug.Log("minRound is " + minRounds);
            }
            //then delete all BCases that are not on the optimum
            for (int i = 0; i < BCasesList.Count; i++)
            {
                if (BCasesList[i].numberOfRounds != minRounds && BCasesList.Count > 1)
                {
                    BCasesList.Remove(BCasesList[i]);
                    //as we have removed one element of the list, make sure to set i to the previous stage
                    i = i - 1;
                }
            }

            //1st bis get the one who has the less distance 


            float minDistance = 1000f;
            foreach (BCases cas in BCasesList)
            {
                if (Vector3.Distance(cas.regiment.transform.position, cas.possiblePosition.transform.position) < minDistance) minDistance = Vector3.Distance(cas.regiment.transform.position, cas.possiblePosition.transform.position);
                //Debug.Log("minDistance is " + minRounds);
            }
            //then delete all BCases that are not on the optimum
            for (int i = 0; i < BCasesList.Count; i++)
            {
                if (Vector3.Distance(BCasesList[i].regiment.transform.position, BCasesList[i].possiblePosition.transform.position) != minDistance && BCasesList.Count > 1)
                {
                    BCasesList.Remove(BCasesList[i]);
                    //as we have removed one element of the list, make sure to set i to the previous stage
                    i = i - 1;
                }
            }
            //2nd get the right or left max

            if (side == "enemy")
            {

                int maxRight = -50;
                //take the most right regiment
                foreach (BCases cas in BCasesList)
                {
                    if (cas.regiment.transform.position.x > maxRight) maxRight = (int)Mathf.Ceil(cas.regiment.transform.position.x);
                }
                //then delete all BCases that are not on the optimum
                for (int i = 0; i < BCasesList.Count; i++)
                {
                    if (BCasesList[i].regiment.transform.position.x != maxRight && BCasesList.Count > 1)
                    {
                        BCasesList.Remove(BCasesList[i]);
                        i = i - 1;
                    }
                }



            }
            else
            {

                int maxLeft = 50;
                //take the most left regiment
                foreach (BCases cas in BCasesList)
                {
                    if (cas.regiment.transform.position.x < maxLeft) maxLeft = (int)Mathf.Ceil(cas.regiment.transform.position.x);
                }
                //then delete all BCases that are not on the optimum
                for (int i = 0; i < BCasesList.Count; i++)
                {
                    if (BCasesList[i].regiment.transform.position.x != maxLeft && BCasesList.Count > 1)
                    {
                        BCasesList.Remove(BCasesList[i]);
                        i = i - 1;
                    }
                }
            }
            //3rd get the one with max speed (move capacity)
            int maxSpeed = 1;
            foreach (BCases cas in BCasesList)
            {
                if (cas.regiment.GetComponent<CombatVariables>().moveCapacityRegStat > maxSpeed) maxSpeed = cas.regiment.GetComponent<CombatVariables>().moveCapacityRegStat;
            }
            //then delete all BCases that are not on the optimum
            for (int i = 0; i < BCasesList.Count; i++)
            {
                if (BCasesList[i].regiment.GetComponent<CombatVariables>().moveCapacityRegStat != maxSpeed && BCasesList.Count > 1)
                {
                    BCasesList.Remove(BCasesList[i]);
                    i = i - 1;
                }
            }
            //4rd get the one with lower position 
            int lowerPos = 50;
            foreach (BCases cas in BCasesList)
            {
                if (cas.regiment.transform.position.z < lowerPos) lowerPos = (int)Mathf.Ceil(cas.regiment.transform.position.z);
            }
            //then delete all BCases that are not on the optimum
            for (int i = 0; i < BCasesList.Count; i++)
            {
                if (BCasesList[i].regiment.transform.position.z != lowerPos && BCasesList.Count > 1)
                {
                    BCasesList.Remove(BCasesList[i]);
                    i = i - 1;
                }
            }
            //5th fuck it, get one and store it into SelectedBCases List
            //Debug.Log("Getting at the end of prioritization");
            BCases cslct = BCasesList[0];
            //cslct.PrintData();  
            selectedBCasesList.Add(new SelectedBCases(cslct.regiment, cslct.possiblePosition, cslct.pathUsed, cslct.numberOfRounds, cslct.endOfRoundGrounds));
            regimentsToPrioritize.Remove(cslct.regiment);
            BCasesList.Clear();

        }
        // FINAL STEP : Display the path selected putting a path display on each unit , we have to fill the path display gameobject instantiated with the path used in the selected path 
        foreach (SelectedBCases cas in selectedBCasesList)
        {

            //Instantiate a Pathline from the regiment with exact path booked
            GameObject PathInstantiated = Instantiate(PathLine, cas.regiment.transform.position, Quaternion.identity);
            PathInstantiated.GetComponent<PathVariables>().dynamicTarget = false;
            if (side == "enemy")
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
            Instantiate(regimentSlot, position, Quaternion.identity);
            //store the vector 3 path in regiment object 
            cas.regiment.GetComponent<RegimentPath>().regimentPathList = cas.pathUsed.vectorPath;
            cas.regiment.GetComponent<CombatVariables>().inFormation = false;
        }

        selectedBCasesList.Clear();




    }

    public Path PathSearchComplete(Ground possibleGround, GameObject regiment, List<SelectedBCases> selectedBCasesList)
    {
        if (side == "enemy")
        {
            //GraphMaskToUse = GraphMask.FromGraphName("EnemyRegimentAloneGraph");
            GraphMaskToUse = 3;
            GetComponent<Seeker>().graphMask = GraphMaskToUse;
        }
        else
        {
            //GraphMaskToUse = GraphMask.FromGraphName("PlayerRegimentAloneGraph");
            //From graph name not working good : index must be frozen
            //=> concrete example if you delete the Formation movement graph the from graph name on playerRegimentAlone
            //==> fromGRaphname will still be pointing at the 2nd position of the index
            GraphMaskToUse = 2;
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
            p = GetComponent<Seeker>().StartPath(position, target, null, GraphMaskToUse);
            p.BlockUntilCalculated();
            if (p.error)
            {
                UnityEngine.Debug.Log("no path found");
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
                    if (i % numberOfMovesPerRound == 0) endOfRoundPosition.Add(GetComponent<UsefulCombatFunctions>().GetTargetGroundVector(p.vectorPath[i] + new Vector3(0, 1, 0)));
                    //iterate for each position
                    i += 1;
                }
                //check if we have not forgotten the last round position that is the end position of the object
                if (i % numberOfMovesPerRound != 0 && i < p.vectorPath.Count) endOfRoundPosition.Add(GetComponent<UsefulCombatFunctions>().GetTargetGroundVector(p.vectorPath[i] + new Vector3(0, 1, 0)));

                //check if the path created has no obstacles put on it and so the path is completed 
                int obstaclesCreated = 0;
                //Check if the endOfRoundPosition List matches some of the same round's position booked by the selected BCases list, and repeat it 

                foreach (SelectedBCases cs in selectedBCasesList)
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
                    UnityEngine.Debug.Log("Too much test on " + regiment + "for target " + possibleGround);
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


}
public class regimentFormationPosition
{

    public GameObject regiment;
    public Vector3 relativePosition;
    public regimentFormationPosition(GameObject oregiment, Vector3 orelativePosition)
    {
        regiment = oregiment;
        relativePosition = orelativePosition;
    }
}

public class BCases
{

    public GameObject regiment;
    public Ground possiblePosition;
    public Path pathUsed;
    public int numberOfRounds;
    public List<Ground> endOfRoundGrounds = new List<Ground>();

    public BCases(GameObject oregiment, Ground opossiblePosition, Path opathUsed, int onumberOfRounds, List<Ground> oendOfRoundGrounds)
    {
        regiment = oregiment;
        possiblePosition = opossiblePosition;
        pathUsed = opathUsed;
        numberOfRounds = onumberOfRounds;
        endOfRoundGrounds = oendOfRoundGrounds;

    }

    public void PrintData()
    {
        UnityEngine.Debug.Log("BCases class print data called");
        UnityEngine.Debug.Log("BCases regiment :" + regiment);
        UnityEngine.Debug.Log("BCases possible position :" + possiblePosition);
        UnityEngine.Debug.Log("BCases path used :" + pathUsed);
        UnityEngine.Debug.Log("BCases numberOfRounds :" + numberOfRounds);
        UnityEngine.Debug.Log("BCases endOfRoundGrounds :" + endOfRoundGrounds.Count);

    }

}

public class SelectedBCases
{
    //Nota bene : use ground to store the entire position and not the exact position
    public GameObject regiment;
    public Ground positionBooked;
    public Path pathUsed;
    public int numberOfRounds;
    public List<Ground> endOfRoundGrounds = new List<Ground>();
    public string row;
    public SelectedBCases(GameObject oregiment, Ground oPosition, Path opathUsed, int onumberOfRounds, List<Ground> oendOfRoundGrounds)
    {
        regiment = oregiment;
        positionBooked = oPosition;
        pathUsed = opathUsed;
        numberOfRounds = onumberOfRounds;
        endOfRoundGrounds = oendOfRoundGrounds;
        row = positionBooked.transform.parent.name;


    }

    public void PrintData()
    {
        UnityEngine.Debug.Log("BCases class print data called");
        UnityEngine.Debug.Log("BCases regiment :" + regiment);
        UnityEngine.Debug.Log("BCases booked position :" + positionBooked);
        UnityEngine.Debug.Log("BCases path used :" + pathUsed);
        UnityEngine.Debug.Log("BCases numberOfRounds :" + numberOfRounds);
        UnityEngine.Debug.Log("BCases endOfRoundGrounds :" + endOfRoundGrounds.Count);

    }
}

public class BpossibleGroundsDistance
{

    public Ground possibleGround;
    public GameObject regiment;
    public float distanceFromRegiment;
    public string row;
    public BpossibleGroundsDistance(GameObject oregiment, Ground oPossibleGround)
    {
        regiment = oregiment;
        possibleGround = oPossibleGround;
        distanceFromRegiment = Vector3.Distance(regiment.transform.position, possibleGround.transform.position);
        row = possibleGround.transform.parent.name;
    }
}