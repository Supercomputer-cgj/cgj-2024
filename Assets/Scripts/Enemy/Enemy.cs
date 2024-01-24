using System;
using System.Collections;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;

public class Enemy : NetworkBehaviour
{

    public EnemyData data;
    //propriété setteur et getteur


    private float maxHealth;

    //synchronise sur toutes les instances 
    [SyncVar] private float currentHealth;

    private void Awake()
    {
        maxHealth = data.health;
    }

    public void SetDefaults()
    {
        currentHealth = maxHealth;
    }

    // Le serveur vers les clients
    [ClientRpc]
    public void RpcTakeDamage(float weaponDamage)
    {
        currentHealth -= weaponDamage;
        Debug.Log(transform.name + " a pris " + weaponDamage + " Il a maintenat " + currentHealth + "pv");

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        GameManager.UnRegisterEnemny(transform.name);
        gameObject.SetActive(false);
        SetDefaults();
        Debug.Log(transform.name + " est mort");
    }



}