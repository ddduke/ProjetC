using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] regs = GameObject.FindGameObjectsWithTag("Regiment");
        foreach(GameObject reg in regs)
        {
            float regx = Mathf.Round(reg.transform.position.x);
            float regz = Mathf.Round(reg.transform.position.z);
            reg.transform.position = new Vector3(regx, reg.transform.position.y, regz);
        }
    }

}
