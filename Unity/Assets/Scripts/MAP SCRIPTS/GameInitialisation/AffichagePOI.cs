using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffichagePOI : MonoBehaviour
{
    /* 
    Author : Kevin Duret
    Last Update : 19/08/2020
    Object of the class : Visually enhance the POI which is under the cursor of the mouse and show the connected POIs
    Macro-working : 
    1) When the player put the cursor over the point of interest
    2) When the player's cursor exit the point of interest
     */
   
    //Function OnMouseOver activates when the cursor of the player is over the point of interest 
    public void OnMouseOver()
    {
	    //enhancing the point by making its scale at 0.7 (initial scale 0.5)
        transform.localScale = new Vector3(0.7f, 0.7f, 0);
        //Get the list of the point of interests connected to the actual point
        List<GameObject> TempList = new List<GameObject>();
        TempList = GetComponent<POI_Variables>().POIsConnected;
        //for every point connected, get the line (component linerenderer)and set it with 2 positions (between the actual point and the connected point)
        foreach (GameObject POI in TempList)
        {
            LineRenderer Line = POI.GetComponent<LineRenderer>();
            Line.positionCount = 2;
            Line.SetColors(Color.black, Color.black);
            Line.SetWidth(0.1f,0.1f);
            Line.SetPosition(0, POI.transform.position);
            Line.SetPosition(1, transform.position);
        }
    }
    //Function OnMouseExit activate when the cursor of the player exit the point of interest
    public void OnMouseExit()
    {
        //set the point to its initial scale (0.5)
        transform.localScale = new Vector3(0.5f, 0.5f, 0);
        //Get the list of the point of interests connected to the actual point
        List<GameObject> TempList = new List<GameObject>();
        TempList = GetComponent<POI_Variables>().POIsConnected;
        //for every point we get the line (linerenderer) component and set the number of points to 0 (no line)
        foreach (GameObject POI in TempList)
        {
            LineRenderer Line = POI.GetComponent<LineRenderer>();
            Line.positionCount = 2;
            Line.positionCount = 0;
        }
    }


}
