using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    List<int> units = new List<int>();
    public int round;
    public Text turnText;
    // Update is called once per frame
    void Update()
    {
        CheckRound();
    }

    void CheckRound()
    {
        GameObject[] t_units = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject unit in t_units)
        {
            units.Add(unit.GetComponent<UnitMove>().actualRound);
        }
        round = units.Min();
        turnText.text = " Round number " + round;
        units.Clear();
    }
}
