/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapFunctions : MonoBehaviour
{
    public GameObject[] objs;
    public List<GameObject> all_cities_connected = new List<GameObject>();
    public GameObject Moving_Lines;
    // Goal : create routes between cities and create a list of connected cities for each city
    void CheckRoutes()
    {
        Moving_Lines = Resources.Load("Route") as GameObject;
        //we make a list with all cities existing in the scene
        objs = GameObject.FindGameObjectsWithTag("City");
        //we take a city from the list and compare it to all the other cities in the list
        foreach (GameObject obj in objs)
        {
            foreach (GameObject objc in objs)// we get the city we want to compare from the list
            {
                if (obj == objc) // if it is the same city, just pass 
                {
                    //Debug.Log("it is the same city!");
                    
                }
                else // if it is a different city
                {
                    if (ValidDistanceBetweenPoints(obj, objc)) // if the check distance function return 0
                    {
                        //first we check if the compared objc city is already connected to the original obj city
                        if (obj.GetComponent<City_Variables>().CitiesConnected.Contains(objc));
                        else
                        {
                            // first we define the position in the middle of the 2 cities to create the prefab route here : aka MovingLinesPosition
                            float Lerpratio = 0.5f;
                            Vector3 MovingLinesPosition = Vector3.Lerp(obj.transform.position, objc.transform.position, Lerpratio);
                            // then we define the rotation for the object to face both cities in order to create the route in the right direction
                            Vector3 VectorToTarget = objc.transform.position - obj.transform.position;
                            float angle = Mathf.Atan2(VectorToTarget.x, VectorToTarget.y) * Mathf.Rad2Deg;
                            //Creation of the route object with position & rotation identified previously
                            GameObject NewRoute = Instantiate(Moving_Lines, MovingLinesPosition, Quaternion.Euler(0, 0, -angle));
                            // set the Y scale to the distance of the 2 cities to join them
                            NewRoute.transform.localScale += new Vector3(0, Vector3.Distance(objc.transform.position, obj.transform.position), 0);
                            // set the z position 1 higher than the cities to let the cities visible
                            NewRoute.transform.localPosition += new Vector3(0, 0, 1);

                            //then we set the both cities in the ConnectedCities List of each other
                            obj.GetComponent<City_Variables>().CitiesConnected.Add(objc);
                            objc.GetComponent<City_Variables>().CitiesConnected.Add(obj);
                        }
                    }
                }

            }
            Debug.Log("here is the connected cities for the city " + obj);
            //cities_connected = obj.GetComponent<City_Variables>().CitiesConnected;
            //foreach (GameObject city in cities_connected)
            {
                Debug.Log(city);
            }
        }
            
    }

    // the create route function goal is  to create a gameobject between two gameobjects obj and objc
    //void CreateRoute(GameObject obj, GameObject objc)
    // the distance function check if the distance between two game objects (A & B) is under the maximum distance between cities
    bool ValidDistanceBetweenPoints(GameObject A, GameObject B)
    {
        float dist = Vector3.Distance(A.transform.position, B.transform.position);
        float Maximum_Distance_Between_Cities = GetComponent<GameVariables>().Maximum_Distance_Between_Cities;
        if (dist < Maximum_Distance_Between_Cities) return true;
        else return false;

    }
}
*/