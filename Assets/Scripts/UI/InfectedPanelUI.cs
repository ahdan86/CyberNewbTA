using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfectedPanelUI : MonoBehaviour
{
    public static InfectedPanelUI Instance { get; set; }
    public RectTransform backgroundPanel;
    public bool isOpen;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        backgroundPanel.localPosition = new Vector3(0, 280, 0);
    }

    public void AnimateOpenPanel()
    {
        LeanTween.move(backgroundPanel, new Vector3(0, 175, 0), 0.5f).setEase(LeanTweenType.easeInOutExpo);
    }

    public void AnimateClosePanel()
    {
        LeanTween.move(backgroundPanel, new Vector3(0, 280, 0), 0.5f).setEase(LeanTweenType.easeInOutExpo);
    }
}
