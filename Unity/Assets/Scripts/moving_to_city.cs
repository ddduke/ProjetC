using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class moving_to_city : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {


    }
    // Update is called once per frame
    void FixedUpdate()
    {

    }

    void OnMouseDown()
    {
        if (CityIsConnected())
        {
            GameObject player = GameObject.Find("Player");
            if (player == null) Debug.Log("player not found");
            GameObject Button = Resources.Load("ButtonGo") as GameObject;
            GameObject MyCanvas = GameObject.Find("Canvas");
            if (Button == null) Debug.Log("Button not found");
            else Debug.Log("Button found");
            GameObject GOButton = Instantiate(Button);
            GOButton.transform.SetParent(MyCanvas.transform);
            GOButton.transform.position = transform.position + new Vector3(0, 1, 0);
            GOButton.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            //set the coodinates of the gameobject of the city for the player cityposition variable and the actual city of the player;
            player.GetComponent<moving>().city_position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }

    }

    public List<GameObject> CitiesConnectedToPlayer = new List<GameObject>();
    bool CityIsConnected()
    {
        GameObject player = GameObject.Find("Player");
        if (player == null) Debug.Log("player not found");
        GameObject PlayerCity = player.GetComponent<moving>().city;
        CitiesConnectedToPlayer = PlayerCity.GetComponent<City_Variables>().CitiesConnected;
        foreach (GameObject City in CitiesConnectedToPlayer)
        {
            if (City == gameObject) return true;
        }
        return false;
    }
        
        
}
