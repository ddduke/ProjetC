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
    public string turn = "Enemy";
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
        List<GameObject> group = new List<GameObject>();
        group = GetAllUnitsBySide(turn);
        foreach (GameObject g in group)
        {
            g.GetComponent<RegimentPath>().GetToNextRoundPosition();
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

        if (turn == "player")
        {
            turn = "enemy";
            List<GameObject> enemyGroup = new List<GameObject>();
            List<GameObject> playerGroup = new List<GameObject>();
            enemyGroup = GetAllUnitsBySide("enemy");
            foreach (GameObject enemy in enemyGroup)
            {
                //enemy.GetComponent<TacticsMove>().turn = true;
            }
            playerGroup = GetAllUnitsBySide("player");
            foreach (GameObject player in playerGroup)
            {
                // player.GetComponent<TacticsMove>().turn = false;
            }

        }
        else
        {
            turn = "player";
            List<GameObject> enemyGroup = new List<GameObject>();
            List<GameObject> playerGroup = new List<GameObject>();
            enemyGroup = GetAllUnitsBySide("enemy");
            foreach (GameObject enemy in enemyGroup)
            {
                //enemy.GetComponent<TacticsMove>().turn = false;
            }
            playerGroup = GetAllUnitsBySide("player");
            foreach (GameObject player in playerGroup)
            {
                //player.GetComponent<TacticsMove>().turn = true;
            }

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
        Debug.Log("Test");
        
    }
}
