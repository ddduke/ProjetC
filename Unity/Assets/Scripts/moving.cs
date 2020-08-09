using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class moving : MonoBehaviour
{
    public GameObject city;
    public int movement_speed;
    public Vector3 city_position;
    public Vector3 player_position;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("moving est lancé");
        //city_position = city.transform.position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Debug.Log("cityposition equals" + city_position);
        player_position = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, city_position, Time.deltaTime * movement_speed);
        if (player_position==city_position) GetComponent<moving>().enabled = !GetComponent<moving>().enabled;

        //Learn to raycast in order to update the actual city of the player 
    }


}
