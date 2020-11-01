using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : TacticsMove
{


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        // if the unit is not moving, check the mouse and view selectable grounds
        if (!moving)
        {
            FindSelectableGrounds();
            CheckMouse();
        }
        else
        {
            //moving
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
                        //launch the moving function
                        MoveToGround(g);
                    }
                }
            }
        }
    }
}
