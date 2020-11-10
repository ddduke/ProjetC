using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class In_Formation : MonoBehaviour
{

    public GameObject formation;
    public int movementSpeed;

    Vector3 relativeFormationPosition;
    float distanceFromTheFormation;

    // Start is called before the first frame update
    void Start()
    {
        relativeFormationPosition = formation.transform.position - transform.position;
        distanceFromTheFormation = Vector3.Distance(formation.transform.position, transform.position);


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(formation.transform.position , transform.position) != distanceFromTheFormation)
        {
            Vector3 formationPosition = formation.transform.position - relativeFormationPosition;
            transform.position = Vector3.MoveTowards(transform.position, formationPosition, Time.deltaTime * movementSpeed);
        }
        
    }
}
