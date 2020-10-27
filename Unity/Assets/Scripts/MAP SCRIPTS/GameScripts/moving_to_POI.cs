using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class moving_to_POI : MonoBehaviour
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
        if (POIIsConnected())
        {
            GameObject player = GameObject.Find("Player");
            GameObject Button = Resources.Load("ButtonGo") as GameObject;
            GameObject MyCanvas = GameObject.Find("Canvas");
            if (Button == null) Debug.Log("Button not found");
            else Debug.Log("Button found");
            GameObject GOButton = Instantiate(Button);
            GOButton.transform.SetParent(MyCanvas.transform);
            GOButton.transform.position = transform.position + new Vector3(0, 1, 0);
            GOButton.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            //set the coodinates of the gameobject of the POI for the player POIposition variable and the actual POI of the player;
            player.GetComponent<moving>().POI_position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }

    }

    public List<GameObject> POIsConnectedToPlayer = new List<GameObject>();
    bool POIIsConnected()
    {
        GameObject player = GameObject.Find("Player");
        if (player == null) Debug.Log("player not found");
        GameObject PlayerPOI = player.GetComponent<ActualPOI>().PlayerPOI;
        POIsConnectedToPlayer = PlayerPOI.GetComponent<POI_Variables>().POIsConnected;
        foreach (GameObject POI in POIsConnectedToPlayer)
        {
            if (POI == gameObject) return true;
        }
        return false;
    }
        
        
}

