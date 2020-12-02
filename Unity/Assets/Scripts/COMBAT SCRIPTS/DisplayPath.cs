using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DisplayPath : MonoBehaviour
{
    public Camera cam;
    public LineRenderer pathLine;
    public Vector3 target;
    private UnityEngine.AI.NavMeshPath path;
    private float elapsed = 0.0f;
    void Start()
    {
        path = new UnityEngine.AI.NavMeshPath();
        elapsed = 0.0f;
    }


    void Update()
    {


        // Update the way to the goal every second.
        elapsed += Time.deltaTime;
        if (elapsed > 1.0f)
        {
            
            elapsed -= 1.0f;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.tag == "Ground")
            {
                Ground g = hit.collider.GetComponent<Ground>();
                target = g.transform.position;
                target.y += 0.5f;
            }
            UnityEngine.AI.NavMesh.CalculatePath(transform.position, target, UnityEngine.AI.NavMesh.AllAreas, path);
        }
        for (int i = 0; i < path.corners.Length - 1; i++)
            pathLine.SetPosition(i,path.corners[i]);
    }
}
