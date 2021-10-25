using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMouseHit : MonoBehaviour
{
    public Camera cam;

    public void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            Debug.Log(hit.collider.tag);
        }
    }
    
    
    
}
