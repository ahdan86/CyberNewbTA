using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebsiteChecker : MonoBehaviour
{
    public static WebsiteChecker Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void SetupWebsiteChecker(Website website)
    {
        
    }
}

public enum WebsiteStatusState
{
    NONE,
    LOADING,
    LOADED,
}
