using UnityEngine;
public class MainMenu: MonoBehaviour

{
    [SerializeField] private NetworkManagerLobby networkManager = null;
    [Header("UI")]
    [SerializeField] private GameObject landingPagePanel = null;
    public void HostLobby()
    {
        networkManager.StartHost();
        landingPagePanel.SetActive(false);
    }
    /*
    public void JoinLobby()
    {
        networkManager.StartClient();
        landingPagePanel.SetActive(false);
    }
    */
    
    
}