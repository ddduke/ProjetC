using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathVariables : MonoBehaviour
{
    public int movesPerRound = 4;

    private void Update()
    {
        if ( movesPerRound != 4)
        {
            GameObject temp = gameObject.transform.Find("Line1stRound").gameObject;
            temp.GetComponent<DisplayFormationPath>().endDisplayPath = movesPerRound;


            temp = gameObject.transform.Find("Line2ndRound").gameObject;
            temp.GetComponent<DisplayFormationPath>().startDisplayPath = movesPerRound - 1;

        }
    }
}
