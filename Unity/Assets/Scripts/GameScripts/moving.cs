using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class moving : MonoBehaviour
{
    public GameObject POI;
    public int movement_speed;
    public Vector3 POI_position;
    public Vector3 player_position;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("moving est lancé");
        //POI_position = POI.transform.position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Learn to raycast in order to update the actual POI of the player
        RaycastHit hit = new RaycastHit();
        Vector3 direction = new Vector3(transform.position.x, transform.position.y, -1);
        if (Physics.Raycast(transform.position, direction, out hit))
        {
            Debug.Log("RAY");
            Debug.DrawRay(transform.position, Vector3.forward, Color.green);
            GetComponent<moving>().POI = hit.transform.gameObject;
        }
        
        Debug.Log("POIposition equals" + POI_position);
        player_position = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, POI_position, Time.deltaTime * movement_speed);
        if (player_position==POI_position) GetComponent<moving>().enabled = !GetComponent<moving>().enabled;
        

    }


}
