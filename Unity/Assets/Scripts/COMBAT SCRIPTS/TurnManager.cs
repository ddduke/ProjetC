using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    //variables for turn system
    static Dictionary<string, List<TacticsMove>> units = new Dictionary<string, List<TacticsMove>>();
    static Queue<string> turnKey = new Queue<string>();
    static Queue<TacticsMove> turnTeam = new Queue<TacticsMove>();
    
    //variables for the turn 
    List<int> unitsList = new List<int>();
    public int round = 0;
    public Text roundText;

    void Start()
    {
        roundText.text = " Round number " + round;
    }
    void Update()
    {
        //turn manager 
        
        //round 
        //CheckRound();
        roundText.text = " Round number " + round;
        //RoundStop();
    }

    void CheckRound()
    {
        GameObject[] t_unitsList = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject unit in t_unitsList)
        {
            if (unit.GetComponent<UnitMove>().moving) unitsList.Add(unit.GetComponent<UnitMove>().actualRound);
        }
        if (unitsList.Count > 0)
        {
            round = unitsList.Min();
            roundText.text = " Round number " + round;
            //Debug.Log("numero de round" + round);
        }
        unitsList.Clear();
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

    public void ChangeTurn()
    {
        
    }


}
