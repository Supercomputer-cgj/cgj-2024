using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NetworkManager = Mirror.NetworkManager;
public class LobbyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    
    private void Start(){
       
            // Lobby 
            Button playButton = GameObject.Find("PlayButton").GetComponent<Button>();
            playButton.onClick.AddListener(OnClickPlayButton);
            
            Button cleaButton = GameObject.Find("CleaButton").GetComponent<Button>();
            cleaButton.onClick.AddListener(OnClickCleaButton);
            
            Button maleandroButton = GameObject.Find("MaleandroButton").GetComponent<Button>();
            maleandroButton.onClick.AddListener(OnClickMaelandroButton);
            
            Button azagoreButton = GameObject.Find("AzagoreButton").GetComponent<Button>();
            azagoreButton.onClick.AddListener(OnClickAzagoreButton);
        }
    // Start is called before the first frame update
        
 void OnClickPlayButton()
    {
        gameObject.SetActive(false);
    }
 

    void OnClickCleaButton()
    { 
        NetworkManager.singleton.playerPrefab = prefabs[0];
    }
    void OnClickMaelandroButton()
    {
        NetworkManager.singleton.playerPrefab = prefabs[1];
    }
    void OnClickAzagoreButton()
    {
        NetworkManager.singleton.playerPrefab = prefabs[2];
    }
    
}
