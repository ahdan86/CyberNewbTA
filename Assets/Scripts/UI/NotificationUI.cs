using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NotificationUI : MonoBehaviour
{
    public static NotificationUI Instance { get; set; }
    public RectTransform backgroundPanel;
    public Text notificationText;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        backgroundPanel.localPosition = new Vector3(600, 0, 0);
    }

    private void Update()
    {
        // This is for testing method 
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            AnimatePanel("Test Objective Updated");
        }
    }

    // Update is called once per frame
    public void AnimatePanel(string text)
    {
        notificationText.text = text;
        StartCoroutine(AnimatePanelCoroutine());
    }

    IEnumerator AnimatePanelCoroutine()
    {
        // animate panel to screen from right to left
        LeanTween.move(backgroundPanel, new Vector3(240, 0, 0), 0.5f).setEase(LeanTweenType.easeInOutExpo);
        yield return new WaitForSeconds(2f);
        LeanTween.move(backgroundPanel, new Vector3(600, 0, 0), 0.5f).setEase(LeanTweenType.easeInOutExpo);
    }
}
