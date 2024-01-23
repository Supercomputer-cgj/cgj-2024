using Mirror;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private Behaviour[] componentsToDisable;
    [SerializeField] private string remoteLayerName = "RemotePlayer";

    [SerializeField] private GameObject playerUiPrefab;
    private GameObject playerUiIntsance;

    [SerializeField] private GameObject playerUiInvPrefab;
    private GameObject playerUiInvInstance;

    private Camera sceneCamera;

    private void Start()
    {
        //désactivation des scripts sur les autres joueurs
        if (!isLocalPlayer)
        {
            DisableCompenents();
            AssignRemoteLayer();
        }
        else
        {
            //gestion de la main cam
            sceneCamera = Camera.main;
            if (sceneCamera != null)
                sceneCamera.gameObject.SetActive(false);
            
            //UiLocal player
            playerUiIntsance = Instantiate(playerUiPrefab);
            playerUiInvInstance = Instantiate(playerUiInvPrefab);
        }
        
        GetComponent<Player>().Setup();
    }
    // pour pas que tout le monde joue le script de chacun
    private void DisableCompenents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
            componentsToDisable[i].enabled = false;
    }
    //pour la gestion du tir avec les layers
    private void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }
    
    
    //register le player au moment de son start
    public override void OnStartClient()
    {
        base.OnStartClient();
        GameManager.RegisterPlayer(GetComponent<NetworkIdentity>().netId.ToString(),GetComponent<Player>());
    }

    private void OnDisable()
    {
        //ractive la camera principal 
        if (sceneCamera != null)
            sceneCamera.gameObject.SetActive(true);
        //desinregistre le joueur
        GameManager.UnRegisterPlayer(transform.name);

        //supprime l'ui
        Destroy(playerUiIntsance);
        Destroy(playerUiInvInstance);
    }
}