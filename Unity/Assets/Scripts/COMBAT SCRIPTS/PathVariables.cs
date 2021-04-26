using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PathVariables : MonoBehaviour
{
    public int movesPerRound = 1;
    public Vector3 staticTarget;
    public bool dynamicTarget = false;
    public string GraphStringToUse;
    public GraphMask GraphMaskToUse;
    public Path pathInjected = null;
    public List<Vector3> PathOfGO;
    public bool pathCalculated;
    private void Update()
    {
        
        GraphMaskToUse = GraphMask.FromGraphName(GraphStringToUse);
        
        if ( movesPerRound != 1)
        {
            GameObject temp = gameObject.transform.Find("Line1stRound").gameObject;
            temp.GetComponent<DisplayPath>().endDisplayPath = movesPerRound * 4;


            temp = gameObject.transform.Find("Line2ndRound").gameObject;
            temp.GetComponent<DisplayPath>().startDisplayPath = movesPerRound * 4 - 1;

        }
    }
}
