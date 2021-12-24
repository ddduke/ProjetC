using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextRound : MonoBehaviour
{
    // Start is called before the first frame update
    public void GetToNextRound()
    {
        if (GetComponent<TurnManager>().round<4)
        {
            GetComponent<TurnManager>().round += 1;
        }
        else
        {
            GetComponent<TurnManager>().round = 0;
        }
    }
}
