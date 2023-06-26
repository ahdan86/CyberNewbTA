using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour
{
    [FormerlySerializedAs("HasFDContamined")] public bool hasFDContamined;
    [FormerlySerializedAs("HasFDAntivirus")] public bool hasFDAntivirus;
    [FormerlySerializedAs("HasFDContaminedCleaned")] public bool hasFDContaminedCleaned;

    public void SetHasContamined(bool status)
    {
        hasFDContamined = status;
        if(status)
            NotificationUI.Instance.AnimatePanel("Flashdrive added to inventory");
        else 
            NotificationUI.Instance.AnimatePanel("Item removed from inventory");
    }
    
    public void SetHasAntivirus(bool status)
    {
        hasFDAntivirus = status;
        if(status)
            NotificationUI.Instance.AnimatePanel("Flashdrive added to inventory");
        else 
            NotificationUI.Instance.AnimatePanel("Item removed from inventory");
    }
}
 