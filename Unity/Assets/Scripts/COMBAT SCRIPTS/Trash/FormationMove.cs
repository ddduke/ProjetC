﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationMove : TacticsMove
{
    /*
    public Ground targetGround;
    public float InFormationMoveCapacity;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        targetGround = GetCurrentGround();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward);

        if (!turn)
        {
            return;
        }
        // if the unit is not moving, check the mouse and view selectable grounds
        if (!moving)
        {
            //display selectable grounds if round is 0
            if (combatScripts.GetComponent<TurnManager>().round == 0)
            { 
                FindSelectableGroundsFormation();
            }
            CheckMouse();
        }
        else
        {
            Move();
        }

    }

    void CheckMouse()
    {
        //if the player clicks
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                //check if the click select a ground
                if (hit.collider.tag == "Ground")
                {
                    Ground g = hit.collider.GetComponent<Ground>();
                    if (g.selectable ==  true)
                    {
                        //set the new target
                        targetGround = g;
                        MoveToGround(g);
                    }
                }
            }
        }
    }
    */
}