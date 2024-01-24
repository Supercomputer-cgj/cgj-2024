using System;
using System.Collections;
using UnityEngine;
using Mirror;
using Mirror.Examples.AdditiveScenes;
using Unity.VisualScripting;
using UnityEngine.UI;

public class PlayerShoot : NetworkBehaviour
{
    public PlayerWeapon weapon;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask mask;
    [SerializeField] private PlayerSetup setup;
    [SerializeField] private float tempsAffichageCrossHair = 0.5f;
    private Image hitcrossHair;
    private bool hasShot = false;
    private Vector3 direction;
    private Vector3 depart;

    private void Start()
    {
        Image[] image;
        image = setup.getPlayerUiInstance().GetComponentsInChildren<Image>();
        if (image == null)
            Debug.LogError("J'arrete le code");
        for (int i = 0; i < image.Length; i++)
            if (image[i].tag == "HitCrossHair")
            {
                hitcrossHair = image[i];
                break;
            }

        if (hitcrossHair == null)
        {
            Debug.LogError("pas de crossHairHit");
        }
        else
        {
            hitcrossHair.enabled = false;
        }


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
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask))
        {
            direction = cam.transform.forward * weapon.range;
            depart = cam.transform.position;

            //gere uniquement le tag player
            if (hit.collider.tag == "Player")
            {
                StartCoroutine(hitTimer());
                depart = cam.transform.position;
                direction = cam.transform.forward * weapon.range;
                CmdPlayerShot(hit.collider.name);
            }

            if (hit.collider.tag == "Enemy")
            {
                StartCoroutine(hitTimer());
                Debug.Log(this.transform.name + " A touché un enemy :" + hit.collider.name);
                CmdEnemyShot(hit.transform.parent.name);
            }
        }
    }

    private IEnumerator hitTimer()
    {
        hitcrossHair.enabled = true;
        yield return new WaitForSeconds(tempsAffichageCrossHair);
        hitcrossHair.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(depart, direction, Color.red);
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


    [Command]
    private void CmdEnemyShot(string enemyId)
    {
        Debug.Log(enemyId + " a été touché");

        //gestion des dégats
        Enemy enemy = GameManager.getEnemy(enemyId);
        enemy.RpcTakeDamage(weapon.damage);
    }
}