using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnlightGround : MonoBehaviour
{
    void OnMouseOver()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    void OnMouseExit()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
    }
}
