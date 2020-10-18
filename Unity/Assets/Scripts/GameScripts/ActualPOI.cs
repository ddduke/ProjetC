using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ActualPOI : MonoBehaviour
{
    public GameObject PlayerPOI;
    // Start is called before the first frame update
    void Start()
    {
        GameObject StartPOI = GameObject.Find("Bordeaux");
        transform.position = new Vector3(StartPOI.transform.position.x, StartPOI.transform.position.y,8) ;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit = new RaycastHit();
        Vector3 direction = new Vector3(0, 0, -7);
        Debug.DrawRay(transform.position, direction, Color.green);
        if (Physics.Raycast(transform.position, direction, out hit))
        {
            Debug.Log("RAY");
            GetComponent<ActualPOI>().PlayerPOI = hit.transform.gameObject;
        }
    }
}
