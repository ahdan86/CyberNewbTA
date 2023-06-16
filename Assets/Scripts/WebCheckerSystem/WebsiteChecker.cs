using System.Collections;
using System.Collections.Generic;
using System.Linq;
using float_oat.Desktop90;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum WebcheckerState
{
    NONE,
    LOADING,
    LOADED,
}
public class WebsiteChecker : MonoBehaviour
{
    [Header("Windows Properties")]
    [SerializeField] private WindowController wrongWindow;
    [SerializeField] private WindowController correctWindow;

    [SerializeField] private GameObject mainWebChecker;
    [SerializeField] private Image webImage;
    [SerializeField] private InputField webBrand;
    [SerializeField] private InputField webUrl;
    [SerializeField] private Toggle toggleOn;
    [SerializeField] private GameObject loadingWebChecker;
    [SerializeField] private GameObject noneWebChecker;
    [SerializeField] private GameObject browser;

    [Header("Active Website")]
    [SerializeField] private List<Website> websitesList;
    private LinkedList<Website> _websitesListClone;
    private LinkedListNode<Website> _currentWebsiteNode;
    private Website _currentWebsite;

    private void OnEnable()
    {
        _websitesListClone = new LinkedList<Website>(websitesList);
        if (_websitesListClone.Count > 0)
        {
            RandomizeOrder(_websitesListClone);
            _currentWebsiteNode = _websitesListClone.First;
            _currentWebsite = _currentWebsiteNode.Value;
        }

        StartCoroutine(ChangeScreen());
    }

    IEnumerator ChangeScreen()
    {
        SetScreen(WebcheckerState.LOADING);
        yield return new WaitForSeconds(0.75f);
        SetScreen(_websitesListClone.Count > 0 ? WebcheckerState.LOADED : WebcheckerState.NONE);
    }

    private void SetScreen(WebcheckerState state)
    {
        mainWebChecker.SetActive(state == WebcheckerState.LOADED);
        loadingWebChecker.SetActive(state == WebcheckerState.LOADING);
        noneWebChecker.SetActive(state == WebcheckerState.NONE);

        if (state == WebcheckerState.LOADED)
        {
            LoadWebChecker();
        }
    }

    private void LoadWebChecker()
    {
        webImage.sprite = _currentWebsite.webSprite;
        webBrand.text = _currentWebsite.brand;
        webUrl.text = _currentWebsite.url;
    }

    public void Submit()
    {
        var toggleStatus = toggleOn.isOn;
        bool isAnswerCorrect = toggleStatus == _currentWebsite.isPhising;
        
        if(!isAnswerCorrect)
            LevelController.Instance.ReduceMoney(50f);
        else
        {
            var temp = _currentWebsiteNode;
            _websitesListClone.Remove(temp);
        }
        StartCoroutine(SubmitAnswer(isAnswerCorrect ? correctWindow : wrongWindow));
        
        if (_websitesListClone.Count > 0)
        {
            _currentWebsiteNode = _currentWebsiteNode.Next ?? _websitesListClone.First;
            _currentWebsite = _currentWebsiteNode.Value;
        }
        else
        {
            _currentWebsiteNode = null;
            _currentWebsite = null;
        }
    }
    
    IEnumerator SubmitAnswer(WindowController selectedWindow)
    {
        selectedWindow.Open();
        yield return new WaitForSeconds(0.75f);
        selectedWindow.Close();

        StartCoroutine(ChangeScreen());
    }

    public void OpenBrowser()
    {
        string link = webUrl.text;
        browser.GetComponent<WindowController>().Open();
        // browser.GetComponent<WebBrowser>().LoadMySite(link);
    }

    static void RandomizeOrder<T>(LinkedList<T> linkedList)
    {
        System.Random random = new System.Random();
        T[] array = linkedList.ToArray();

        // Fisher-Yates shuffle algorithm
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }

        linkedList.Clear();
        foreach (var item in array)
        {
            linkedList.AddLast(item);
        }
    }
}
