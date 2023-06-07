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

    public void SetHasContamined(bool status)
    {
        HasFDContamined = status;
        if(status)
            NotificationUI.Instance.AnimatePanel("Flashdrive added to inventory");
        else 
            NotificationUI.Instance.AnimatePanel("Item removed from inventory");
    }
    
    public void SetHasAntivirus(bool status)
    {
        HasFDAntivirus = status;
        if(status)
            NotificationUI.Instance.AnimatePanel("Flashdrive added to inventory");
        else 
            NotificationUI.Instance.AnimatePanel("Item removed from inventory");
    }
}
 