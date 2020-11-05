using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    //ground variables : in order to know if the ground is our current position, targeted, walkable...
    public bool current = false;
    public bool target = false;
    public bool selectable = false;
    public bool walkable = true;

    //using a list to get all the adjacency grounds 
    public List<Ground> adjacencyList = new List<Ground>();

    //variables for our adjacency list for BFS = Breath FIrst Search Algorithm
    public bool visited = false;
    public Ground parent = null; 
    public int distance = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the ground has to be specific color for the player
        if(current)
        {
            GetComponent<MeshRenderer>().material.color = Color.magenta;
        }
        else if (target)
        {
            GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else if (selectable)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }

    //used to reset all variables of the ground at each turn
    public void Reset()
    {

        adjacencyList.Clear();
        current = false;
        target = false;
        //selectable = false;
        walkable = true;

        visited = false;
        parent = null;
        distance = 0;
    }

    //void used to get the neighbors and check if they are selectable to walk on
    public void FindNeighbors(float jumpHeight)
    {

        Reset();
        CheckGround(Vector3.forward, jumpHeight);
        CheckGround(-Vector3.forward, jumpHeight);
        CheckGround(Vector3.right, jumpHeight);
        CheckGround(Vector3.left, jumpHeight);

    }

    //void used to check if the neighbor is ground and has no unit on it
    public void CheckGround(Vector3 direction, float jumpHeight)
    {
        //cube of 0.5 x and z and height of a jumpheight to check if it is walkable
        Vector3 halfExtends = new Vector3(0.25f,(1+jumpHeight/2),0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtends);
        //once we get all objects collided in the cube, we get the ground type and if it is walkable we add it to the adjacency list
        foreach(Collider item in colliders)
        {
            Ground ground = item.GetComponent<Ground>();
            if (ground != null && ground.walkable)
            {
                RaycastHit hit;

                //check if there is an object upwards of the ground
                if (!Physics.Raycast(ground.transform.position, Vector3.up, out hit, 1))
                {
                    adjacencyList.Add(ground);
                }
                //check if the thing blocking the ray is a unit (moveable) or the formation
                else if (hit.transform.gameObject.tag == "Unit" || hit.transform.gameObject.tag == "Formation")
                {
                    adjacencyList.Add(ground);
                }
            }
        }
    
    }
}
