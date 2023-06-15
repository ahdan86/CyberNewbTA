using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Website", menuName = "ScriptableObjects/Website", order = 1)]
public class Website : ScriptableObject
{
    public string url;
    public string brand;
    public Sprite webSprite;
    public bool isPhising;
}
