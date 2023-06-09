using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [FormerlySerializedAs("_progressBarSprite")] [SerializeField] private Image progressBarSprite;
    [FormerlySerializedAs("_progressBarSpeed")] [SerializeField] private float progressBarSpeed = 0.1f;
    [FormerlySerializedAs("_targetFill")] [SerializeField] private float targetFill = 1;
    private bool _isActive;
    private bool _isCompleted;

    // Update is called once per frame
    void Update()
    {
        if(_isActive)
            progressBarSprite.fillAmount = Mathf.MoveTowards(
                progressBarSprite.fillAmount, 
                targetFill, 
                progressBarSpeed * Time.deltaTime
            );
        if (progressBarSprite.fillAmount >= targetFill)
        {
            Debug.Log("Baru Penuh");
            SetProgressActive(false);
        }
    }

    public void SetProgressValue(float value)
    {
        progressBarSprite.fillAmount = value;
    }

    public void SetProgressActive(bool status)
    {
        _isActive = status;
        _isCompleted = !status;
    }
    
    public bool IsProgressCompleted()
    {
        return _isCompleted;
    }
}
