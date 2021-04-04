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
    private GraphMask graph;
    void Start()
    {
        //path = new UnityEngine.AI.NavMeshPath();
        elapsed = 0.0f;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        CombatScripts = GameObject.Find("CombatScripts").transform.gameObject;

    }


    void Update()
    {
        GetComponent<Seeker>().graphMask = gameObject.transform.parent.GetComponent<PathVariables>().GraphMaskToUse;
        Path p = null;
        // Check if activated for a dynamic target (mouse position) or a static target (exact point on the graph).
        if (gameObject.transform.parent.GetComponent<PathVariables>().dynamicTarget)
        {
            // Update the way to the goal every second.
            elapsed += Time.deltaTime;
            if (elapsed > 0.1f)
            {
                GetComponent<Seeker>().graphMask = gameObject.transform.parent.GetComponent<PathVariables>().GraphMaskToUse;
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
            //check if the target is reachable or recalculate it based on max
            target.z = CombatScripts.GetComponent<UsefulCombatFunctions>().CorrectTargetZ(target.z);
            target.x = CombatScripts.GetComponent<UsefulCombatFunctions>().CorrectTargetX(target.x);
            //Get the seeker of GO, calculate path from the parent position (regiment) to the target

            p = GetComponent<Seeker>().StartPath(transform.position, target);
            p.BlockUntilCalculated();
        }
        // if we already have an exact point to display path on, we juste set it as our target
        else if (gameObject.transform.parent.GetComponent<PathVariables>().pathInjected == null)
        {
            target = gameObject.transform.parent.GetComponent<PathVariables>().staticTarget;
            target.y += 0.5f;
            //check if the target is reachable or recalculate it based on max
            target.z = CombatScripts.GetComponent<UsefulCombatFunctions>().CorrectTargetZ(target.z);
            target.x = CombatScripts.GetComponent<UsefulCombatFunctions>().CorrectTargetX(target.x);
            //Get the seeker of GO, calculate path from the parent position (regiment) to the target

            p = GetComponent<Seeker>().StartPath(transform.position, target);
            p.BlockUntilCalculated();
        }
        // if we already have a path injected in the 
        else if (gameObject.transform.parent.GetComponent<PathVariables>().pathInjected != null)
        {
            p = gameObject.transform.parent.GetComponent<PathVariables>().pathInjected;
        }

        
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
                //Debug.Log(p.vectorPath[i]);
                vertexPosition++;
            }
        }
        
    }
}
