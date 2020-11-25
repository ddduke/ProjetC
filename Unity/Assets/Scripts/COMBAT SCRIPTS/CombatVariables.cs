using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatVariables : MonoBehaviour
{
    //variables for the game design
    public float movesPerRound = 1;
    public float moveCapacity = 1;
    public int range = 1;
    public int rangeHeight = 2;
    public int people = 3;
    public int healthByPeople = 10;
    public int damageByPeople = 5;
    public int defenseByPeople = 3;

    public int totalHealth;

    public bool dead = false;

    public bool inFormation = true;
    public bool chargeAndBreakFormation = false;

    void Start()
    {
        totalHealth = healthByPeople * people;
    }

    public void CheckUnitCombatVariables()
    {
        float relativePeopleNumber = (float)totalHealth / healthByPeople;
        //Debug.Log(relativePeopleNumber);
        people = (int)Mathf.Ceil(relativePeopleNumber);
        if (people <= 0) dead = true;
    }
}
