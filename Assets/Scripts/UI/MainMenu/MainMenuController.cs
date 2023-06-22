using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject[] rightMenuPanel;
    private void Awake()
    {
        var mainMenuTransform = mainMenuPanel.GetComponent<RectTransform>();
        mainMenuTransform.localPosition = new Vector3(-600, 0, 0);
    }

    private void Start()
    {
        var mainMenuTransform = mainMenuPanel.GetComponent<RectTransform>();
        LeanTween.move(mainMenuTransform, new Vector3(-234,0, 0), 1f)
            .setEase(LeanTweenType.easeInOutExpo);
        
        for (int i = 0; i < rightMenuPanel.Length; i++)
        {
            var panelTransform = rightMenuPanel[i].GetComponent<RectTransform>();
            panelTransform.localScale = Vector3.zero;
            
            var hideButton = rightMenuPanel[i].transform.Find("Hide Button").GetComponent<Button>();
            var indexPanel = i;
            hideButton.onClick.AddListener(() => CloseMenuPanel(indexPanel));
            rightMenuPanel[i].SetActive(false);
        }
    }
    
    public void ShowMenuPanel(int menuIndex)
    {
        StartCoroutine(ShowMenuPanelAnimation(menuIndex));
    }
    
    IEnumerator ShowMenuPanelAnimation(int menuIndex)
    {
        // if there is another panel active, close it first
        for (int i = 0; i < rightMenuPanel.Length; i++)
        {
            if (rightMenuPanel[i].activeSelf)
            {
                CloseMenuPanel(i);
                yield return new WaitForSeconds(0.5f);
            }
        }

        rightMenuPanel[menuIndex].SetActive(true);
        RectTransform panelTransform = rightMenuPanel[menuIndex].GetComponent<RectTransform>();
        panelTransform.LeanScale(Vector3.one, 0.5f).setEaseInOutExpo();
    }

    public void CloseMenuPanel(int menuIndex)
    {
        StartCoroutine(CloseMenuPanelAnimation(menuIndex));
    }
    
    IEnumerator CloseMenuPanelAnimation(int menuIndex)
    {
        RectTransform panelTransform = rightMenuPanel[menuIndex].GetComponent<RectTransform>();
        panelTransform.LeanScale(Vector3.zero, 0.5f).setEaseInOutExpo();
        yield return new WaitForSeconds(0.5f);
        rightMenuPanel[menuIndex].SetActive(false);
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
    
}
