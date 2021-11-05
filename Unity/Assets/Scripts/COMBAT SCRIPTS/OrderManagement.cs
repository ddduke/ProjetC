using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManagement : MonoBehaviour
{

    //value to get the actual order (modified by the other order scripts)
    public string actualOrder;

    //find your dropdown menu transform
    public Dropdown dropdownMenu;

    public void OnOrderChange()
    {
        //masterFunction on order change : 

        //stop actual order script 
        StopActualOrder();
        //launch new order
        launchNewOrder(dropdownMenu.value);
    }


    
    //Function to stop the actual order script (from the actual order value, if none stop none)
    public void StopActualOrder()
    {
        if(actualOrder == "BackToFormation")
        {
            GetComponent<DisplayBackToFormation>().StopScript();
        }
        else if(actualOrder == "RegimentCharge")
        {
            GetComponent<DisplayRegimentCharge>().StopScript();
        }
        else if (actualOrder == "FormationCharge")
        {
            GetComponent<DisplayFormationChargeOnCombatMap>().StopScript();
        }
        else if (actualOrder == "FormationMove")
        {
            GetComponent<DisplaysFormationMoveOnCombatMap>().StopScript();
        }


    }

    //Function to launch new order (from the order selected function)
    public void launchNewOrder(int order)
    {
        string side = GetComponent<TurnManager>().turn;
        /*
         * Order value : 
         * 0 = Move In Formation
         * 1 = Charge In Formation
         * 2 = Charge and Break Formation
         */
        //if the order is to be in formation, we need to know if the regiment is already in formation, else we use back in formation script
        if (order == 0) //Order : Move in formation
        {
            //if the regiments are not in formation, display back to formation
            if(!GetComponent<UsefulCombatFunctions>().sideInFormation(side))
            {
                GetComponent<DisplayBackToFormation>().LaunchScript();
            }
            else
            {
                GetComponent<DisplaysFormationMoveOnCombatMap>().LaunchScript();
            }
        }

        if(order ==1) //Order : Charge In Formation
        {
            if (!GetComponent<UsefulCombatFunctions>().sideInFormation(side))
            {
                GetComponent<DisplayBackToFormation>().LaunchScript();
                //function still to create
            }
            else
            {
                GetComponent<DisplayFormationChargeOnCombatMap>().LaunchScript();
            }
        }

        if (order == 2) //Order : Charge and Break Formation 
        {
            GetComponent<DisplayRegimentCharge>().StartScript();
        }


        //if the order is to charge, just launch charge script



    }


}
