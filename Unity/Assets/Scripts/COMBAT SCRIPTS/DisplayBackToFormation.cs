using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayBackToFormation : MonoBehaviour
{
    public string side;
    public GameObject regimentSlot;
    public bool inputMouse = false;
    public Camera cam;
    public Vector3 target;
    public Vector3 newTarget;
    public Vector3 playerFormationPivot;
    public Vector3 enemyFormationPivot;
    public List<regimentFormationPosition> playerRegimentFormationPositions;
    public List<regimentFormationPosition> enemyRegimentFormationPositions;
    // Start is called before the first frame update
    void Start()
    {
        side = GetComponent<TurnManager>().turn;
        playerRegimentFormationPositions = new List<regimentFormationPosition>();
        enemyRegimentFormationPositions = new List<regimentFormationPosition>();
        UpdateFormationInfo("enemy");
        UpdateFormationInfo("player");
        GetComponent<DisplayBackToFormation>().enabled = false;
    }

    public void LaunchScript()
    {
        GetComponent<DisplayBackToFormation>().enabled = true;
    }

    /// <summary>
    /// Stop the display of the formation slots and the path that regiments will follow
    /// </summary>
    public void StopScript()
    {

        GameObject[] existingSlots = GameObject.FindGameObjectsWithTag("RegimentSlot");
        foreach (GameObject slot in existingSlots) GameObject.Destroy(slot);
        GameObject[] existingPathLines = GameObject.FindGameObjectsWithTag("PathLine");
        foreach (GameObject pathLine in existingPathLines) GameObject.Destroy(pathLine); 
        GetComponent<DisplayBackToFormation>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            inputMouse = true;
        }
        //check if the target has changed, if it has delete all gameObjects formationslots and recreate ones
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.tag == "Ground")
        {
            Ground g = hit.collider.GetComponent<Ground>();
            newTarget = g.transform.position;
            newTarget.y += 0.5f;
            /*
             * Make A correct Target Z & X but with the target formation not the actual one (in charge)
            newTarget.z = GetComponent<UsefulCombatFunctions>().CorrectTargetZ(newTarget.z);
            newTarget.x = GetComponent<UsefulCombatFunctions>().CorrectTargetX(newTarget.x);
            */
        }
        if (newTarget != target && !inputMouse)
        {
            target = newTarget;
            /*
             * Make A correct Target Z & X but with the target formation not the actual one (in charge)
            newTarget.z = GetComponent<UsefulCombatFunctions>().CorrectTargetZ(newTarget.z);
            newTarget.x = GetComponent<UsefulCombatFunctions>().CorrectTargetX(newTarget.x);
            */
            GameObject[] existingSlots = GameObject.FindGameObjectsWithTag("RegimentSlot");
            foreach (GameObject slot in existingSlots) GameObject.Destroy(slot);
            DisplayFormation(side, target);
            //make a charge-like function, with priority, with possible grounds, selected cases, etc... with another function as it is not the same possible grounds that we try to reach!
        }

        if (inputMouse)
        {
           //freeze path and combatslots then store it into each unit as for the charge function
        }

    }

    private void DisplayFormation(string side, Vector3 target)
    {
        List<Vector3> regimentsRelativePosition = new List<Vector3>();
        
        foreach (regimentFormationPosition elmt in playerRegimentFormationPositions)
        {
            regimentsRelativePosition.Add(elmt.relativePosition);
        }
        //Displays the regimentslots relatively to the target
        foreach (Vector3 regimentRelativePosition in regimentsRelativePosition)
        {
            Vector3 position = target - regimentRelativePosition;
            Instantiate(regimentSlot, position, Quaternion.identity);
        }
    }


    public void UpdateFormationInfo(string side)
    {
        if (side == "player")
        {
            playerRegimentFormationPositions.Clear();
            playerFormationPivot = GetComponent<UsefulCombatFunctions>().FormationPivot(side);
            List<GameObject> regimentsList = new List<GameObject>();
            regimentsList = GetComponent<TurnManager>().GetAllUnitsBySide(side);
            foreach (GameObject regiment in regimentsList)
            {
                Vector3 relativePosition = playerFormationPivot - regiment.transform.position;
                playerRegimentFormationPositions.Add(new regimentFormationPosition(regiment, relativePosition));
            }
        }

        if (side == "enemy")
        {
            enemyRegimentFormationPositions.Clear();
            enemyFormationPivot = GetComponent<UsefulCombatFunctions>().FormationPivot(side);
            List<GameObject> regimentsList = new List<GameObject>();
            regimentsList = GetComponent<TurnManager>().GetAllUnitsBySide(side);
            foreach (GameObject regiment in regimentsList)
            {
                Vector3 relativePosition = playerFormationPivot - regiment.transform.position;
                enemyRegimentFormationPositions.Add(new regimentFormationPosition(regiment, relativePosition));
            }
        }
    }

    
}
public class regimentFormationPosition
{

    public GameObject regiment;
    public Vector3 relativePosition;
    public regimentFormationPosition(GameObject oregiment, Vector3 orelativePosition)
    {
        regiment = oregiment;
        relativePosition = orelativePosition;
    }
}
