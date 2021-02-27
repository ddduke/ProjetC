using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

public class DisplayPath : MonoBehaviour
{
    public Camera cam;
    public LineRenderer pathLine;
    public Vector3 target;
    public int startDisplayPath;
    public int endDisplayPath;
    public GameObject CombatScripts;
    //private UnityEngine.AI.NavMeshPath path;
    private float elapsed = 0.0f;
    void Start()
    {
        //path = new UnityEngine.AI.NavMeshPath();
        elapsed = 0.0f;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        CombatScripts = GameObject.Find("CombatScripts").transform.gameObject;

    }


    void Update()
    {

        if (gameObject.transform.parent.GetComponent<PathVariables>().dynamicTarget)
        {
            // Update the way to the goal every second.
            elapsed += Time.deltaTime;
            if (elapsed > 0.1f)
            {

                elapsed -= 0.1f;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                // get the target position of the mouse
                if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.tag == "Ground")
                {
                    Ground g = hit.collider.GetComponent<Ground>();
                    target = g.transform.position;
                    target.y += 0.5f;
                }
                AstarPath.active.Scan();
            }
        }
        else
        {
            target = gameObject.transform.parent.GetComponent<PathVariables>().staticTarget;
            target.y += 0.5f;
        }

        //check if the target is reachable or recalculate it based on max
        string side = CombatScripts.GetComponent<TurnManager>().turn;
        //check if reaches the top of the map, and if it is, correct the target 
        if (target.z + CombatScripts.GetComponent<UsefulCombatFunctions>().GetMaxZ(side) > 6) target.z = 6 - CombatScripts.GetComponent<UsefulCombatFunctions>().GetMaxZ(side);
        //check if reaches the bottom of the map, and if it is, correct the target 
        if (target.z + CombatScripts.GetComponent<UsefulCombatFunctions>().GetMinZ(side) < -6) target.z = -6 - CombatScripts.GetComponent<UsefulCombatFunctions>().GetMinZ(side);

        //Get the seeker of GO, calculate path from the parent position (regiment) to the target
        Path p = GetComponent<Seeker>().StartPath(transform.position, target);
        p.BlockUntilCalculated();
        // check if the path displayed is too large for the line (that can be 1st tour or 2nd Tour), in this case we reduce the path to the length of the line
        if (endDisplayPath < p.vectorPath.Count)
        {
            pathLine.SetVertexCount(endDisplayPath - startDisplayPath);
            int vertexPosition = 0;
            for (int i = startDisplayPath; i < endDisplayPath; i++)
            {
                Vector3 tmp = p.vectorPath[i];
                tmp.y += 0.2f;
                pathLine.SetPosition(vertexPosition, tmp);
                Debug.Log(p.vectorPath[i]);
                vertexPosition++;
            }
        }
        //and finally we check if the line (that still can be 1st Tour or 2nd Tour) is starting at some point or if the path is too small to display anything
        else if (startDisplayPath < p.vectorPath.Count)
        {
            pathLine.SetVertexCount(p.vectorPath.Count - startDisplayPath);
            int vertexPosition = 0;
            for (int i = startDisplayPath; i < p.vectorPath.Count; i++)
            {
                Vector3 tmp = p.vectorPath[i];
                tmp.y += 0.2f;
                pathLine.SetPosition(vertexPosition, tmp);
                Debug.Log(p.vectorPath[i]);
                vertexPosition++;
            }
        }
        
    }
}
