using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class moving : MonoBehaviour
{
    public int movement_speed;
    public Vector3 POI_position;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Debug.Log("moving est lancé");
        //POI_position = POI.transform.position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UnityEngine.Debug.Log("POIposition equals" + POI_position);
        Vector3 player_position = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(POI_position.x, POI_position.y, 8), Time.deltaTime * movement_speed);
        if (player_position == POI_position)
        {
            GetComponent<moving>().enabled = !GetComponent<moving>().enabled;
        }//Learn to raycast in order to update the actual POI of the player
       
        

        

    }


}
