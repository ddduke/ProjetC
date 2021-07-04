using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public void OnButtonPress()
    {
        GameObject Button = GameObject.Find("ButtonGo(Clone)"); ;
        GameObject.Destroy(Button);
        GameObject player = GameObject.Find("Player");
        if (player == null) UnityEngine.Debug.Log("player not found");
        UnityEngine.Debug.Log("Button pressed");
        player.GetComponent<moving>().enabled = !player.GetComponent<moving>().enabled;
    }
}
