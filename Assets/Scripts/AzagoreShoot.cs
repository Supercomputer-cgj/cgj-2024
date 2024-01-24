using System;
using System.Collections;
using System.Net;
using UnityEngine;
using Mirror;
using Mirror.Examples.AdditiveScenes;

public class AzagoreShoot : NetworkBehaviour
{
    public PlayerWeapon weapon;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask mask;
    [SerializeField] private LayerMask wall;
    [SerializeField] GameObject theWall;
    private PlayerController playerController;
    private bool hasShot = false;
    private bool hasMagic = false;
    private Vector3 direction;
    private Vector3 depart;
    
    private Animator animator;
    
    private void Start()
    {
        theWall = Instantiate(theWall);
        if (cam == null)
        {
            Debug.LogError("Pas de cam sur PlayerShoot, désactivation du script");
            this.enabled = false;
        }

        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        
    }

    //action du tire
    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !hasShot)
        {
            Shoot();
            playerController.doSomethingTrue();
        }
        if (Input.GetButtonDown("Fire2") && !hasShot && !hasMagic)
        {
            ShootMagic();
            hasMagic = true;
            playerController.doSomethingTrue();
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
        yield return new WaitForSeconds(weapon.timeAnimation+1); //temps de recharge de l'attaque, evite le spam
        playerController.doSomethingFalse();
    }
    
    
    private void ShootMagic() //on lance les sorts magiques
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward,out hit,wall))
        {
            theWall.transform.position = hit.point;
            theWall.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            Debug.Log("On va poser le wall");
            
            StartCoroutine(magicAttack(theWall)); //on lance le sort
        }
    }
    
    
    private IEnumerator magicAttack(GameObject spell)
    {
        animator.Play("atkMagic"); //on joue lanimation de lancée de sort
        yield return new WaitForSeconds(1); //tps de l'animation
        spell.SetActive(true); //on active le mur
        playerController.doSomethingFalse();
        yield return new WaitForSeconds(20); //tps de recharge de l'attaque, evite le spam 
        hasMagic = false; 
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

    private void OnDisable()
    {
        Destroy(theWall);
    }
}