using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class UnitMovingToPoint : MonoBehaviour
{

    public Camera cam;

    public NavMeshAgent agent;

    Vector3 relativeGroundPosition;

    void Start()
    {
        Ground currentGround = GetCurrentGround();
        relativeGroundPosition = transform.position - currentGround.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.tag == "Ground")
            {
                Ground g = hit.collider.GetComponent<Ground>();
                Vector3 position = g.transform.position;
                position += relativeGroundPosition;
                agent.SetDestination(position);
            }
        }
        
    }

    public Ground GetCurrentGround()
    {
        Ground currentGround = GetTargetGround(gameObject);
        return currentGround;

    }

    public Ground GetTargetGround(GameObject target)
    {
        LayerMask layer_mask = LayerMask.GetMask("Ground");
        RaycastHit hit;
        Ground ground = null;
        if (Physics.Raycast(target.transform.position, -Vector3.up, out hit, 1, layer_mask))
        {
            ground = hit.collider.GetComponent<Ground>();
        }

        return ground;
    }
}
