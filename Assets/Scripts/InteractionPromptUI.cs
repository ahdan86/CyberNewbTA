using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    private Camera _mainCam;
    [SerializeField] private GameObject _uiPanel; 

    private void Start()
    {
        _mainCam = Camera.main;
    }

    private void LateUpdate()
    {
        var rotation = _mainCam.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
    }

    public bool isDisplayed = false;

    public void SetUp()
    {
        _uiPanel.SetActive(true);
    }

    public void Close()
    {
        _uiPanel.SetActive(false);
    }
}
