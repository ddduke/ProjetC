using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathVariables : MonoBehaviour
{
    public int movesPerRound = 1;
    public Vector3 staticTarget;
    public bool dynamicTarget = false;
    private void Update()
    {
        if ( movesPerRound != 1)
        {
            GameObject temp = gameObject.transform.Find("Line1stRound").gameObject;
            temp.GetComponent<DisplayPath>().endDisplayPath = movesPerRound * 4;


            temp = gameObject.transform.Find("Line2ndRound").gameObject;
            temp.GetComponent<DisplayPath>().startDisplayPath = movesPerRound * 4 - 1;

        }
    }
}
