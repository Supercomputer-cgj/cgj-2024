using System;
using System.Collections;
using System.Net;
using UnityEngine;
using Mirror;
using Mirror.Examples.AdditiveScenes;

public class PlayerShoot : NetworkBehaviour
{
    public PlayerWeapon weapon;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask mask;
    private bool hasShot = false;
    private Vector3 direction;
    private Vector3 depart;
    
    private Animator animator;
    
    private void Start()
    {
        if (cam == null)
        {
            Debug.LogError("Pas de cam sur PlayerShoot, désactivation du script");
            this.enabled = false;
        }

        animator = GetComponent<Animator>();
    }

    //action du tire
    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !hasShot)
        {
            Shoot();
        }
        if (Input.GetButtonDown("Fire2") && !hasShot)
        {
            ShootMagic();
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
            hasShot = true;
            StartCoroutine(CacAttack());
            //gere uniquement le tag player
            if (hit.collider.tag == "Player")
            {
                depart = cam.transform.position;
                direction = cam.transform.forward * weapon.range;
                CmdPlayerShot(hit.collider.name);
            }
        }
    }
    
    private IEnumerator CacAttack()
    {
        animator.Play("atkCac"); //on joue lanimation d'attack
        yield return new WaitForSeconds(weapon.timeAnimation); //on joue son temps d'attack
        hasShot = false;
        yield return new WaitForSeconds(weapon.timeAnimation+2); //temps de recharge de l'attaque, evite le spam
    }
    
    
    private void ShootMagic() //on lance les sorts magiques
    {
        //récupere le raycast et envoie au serveur son name 
        GameObject spell = GameObject.FindWithTag("SpellMagic");

        spell.SetActive(true);
        StartCoroutine(magicAttack(spell)); //on lance le sort


        /*RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward,out hit,weapon.range,mask))
        {
            hasShot = true;
            StartCoroutine(CacAttack());
            //gere uniquement le tag player
            if (hit.collider.tag == "Player")
            {
                depart = cam.transform.position;
                direction = cam.transform.forward * weapon.range;
                CmdPlayerShot(hit.collider.name);
            }
        }*/
    }
    
    
    private IEnumerator magicAttack(GameObject spell)
    {
        animator.Play("atkMagic"); //on joue lanimation de lancée de sort
        yield return new WaitForSeconds(10); //tps de "l'atk'
        spell.SetActive(true);
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(depart, direction);
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