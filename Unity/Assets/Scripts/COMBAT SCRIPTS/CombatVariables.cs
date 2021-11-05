using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatVariables : MonoBehaviour
{
    //variables for the game design
    //Moves per round is the standard move per round autorisation for this unit (not modified except by winning experience tier etc)
    public GameObject combatScripts;
    public int movesPerRoundGame = 1;
    //MoveCapacity is the actual move capacity for the unit, so it can be less or more depending on actual buffers
    public int moveCapacityRegStat = 1;
    public int range = 1;
    public int rangeHeight = 2;
    public int people = 3;
    public int healthByPeople = 10;
    public int damageByPeople = 5;
    public int defenseByPeople = 3;
    public bool enemy = true;

    public int totalHealth;

    public bool dead = false;

    public bool inFormation = true;

    void Start()
    {
        combatScripts = GameObject.Find("CombatScripts").transform.gameObject;
        totalHealth = healthByPeople * people;
    }

    public void CheckUnitCombatVariables()
    {
        float relativePeopleNumber = (float)totalHealth / healthByPeople;
        //Debug.Log(relativePeopleNumber);
        people = (int)Mathf.Ceil(relativePeopleNumber);
        if (people <= 0) dead = true;
    }
    /// <summary>
    /// Check if the actual position of the regiment is its position in formation or not
    /// </summary>
    public void CheckRegimentIsInFormation()
    {
        //check if the actual position of the object
        if (combatScripts.GetComponent<UsefulCombatFunctions>().IsInFormation(transform.gameObject))
        {
            inFormation = true;
        }
        else inFormation = false;
    }
}
