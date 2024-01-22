using Mirror;
using UnityEngine;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] private Behaviour[] componentsToDisable;
    [SerializeField] private string remoteLayerName = "RemotePlayer";

    private Camera sceneCamera;

    private void Start()
    {
        //désactivation des scripts sur les autres joueurs
        if (!isLocalPlayer)
        {
            DisableCompenents();
            AssignRemoteLayer();
        }
        //gestion de la main cam
        else
        {
            sceneCamera = Camera.main;
            if (sceneCamera != null)
                sceneCamera.gameObject.SetActive(false);
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

    //ractive la camera principal et desinregistre le joueur
    private void OnDisable()
    {
        if (sceneCamera != null)
            sceneCamera.gameObject.SetActive(true);
        GameManager.UnRegisterPlayer(transform.name);
    }
}