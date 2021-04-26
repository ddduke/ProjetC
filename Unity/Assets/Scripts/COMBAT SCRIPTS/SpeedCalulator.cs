using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedCalulator : MonoBehaviour
{
    public GameObject CombatScripts;
    public string side;
    bool previousValue;
    int previousValue2;
    void Start()
    {
        previousValue = gameObject.GetComponent<CombatVariables>().inFormation;
        previousValue2 = gameObject.GetComponent<CombatVariables>().moveCapacityRegStat;
        if (gameObject.GetComponent<CombatVariables>().enemy) side = "enemy";
        else side = "player";
        CheckSpeed();
    }

    void Update()
    {
        //if the variable in formation has changed
        if (gameObject.GetComponent<CombatVariables>().inFormation != previousValue)
        {
            previousValue = gameObject.GetComponent<CombatVariables>().inFormation;
            CheckSpeed();
        }
        //if the variable moveCapacity has changed
        if (gameObject.GetComponent<CombatVariables>().moveCapacityRegStat != previousValue2)
        {
            previousValue2 = gameObject.GetComponent<CombatVariables>().moveCapacityRegStat;
            CheckSpeed();
        }
    }

    void CheckSpeed()
    {
        if (gameObject.GetComponent<CombatVariables>().inFormation)
        {

            List<GameObject> regimentsList = new List<GameObject>();
            regimentsList = CombatScripts.GetComponent<TurnManager>().GetAllUnitsBySide(side);
            //get the minimum moves per round in the team
            int minMovesPerRound = 100;
            foreach (GameObject reg in regimentsList)
            {
                if ((int)reg.GetComponent<CombatVariables>().moveCapacityRegStat < minMovesPerRound) minMovesPerRound = (int)reg.GetComponent<CombatVariables>().moveCapacityRegStat;
            }
            gameObject.GetComponent<CombatVariables>().movesPerRoundGame = minMovesPerRound;
        }
        else if (!gameObject.GetComponent<CombatVariables>().inFormation)
        {
            GetComponent<CombatVariables>().movesPerRoundGame = GetComponent<CombatVariables>().moveCapacityRegStat;
        }
    }
}
