using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint_script : MonoBehaviour
{

    RaycastHit hit;
    Vector3 movePoint;
    public GameObject prefab;
    public GameObject CombatScripts;

    // Start is called before the first frame update
    void Start()
    {
        CombatScripts = GameObject.Find("CombatScripts").transform.gameObject;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.tag == "Ground")
        {
            Ground g = hit.collider.GetComponent<Ground>();
            movePoint = g.transform.position;
            movePoint.y += 0.5f;
            movePoint.z = CombatScripts.GetComponent<UsefulCombatFunctions>().CorrectTargetZ(movePoint.z);
            movePoint.x = CombatScripts.GetComponent<UsefulCombatFunctions>().CorrectTargetX(movePoint.x);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CombatScripts = GameObject.Find("CombatScripts").transform.gameObject;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.collider.tag == "Ground")
        {
            Ground g = hit.collider.GetComponent<Ground>();
            movePoint = g.transform.position;
            movePoint.y += 1f;
            movePoint.z = Mathf.Round(movePoint.z);
            movePoint.x = Mathf.Round(movePoint.x);
            transform.position = movePoint;
        }

        if(Input.GetMouseButton(0))
        {
            GameObject go = Instantiate(prefab, movePoint, transform.rotation);
            if (CombatScripts.GetComponent<TurnManager>().turn == "enemy")
            {
                go.GetComponent<CombatVariables>().enemy = true;
            }
            CombatScripts.GetComponent<DisplayBackToFormation>().UpdateFormationInfo(CombatScripts.GetComponent<TurnManager>().turn);
            Destroy(gameObject);
        }
    }
}
