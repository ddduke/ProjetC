using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffichageCity : MonoBehaviour
{
   
    public void OnMouseOver()
    {
	//hello
        Debug.Log("entrée");
        transform.localScale = new Vector3(0.7f, 0.7f, 0);
        List<GameObject> TempList = new List<GameObject>();
        TempList = GetComponent<City_Variables>().CitiesConnected;
        foreach (GameObject City in TempList)
        {
            LineRenderer Line = City.GetComponent<LineRenderer>();
            Line.positionCount = 2;
            Line.SetColors(Color.black, Color.black);
            Line.SetWidth(0.1f,0.1f);
            Line.SetPosition(0, City.transform.position);
            Line.SetPosition(1, transform.position);
        }
    }

    public void OnMouseExit()
    {
        Debug.Log("sortie");
        transform.localScale = new Vector3(0.5f, 0.5f, 0);
        List<GameObject> TempList = new List<GameObject>();
        TempList = GetComponent<City_Variables>().CitiesConnected;
        foreach (GameObject City in TempList)
        {
            LineRenderer Line = City.GetComponent<LineRenderer>();
            Line.positionCount = 2;
            Line.positionCount = 0;
        }
    }


}
