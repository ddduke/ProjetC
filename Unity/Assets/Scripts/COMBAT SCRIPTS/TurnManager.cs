using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    List<int> units = new List<int>();
    public int round = 0;
    public Text turnText;
    // Update is called once per frame

    void Start()
    {
        turnText.text = " Round number " + round;
    }
    void Update()
    {
        //CheckRound();
        turnText.text = " Round number " + round;
        //RoundStop();
    }

    void CheckRound()
    {
        GameObject[] t_units = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject unit in t_units)
        {
            if (unit.GetComponent<UnitMove>().moving) units.Add(unit.GetComponent<UnitMove>().actualRound);
        }
        if (units.Count > 0)
        {
            round = units.Min();
            turnText.text = " Round number " + round;
            //Debug.Log("numero de round" + round);
        }
        units.Clear();
    }

    public void GetToNextRound()
    {
        if (round < 4)
        {
            round += 1;
        }
        else
        {
            round = 0;
        }
    }
}
