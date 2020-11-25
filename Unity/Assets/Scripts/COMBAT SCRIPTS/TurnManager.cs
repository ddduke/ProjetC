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

        if (turn == "Player")
        {
            turn = "Enemy";
            List<GameObject> enemyGroup = new List<GameObject>();
            List<GameObject> playerGroup = new List<GameObject>();
            enemyGroup = GetAllGameObjectsBySide("enemy");
            foreach (GameObject enemy in enemyGroup)
            {
                enemy.GetComponent<TacticsMove>().turn = true;
            }
            playerGroup = GetAllGameObjectsBySide("player");
            foreach (GameObject player in playerGroup)
            {
                player.GetComponent<TacticsMove>().turn = false;
            }

        }
        else
        {
            turn = "Player";
            List<GameObject> enemyGroup = new List<GameObject>();
            List<GameObject> playerGroup = new List<GameObject>();
            enemyGroup = GetAllGameObjectsBySide("enemy");
            foreach (GameObject enemy in enemyGroup)
            {
                enemy.GetComponent<TacticsMove>().turn = false;
            }
            playerGroup = GetAllGameObjectsBySide("player");
            foreach (GameObject player in playerGroup)
            {
                player.GetComponent<TacticsMove>().turn = true;
            }

        }
    }

    public void Fight()
    {
        //get the units that are fighting and apply damages
        List<GameObject> enemyAndPlayerGroups = new List<GameObject>();
        List<GameObject> enemiesToFightOnSides = new List<GameObject>();
        enemyAndPlayerGroups = FindenemyAndPlayerUnits();
        foreach (GameObject unit in enemyAndPlayerGroups)
        {
            unit.GetComponent<UnitFight>().LaunchUnitFight();

            unit.GetComponent<CombatVariables>().CheckUnitCombatVariables();

        }
    }

    public List<GameObject> GetAllGameObjectsBySide(string side)
    {
        List<GameObject> enemyAndPlayerGroups = new List<GameObject>();
        enemyAndPlayerGroups = FindenemyAndPlayerGroups();
        List<GameObject> group = new List<GameObject>();
        foreach (GameObject obj in enemyAndPlayerGroups)
        {
            if (obj.GetComponent<TacticsMove>().enemy && side == "enemy") group.Add(obj);
            if (!obj.GetComponent<TacticsMove>().enemy && side == "player") group.Add(obj);
        }
        return group;
    }

    public List<GameObject> FindenemyAndPlayerGroups()
    {
        List<GameObject> enemyAndPlayerGroups = new List<GameObject>();
        GameObject[] t_unitsList = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject unit in t_unitsList) enemyAndPlayerGroups.Add(unit);
        GameObject[] t_formationsList = GameObject.FindGameObjectsWithTag("Formation");
        foreach (GameObject formation in t_formationsList) enemyAndPlayerGroups.Add(formation);
        return enemyAndPlayerGroups;
    }

    public List<GameObject> GetAllUnitsBySide(string side)
    {
        List<GameObject> enemyAndPlayerGroups = new List<GameObject>();
        enemyAndPlayerGroups = FindenemyAndPlayerUnits();
        List<GameObject> group = new List<GameObject>();
        foreach (GameObject obj in enemyAndPlayerGroups)
        {
            if (obj.GetComponent<TacticsMove>().enemy && side == "enemy") group.Add(obj);
            if (!obj.GetComponent<TacticsMove>().enemy && side == "player") group.Add(obj);
        }
        return group;
    }

    public List<GameObject> FindenemyAndPlayerUnits()
    {
        List<GameObject> enemyAndPlayerGroups = new List<GameObject>();
        GameObject[] t_unitsList = GameObject.FindGameObjectsWithTag("Unit");
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
}
