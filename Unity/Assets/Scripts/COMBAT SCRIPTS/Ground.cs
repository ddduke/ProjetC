﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    //ground variables : in order to know if the ground is our current position, targeted, walkable...
    
    public bool mouseOver = false;
    public bool range = false;
    public string row;

    //using a list to get all the adjacency grounds 
    public List<Ground> adjacencyList = new List<Ground>();

    //variables for our adjacency list for BFS = Breath FIrst Search Algorithm
    public bool visited = false;
    public Ground parent = null; 
    public int distance = 0;

    // Start is called before the first frame update
    void Start()
    {
        row = transform.parent.name;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the ground has to be specific color for the player
        
        if (mouseOver)
        {
            GetComponent<MeshRenderer>().material.color = Color.black;
        }
        else if (range)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }

    void OnMouseOver()
    {
        mouseOver = true;
    }

    void OnMouseExit()
    {
        mouseOver = false;
    }

    //used to reset all variables of the ground at each turn
    public void Reset()
    {

        adjacencyList.Clear();

        visited = false;
        parent = null;
        distance = 0;
    }

    


    //void used to get the neighbors and check if they are selectable to walk on
    public void FindNeighbors(float MaxHeight)
    {

        Reset();
        CheckGround(Vector3.forward, MaxHeight);
        CheckGround(-Vector3.forward, MaxHeight);
        CheckGround(Vector3.right, MaxHeight);
        CheckGround(Vector3.left, MaxHeight);

    }

    //void used to check if the neighbor is ground and has no unit on it
    public void CheckGround(Vector3 direction, float MaxHeight)
    {
        //cube of 0.5 x and z and height of a MaxHeight to check if it is walkable
        Vector3 halfExtends = new Vector3(0.25f,(1+MaxHeight/2),0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + direction, halfExtends);
        //once we get all objects collided in the cube, we get the ground type and if it is walkable we add it to the adjacency list
        foreach(Collider item in colliders)
        {
            Ground ground = item.GetComponent<Ground>();
            if (ground != null)
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

    public List<GameObject> GetAllUnitsOnNeighborsGroundsFight(List<Ground> grounds)
    {
        List<GameObject> unitsOnGrounds = new List<GameObject>();
        foreach (Ground ground in grounds)
        {
            GameObject unit = CheckGroundUnit(ground);
            if(unit!=null)
            {
                unitsOnGrounds.Add(unit);
            }

        }
        return unitsOnGrounds;

    }

    public GameObject CheckGroundUnit(Ground t_ground)
    {
        //cube of 0.5 x and z and height of a MaxHeight to check if it is walkable
        Vector3 halfExtends = new Vector3(0.25f, 1, 0.25f);
        Collider[] colliders = Physics.OverlapBox(t_ground.transform.position + new Vector3 (0,1,0), halfExtends);
        //once we get all objects collided in the cube, we get the ground type and if it is walkable we add it to the adjacency list
        foreach (Collider item in colliders)
        {
            Ground ground = item.GetComponent<Ground>();
            if (ground != null)
            {
                RaycastHit hit;
                LayerMask layer_mask = LayerMask.GetMask("Unit");

                //check if there is an object upwards of the ground
                if (!Physics.Raycast(ground.transform.position, Vector3.up, out hit, 1, layer_mask))
                {
                    return null;
                }
                //check if the thing blocking the ray is a unit (moveable) or the formation
                else if (hit.transform.gameObject.tag == "Unit")
                {
                    return hit.transform.gameObject;
                }
            }
        }
        return null;

        
    }
    public bool EmptyGround(Ground ground)
    {

        RaycastHit hit;

        //check if there is an object upwards of the ground
        if (Physics.Raycast(ground.transform.position, Vector3.up, out hit, 1))
        {
            //check if the thing blocking the ray is a unit (moveable) or the formation
            if (hit.transform.gameObject.tag == "Regiment" || hit.transform.gameObject.tag == "Obstacle")
            {
                return false;
            }
            else return true;
        }
        else return true;
        
    }

    public List<Ground> SphericFindNeighborsGroundsByRange(int range, float maxHeight)
    {
        List<Ground> grounds = new List<Ground>();

        //iterating to get all grounds in range 
        Vector3 halfExtends = new Vector3(0.25f, (1 + maxHeight / 2), 0.25f);
        Vector3 center = transform.position;
        Collider[] colliders = Physics.OverlapSphere(center, range);
        //once we get all objects collided in the cube, we get the ground type and if it is walkable we add it to the adjacency list
        foreach (Collider item in colliders)
        {
            Ground ground = item.GetComponent<Ground>();
            if (ground != null)
            {
                grounds.Add(ground);
            }
        }
        return grounds;
    }


    public List<Ground> StandardFindNeighborsGroundsByRange(int range, float maxHeight)
    {
        List<Ground> grounds = new List<Ground>();

        //iterating to get all grounds in range 
        Vector3 halfExtends = new Vector3(0.25f, (1 + maxHeight / 2), 0.25f);
        Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3 (range,0,0) , halfExtends);
        //once we get all objects collided in the cube, we get the ground type and if it is walkable we add it to the adjacency list
        foreach (Collider item in colliders)
        {
            Ground ground = item.GetComponent<Ground>();
            if (ground != null)
            {
                grounds.Add(ground);
            }
        }


        Vector3 halfExtends2 = new Vector3(-0.25f, (1 + maxHeight / 2), 0.25f);
        Collider[] colliders2 = Physics.OverlapBox(transform.position + new Vector3(-(range), 0, 0), halfExtends2);
        //once we get all objects collided in the cube, we get the ground type and if it is walkable we add it to the adjacency list
        foreach (Collider item in colliders2)
        {
            Ground ground = item.GetComponent<Ground>();
            if (ground != null)
            {
                grounds.Add(ground);
            }
        }

        Vector3 halfExtends3 = new Vector3(0.25f, (1 + maxHeight / 2), 0.25f);
        Collider[] colliders3 = Physics.OverlapBox(transform.position + new Vector3(0, 0, range), halfExtends3);
        //once we get all objects collided in the cube, we get the ground type and if it is walkable we add it to the adjacency list
        foreach (Collider item in colliders3)
        {
            Ground ground = item.GetComponent<Ground>();
            if (ground != null)
            {
                grounds.Add(ground);
            }
        }



        Vector3 halfExtends4 = new Vector3(0.25f, (1 + maxHeight / 2), -0.25f);
        Collider[] colliders4 = Physics.OverlapBox(transform.position + new Vector3(0, 0, -(range)), halfExtends4);
        //once we get all objects collided in the cube, we get the ground type and if it is walkable we add it to the adjacency list
        foreach (Collider item in colliders4)
        {
            Ground ground = item.GetComponent<Ground>();
            if (ground != null)
            {
                grounds.Add(ground);
            }
        }





        /*foreach(Ground item in grounds)
        {
            Debug.Log(item + "of row" + item.transform.parent);
        }*/

        return grounds;
    }

    public List<Ground> DiagonalFindNeighborsGroundsByRange(int range, float maxHeight)
    {
        List<Ground> grounds = new List<Ground>();

        //iterating to get all grounds in range 
        Vector3 halfExtendsbis = new Vector3(0.25f, (1 + maxHeight / 2), 0.25f);
        Collider[] collidersbis = Physics.OverlapBox(transform.position + new Vector3(range, 0, -(range)), halfExtendsbis);
        //once we get all objects collided in the cube, we get the ground type and if it is walkable we add it to the adjacency list
        foreach (Collider item in collidersbis)
        {
            Ground ground = item.GetComponent<Ground>();
            if (ground != null)
            {
                grounds.Add(ground);
            }
        }


        Vector3 halfExtends2bis = new Vector3(-0.25f, (1 + maxHeight / 2), 0.25f);
        Collider[] colliders2bis = Physics.OverlapBox(transform.position + new Vector3(range , 0, range ), halfExtends2bis);
        //once we get all objects collided in the cube, we get the ground type and if it is walkable we add it to the adjacency list
        foreach (Collider item in colliders2bis)
        {
            Ground ground = item.GetComponent<Ground>();
            if (ground != null)
            {
                grounds.Add(ground);
            }
        }


        Vector3 halfExtends3bis = new Vector3(0.25f, (1 + maxHeight / 2), 0.25f);
        Collider[] colliders3bis = Physics.OverlapBox(transform.position + new Vector3(-(range ), 0, range ), halfExtends3bis);
        //once we get all objects collided in the cube, we get the ground type and if it is walkable we add it to the adjacency list
        foreach (Collider item in colliders3bis)
        {
            Ground ground = item.GetComponent<Ground>();
            if (ground != null)
            {
                grounds.Add(ground);
            }
        }


        Vector3 halfExtends4bis = new Vector3(0.25f, (1 + maxHeight / 2), -0.25f);
        Collider[] colliders4bis = Physics.OverlapBox(transform.position + new Vector3(-(range ), 0, -(range )), halfExtends4bis);
        //once we get all objects collided in the cube, we get the ground type and if it is walkable we add it to the adjacency list
        foreach (Collider item in colliders4bis)
        {
            Ground ground = item.GetComponent<Ground>();
            if (ground != null)
            {
                grounds.Add(ground);
            }
        }

        return grounds;
    }



}
