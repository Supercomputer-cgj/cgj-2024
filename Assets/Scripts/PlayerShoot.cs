using System;
using UnityEngine;
using Mirror;
using Mirror.Examples.AdditiveScenes;

public class PlayerShoot : NetworkBehaviour
{
    public PlayerWeapon weapon;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask mask;
    private void Start()
    {
        if (cam == null)
        {
            Debug.LogError("Pas de cam sur PlayerShoot, désactivation du script");
            this.enabled = false;
        }
    }

    //action du tire
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    
    //gestion du tire sur le this
    [Client]
    private void Shoot()
    {
        //récupere le raycast et envoie au serveur son name 
        RaycastHit hit;
        
        if (Physics.Raycast(cam.transform.position, cam.transform.forward,out hit,weapon.range,mask))
        {
            //gere uniquement le tag player
            if (hit.collider.tag == "Player")
            {
                Debug.DrawLine(cam.transform.position, cam.transform.forward * weapon.range, Color.red);
                CmdPlayerShot(hit.collider.name);
            }
        }
    }
    
    //gestion de this envoie au serveur
    [Command]
    private void CmdPlayerShot(string playerId)
    {
        Debug.Log(playerId + " a été touché");
        
        //gestion des dégats
        Player player = GameManager.getPlayer(playerId);
        player.RpcTakeDamage(weapon.damage);
    }
}