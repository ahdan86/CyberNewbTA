using System.Collections;
using System.Collections.Generic;
using System.Linq;
using float_oat.Desktop90;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum WebcheckerState
{
    None,
    Loading,
    Loaded,
}
public class WebsiteChecker : MonoBehaviour
{
    [Header("Windows Properties")]
    [SerializeField] private WindowController wrongWindow;
    [SerializeField] private WindowController correctWindow;
    [SerializeField] private WindowController timesUpWindow;

    [SerializeField] private GameObject mainWebChecker;
    [SerializeField] private Image webImage;
    [SerializeField] private InputField webBrand;
    [SerializeField] private InputField webUrl;
    [SerializeField] private Toggle toggleOn;
    [SerializeField] private Text counterText;
    [SerializeField] private Button submitButton;
    private Coroutine _counterCoroutine;
    
    [SerializeField] private GameObject loadingWebChecker;
    [SerializeField] private GameObject noneWebChecker;
    [SerializeField] private GameObject browser;
    

    [FormerlySerializedAs("websitesList")]
    [Header("Active Website")]
    [SerializeField] private List<Website> allWebsitesList;
    [SerializeField] private int phishWebCount;
    [SerializeField] private int nonPhishWebCount;
    [SerializeField] private int timeLimit;
    
    private LinkedList<Website> _websitesLinkedList;
    private LinkedListNode<Website> _currentWebsiteNode;
    private Website _currentWebsite;

    private void OnEnable()
    {
        var splitWeb = 
            allWebsitesList
                .GroupBy(web => web.isPhising)
                .ToDictionary(x => x.Key, z => z.ToArray());
        var phishingWebsites = splitWeb[true];
        RandomizeArrayOrder(phishingWebsites);
        var nonPhishingWebsites = splitWeb[false];
        RandomizeArrayOrder(nonPhishingWebsites);
        
        _websitesLinkedList = new LinkedList<Website>();
        for(int i = 0; i < phishWebCount; i++)
            _websitesLinkedList.AddLast(phishingWebsites[i]);
        for(int i = 0; i < nonPhishWebCount; i++)
            _websitesLinkedList.AddLast(nonPhishingWebsites[i]);
        
        if (_websitesLinkedList.Count > 0)
        {
            RandomizeListOrder(_websitesLinkedList);
            _currentWebsiteNode = _websitesLinkedList.First;
            _currentWebsite = _currentWebsiteNode.Value;
        }

        StartCoroutine(ChangeScreen());
    }

    IEnumerator ChangeScreen()
    {
        SetScreen(WebcheckerState.Loading);
        yield return new WaitForSeconds(0.75f);
        SetScreen(_websitesLinkedList.Count > 0 ? WebcheckerState.Loaded : WebcheckerState.None);
    }

    private void SetScreen(WebcheckerState state)
    {
        mainWebChecker.SetActive(state == WebcheckerState.Loaded);
        loadingWebChecker.SetActive(state == WebcheckerState.Loading);
        noneWebChecker.SetActive(state == WebcheckerState.None);

        if (state == WebcheckerState.Loaded)
        {
            LoadWebChecker();
        }
    }

    private void LoadWebChecker()
    {
        webImage.sprite = _currentWebsite.webSprite;
        webBrand.text = _currentWebsite.brand;
        webUrl.text = _currentWebsite.url;
        
        toggleOn.isOn = false;
        submitButton.interactable = true;
        toggleOn.interactable = true;
        
        _counterCoroutine = StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        int counter = timeLimit;
        counterText.text = counter.ToString();
        while (counter >= 0)
        {
            yield return new WaitForSeconds(1f);
            counter--;
            counterText.text = counter.ToString();
        }
        LevelController.Instance.ReduceMoney(25f);

        StartCoroutine(PopUpAnswer(timesUpWindow));
        ChangeWebsite();
    }

    public void Submit()
    {
        StopCoroutine(_counterCoroutine);
        submitButton.interactable = false;
        toggleOn.interactable = false;
        var toggleStatus = toggleOn.isOn;
        bool isAnswerCorrect = toggleStatus == _currentWebsite.isPhising;

        if (!isAnswerCorrect)
        {
            LevelController.Instance.ReduceMoney(50f);
            StartCoroutine(PopUpAnswer(wrongWindow));
            ChangeWebsite();
        }
        else
        {
            QuestEvent.current.Solve((int)ObjectiveType.SOLVE_PHISHING);
            var temp = _currentWebsiteNode;
            StartCoroutine(PopUpAnswer(correctWindow));
            ChangeWebsite();
            _websitesLinkedList.Remove(temp);
        }
    }
    
    private void ChangeWebsite()
    {
        if (_websitesLinkedList.Count > 0)
        {
            _currentWebsiteNode = _currentWebsiteNode.Next ?? _websitesLinkedList.First;
            _currentWebsite = _currentWebsiteNode.Value;
        }
        else
        {
            _currentWebsiteNode = null;
            _currentWebsite = null;
        }
    }
    
    IEnumerator PopUpAnswer(WindowController selectedWindow)
    {
        selectedWindow.Open();
        yield return new WaitForSeconds(0.75f);
        selectedWindow.Close();

        StartCoroutine(ChangeScreen());
    }

    public void OpenBrowser()
    {
        browser.GetComponent<WindowController>().Open();
        // browser.GetComponent<WebBrowser>().LoadMySite(link);
    }

    static void RandomizeListOrder<T>(LinkedList<T> linkedList)
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
    
    static void RandomizeArrayOrder<T>(T[] array)
    {
        System.Random random = new System.Random();

        // Fisher-Yates shuffle algorithm
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}
