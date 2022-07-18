using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    
    
    List<int> unitsList = new List<int>();
    List<string> formationAndUnitTags= new List<string>();
    public int round = 0;
    public string turn = "enemy";
    public Text roundText;
    public Text turnText;
    public bool finishedTurn = true;
    public float  speedOfMovement = 1;

    void Start()
    {
        roundText.text = " Round number " + round;
        turnText.text = turn + " turn";
        ChangeTurn();
        

    }
    void Update()
    {
        //turn manager 
        
        //round 
        //CheckRound();
        roundText.text = " Round number " + round;
        turnText.text = turn + " turn";
        //RoundStop();
    }


    public void GetToNextRound()
    {
        //delete all path and regimentslot existing on the grid 
        GameObject[] existingSlots = GameObject.FindGameObjectsWithTag("RegimentSlot");
        foreach (GameObject slot in existingSlots) GameObject.Destroy(slot);
        GameObject[] existingPathLines = GameObject.FindGameObjectsWithTag("PathLine");
        foreach (GameObject pathLine in existingPathLines) GameObject.Destroy(pathLine);


        List<GameObject> group = new List<GameObject>();
        group = GetAllUnitsBySide(turn);
        foreach (GameObject g in group)
        {
            g.GetComponent<RegimentPath>().GetToNextRoundPosition();
            g.GetComponent<CombatVariables>().CheckRegimentIsInFormation();
           
        }
        if (finishedTurn)
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

    public void ChangeTurn()
    {
        
        //delete all path and regimentslot existing on the grid 
        GameObject[] existingSlots = GameObject.FindGameObjectsWithTag("RegimentSlot");
        foreach (GameObject slot in existingSlots) GameObject.Destroy(slot);
        GameObject[] existingPathLines = GameObject.FindGameObjectsWithTag("PathLine");
        foreach (GameObject pathLine in existingPathLines) GameObject.Destroy(pathLine);

        if (turn == "player")
        {
            turn = "enemy";
            int tmp = GetComponent<OrderManagement>().enemyOrder;
            GetComponent<OrderManagement>().launchNewOrder(tmp);

        }
        else
        {
            turn = "player";
            int tmp = GetComponent<OrderManagement>().playerOrder;
            GetComponent<OrderManagement>().launchNewOrder(tmp);

        }
    }

    /*public void Fight()
    {
        //get the units that are fighting and apply damages
        List<GameObject> enemyAndPlayerGroups = new List<GameObject>();
        List<GameObject> enemiesToFightOnSides = new List<GameObject>();
        enemyAndPlayerGroups = FindenemyAndPlayerUnits();
        foreach (GameObject unit in enemyAndPlayerGroups)
        {
            if(!unit.GetComponent<CombatVariables>().dead) unit.GetComponent<UnitFight>().LaunchUnitFight();
        }

        foreach (GameObject unit in enemyAndPlayerGroups)
        {
            if (!unit.GetComponent<CombatVariables>().dead) unit.GetComponent<CombatVariables>().CheckUnitCombatVariables();
        }

        foreach (GameObject unit in enemyAndPlayerGroups)
        {
            if (unit.GetComponent<CombatVariables>().dead)
            {
                unit.GetComponent<MeshRenderer>().material.color = Color.red;
                unit.GetComponent<UnitMove>().enabled = false;
                unit.transform.position = new Vector3(0, -4, 0);
            }
        }

    }*/

    public List<GameObject> GetAllUnitsBySide(string side)
    {
        List<GameObject> enemyAndPlayerGroups = new List<GameObject>();
        enemyAndPlayerGroups = FindenemyAndPlayerUnits();
        List<GameObject> group = new List<GameObject>();
        foreach (GameObject obj in enemyAndPlayerGroups)
        {
            if (obj.GetComponent<CombatVariables>().enemy && side == "enemy") group.Add(obj);
            if (!obj.GetComponent<CombatVariables>().enemy && side == "player") group.Add(obj);
        }
        return group;
    }

    public List<GameObject> FindenemyAndPlayerUnits()
    {
        List<GameObject> enemyAndPlayerGroups = new List<GameObject>();
        GameObject[] t_unitsList = GameObject.FindGameObjectsWithTag("Regiment");
        foreach (GameObject unit in t_unitsList) enemyAndPlayerGroups.Add(unit);
        return enemyAndPlayerGroups;
    }

    /*void CheckRound()
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
    }*/

    public void Test()
    {
        UnityEngine.Debug.Log("Test");
        
    }
}
