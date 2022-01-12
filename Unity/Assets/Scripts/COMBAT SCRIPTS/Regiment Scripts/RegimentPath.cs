using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegimentPath : MonoBehaviour
{
    public List<Vector3> regimentPathList = new List<Vector3>();
    public float speedOfMovement;
    public GameObject CombatScripts;
    // Start is called before the first frame update

    private void Update()
    {
        for(int i =0; i < regimentPathList.Count; i++)
        {
            //get the ground p and add +0.5 to its y position to get the real position
            Ground g = CombatScripts.GetComponent<UsefulCombatFunctions>().GetTargetGroundVector(new Vector3(regimentPathList[i].x, 7, regimentPathList[i].z));
            regimentPathList[i] = new Vector3(regimentPathList[i].x , g.transform.position.y + 1, regimentPathList[i].z) ;
        }
    }

    public void GetToNextRoundPosition()
    {
        if (regimentPathList.Count > 0)
        {
            if (Vector3.Distance(transform.position, regimentPathList[0]) < 0.1f)
            {
                regimentPathList.RemoveAt(0);
            }
        }
        
        //foreach node that we can move on on thi round (depends of movesPerRoundGame)
        if (regimentPathList.Count > 0)
        {
            speedOfMovement = CombatScripts.GetComponent<TurnManager>().speedOfMovement;
            // block the turnmanager to get to the next round
            CombatScripts.GetComponent<TurnManager>().finishedTurn = false;
            int NodesPerRound = GetComponent<CombatVariables>().movesPerRoundGame;
            //if we already are at the next waypoint, delete it of the list
            
            int i = 0;
            bool moveAlreadyLaunched = false;
            //loop until all nodes has been touched
            while (i < NodesPerRound)
            {

                //if we are not on the next waypoint and movement has not been launched, launch it 
                if (Vector3.Distance(transform.position, regimentPathList[i]) > 0.1f && !moveAlreadyLaunched)
                {
                    while (Vector3.Distance(transform.position, regimentPathList[i]) > 0.1f)
                    {
                        moveAlreadyLaunched = true;
                        transform.position = Vector3.MoveTowards(transform.position, regimentPathList[i], Time.deltaTime * speedOfMovement);
                    }
                    transform.position = regimentPathList[i];
                    i += 1;
                    moveAlreadyLaunched = false;
                }
                //if we are at the next waypoint and ended our movement, it may seems that there is a duplicate of the previous point or something else, anyway remove it
                else if(Vector3.Distance(transform.position, regimentPathList[i]) < 0.1f && !moveAlreadyLaunched)
                {
                    i += 1;
                }
                

            }
            //delete all the nodes per round
            for (int j = 0; j < NodesPerRound; j++)
            {
                regimentPathList.RemoveAt(j);
            }

            CombatScripts.GetComponent<TurnManager>().finishedTurn = true;
        }
        
    }
}
