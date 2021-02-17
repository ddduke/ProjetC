using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DisplaysOnCombatMap : MonoBehaviour
{
    public GameObject RegimentSlot;
    public Vector3 target;
    public Vector3 newTarget;
    public Camera cam;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        //get the ground selected by mouse pointer
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.tag == "Ground")
        {
            Ground g = hit.collider.GetComponent<Ground>();
            target = g.transform.position;
            target.y += 0.5f;
        }

        DisplayFormation("player", target);
    }

    private void Update()
    {
        //check if the target has changed, if it has delete all gameObjects formationslots and recreate ones
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.tag == "Ground")
        {
            Ground g = hit.collider.GetComponent<Ground>();
            newTarget = g.transform.position;
            newTarget.y += 0.5f;
        }
        if (newTarget!=target)
        {
            target = newTarget;
            GameObject[] existingSlots = GameObject.FindGameObjectsWithTag("RegimentSlot");
            foreach (GameObject slot in existingSlots) GameObject.Destroy(slot);
            DisplayFormation("player", target);
        }
    }

    /// <summary>
    /// Get to display the formation slots where the regiments can move relatively to the target
    /// </summary>
    public void DisplayFormation(string side, Vector3 target)
    {

        
        //get all regiments by side 
        List<GameObject> regimentsList = new List<GameObject>();
        regimentsList = GetComponent<TurnManager>().GetAllUnitsBySide(side);
        Debug.Log("test" + regimentsList);

        //Get the formation pivot (center), represented by the max X (right of the formation) and average z (vertical center of the formation)
        //first get the max z
        float maxX = -100;
        foreach(GameObject regiment in regimentsList)
        {
            if (regiment.transform.position.x > maxX) maxX = regiment.transform.position.x;
        }
        //then get the average z and set round it to put it on the grid
        List<float> regimentsZ = new List<float>();
        foreach (GameObject regiment in regimentsList)
        {
            regimentsZ.Add(regiment.transform.position.z);
        }
        float averageZ = regimentsZ.Average();

        //set the relative position to formation pivot to all regiments
        Vector3 formationPivot = new Vector3(Mathf.Round(maxX), 1, Mathf.Round(averageZ));
        List<Vector3> regimentsRelativePosition = new List<Vector3>();
        foreach (GameObject regiment in regimentsList)
        {
            Vector3 relativePosition = formationPivot - regiment.transform.position;
            regimentsRelativePosition.Add(relativePosition);
        }
        //Displays the regimentslots relatively to the target
        foreach (Vector3 regimentRelativePosition in regimentsRelativePosition)
        {
            Vector3 position = target - regimentRelativePosition;
            Instantiate(RegimentSlot, position, Quaternion.identity);
        }
    }
}
