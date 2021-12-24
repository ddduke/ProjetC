using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateArchersBluePrint : MonoBehaviour
{
    public GameObject BluePrint;

    public void SpawnArchers()
    {
        Instantiate(BluePrint);
    }
}
