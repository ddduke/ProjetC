using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldFunctions : MonoBehaviour
{

    public GameObject Moving_Lines;

    //Create an object route between 2 points 
    void CreateRoute(GameObject A, GameObject B)
    {
        Moving_Lines = Resources.Load("Route") as GameObject;
        // first we define the position in the middle of the 2 cities to create the prefab route here : aka MovingLinesPosition
        float Lerpratio = 0.5f;
        Vector3 MovingLinesPosition = Vector3.Lerp(A.transform.position, B.transform.position, Lerpratio);
        // then we define the rotation for the object to face both cities in order to create the route in the right direction
        Vector3 VectorToTarget = B.transform.position - A.transform.position;
        float angle = Mathf.Atan2(VectorToTarget.x, VectorToTarget.y) * Mathf.Rad2Deg;
        //Creation of the route object with position & rotation identified previously
        GameObject NewRoute = Instantiate(Moving_Lines, MovingLinesPosition, Quaternion.Euler(0, 0, -angle));
        // set the Y scale to the distance of the 2 cities to join them
        NewRoute.transform.localScale += new Vector3(0, Vector3.Distance(B.transform.position, A.transform.position), 0);
        // set the z position 1 higher than the cities to let the cities visible
        NewRoute.transform.localPosition += new Vector3(0, 0, 0.5f);

    }


}
