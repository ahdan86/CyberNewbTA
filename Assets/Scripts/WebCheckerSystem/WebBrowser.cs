using UnityEngine;
using VoltstroStudios.UnityWebBrowser.Core;
public class WebBrowser : MonoBehaviour
{
    [SerializeField] private BaseUwbClientManager clientManager;
        
    private WebBrowserClient webBrowserClient;

    private void Start()
    {
        webBrowserClient = clientManager.browserClient;
    }

    //Call this from were ever, and it will load 'https://voltstro.dev'
    public void LoadMySite(string website = "https://google.com")
    {
        webBrowserClient.LoadUrl(website);
    }
}
