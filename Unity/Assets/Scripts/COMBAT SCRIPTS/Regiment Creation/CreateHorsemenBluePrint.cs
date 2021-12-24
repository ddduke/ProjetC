using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateHorsemenBluePrint : MonoBehaviour
{
    public GameObject BluePrint;

    public void SpawnHorsemen()
    {
        Instantiate(BluePrint);
    }
}
