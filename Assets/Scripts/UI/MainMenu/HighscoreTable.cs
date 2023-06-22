using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreTable : MonoBehaviour
{
    [SerializeField] private Transform _entryContainer;
    [SerializeField] private Transform _entryTemplate;
    [SerializeField] private string levelName;
    private List<Transform> _highscoreEntryTransformList;

    private void Awake()
    {
        _entryTemplate.gameObject.SetActive(false);
        
        OnLoadHighscore();
    }
    
    public void OnClickLevelButton(string level)
    {
        levelName = level;
        OnLoadHighscore();
    }
    
    private void OnLoadHighscore()
    {
        string jsonString = PlayerPrefs.GetString("highscoreTable_" + levelName);
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        
        if(highscores == null)
        {
            Debug.Log("No Highscore");
            return;
        }
        
        highscores.highscoreEntryList.Sort((x, y) => y.score.CompareTo(x.score));
        foreach (var highscoreEntry in highscores.highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, _entryContainer, new List<Transform>());
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container,
        List<Transform> transformList)
    {
        Transform entryTransform = Instantiate(_entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryTransform.gameObject.SetActive(true);
        
        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;
            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }
        
        entryTransform.Find("PosValue").GetComponent<TMPro.TextMeshProUGUI>().text = rankString;

        string playerName = highscoreEntry.name;
        entryTransform.Find("NameValue").GetComponent<TMPro.TextMeshProUGUI>().text = playerName;
        
        float score = highscoreEntry.score;
        entryTransform.Find("ScoreValue").GetComponent<TMPro.TextMeshProUGUI>().text = score.ToString();
        
        entryTransform.Find("background").gameObject.SetActive(rank % 2 == 1);
        transformList.Add(entryRectTransform);
    }

    public static void AddHighscoreEntry(float score, string name, string levelName)
    {
        HighscoreEntry highscoreEntry = new HighscoreEntry{score = score, name = name};
        
        string jsonString = PlayerPrefs.GetString("highscoreTable_" + levelName);
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        
        highscores.highscoreEntryList.Add(highscoreEntry);
        
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable_" + levelName, json);
        PlayerPrefs.Save();
    }
}

public class Highscores
{
    public List<HighscoreEntry> highscoreEntryList;
}

[System.Serializable]
public class HighscoreEntry
{
    public float score;
    public string name;
}
