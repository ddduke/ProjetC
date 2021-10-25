using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRegimentBluePrint : MonoBehaviour
{
    public GameObject BluePrint;

    public void SpawnRegiment()
    {
        Instantiate(BluePrint);
    }
}
