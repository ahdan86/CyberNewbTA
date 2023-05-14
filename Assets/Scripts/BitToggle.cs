using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BitToggle : MonoBehaviour
{
    [SerializeField] private int number;
    public event Action<int, bool> OnToggleChanged = delegate { };

    private void Awake()
    {
        GetComponent<Toggle>().isOn = false;
        GetComponent<Toggle>().onValueChanged.AddListener(HandleToggleChanged);
    }

    private void HandleToggleChanged(bool enabled)
    {
        OnToggleChanged(number, enabled);
    }

    private void OnValidate()
    {
        GetComponentInChildren<Text>().text = number.ToString();
        gameObject.name = "Toggle " + number;
    }
}
