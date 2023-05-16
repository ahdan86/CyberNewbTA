using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool HasFDContamined = false;
    public bool HasFDAntivirus = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HasFDContamined = !HasFDContamined;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HasFDAntivirus = !HasFDAntivirus;
        }
    }
}
 