using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image _progressBarSprite;
    [SerializeField] private float _progressBarSpeed = 0.1f;
    [SerializeField] private float _targetFill = 1;
    private bool isActive = false;
    private bool isCompleted = false;

    // Update is called once per frame
    void Update()
    {
        if(isActive)
            _progressBarSprite.fillAmount = Mathf.MoveTowards(_progressBarSprite.fillAmount, _targetFill, _progressBarSpeed * Time.deltaTime);
        if (_progressBarSprite.fillAmount == 1)
        {
            isCompleted = true;
            isActive = false;
        }
    }

    public void SetProgressValue(float value)
    {
        _targetFill = value;
    }

    public void SetProgressActive(bool status)
    {
        isActive = status;
    }
    
    public bool isProgressCompleted()
    {
        return isCompleted;
    }
}
