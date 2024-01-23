using System;
using System.Collections;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar] private bool _isDead = false;

    //propriété setteur et getteur
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField] private float maxHealth = 100f;
    //synchronise sur toutes les instances 
    [SyncVar] private float currentHealth;
        
    
    [SerializeField] private Behaviour[] disableOnDeath;
    private bool[] wasEnabledOnStart;


    public void Setup()
    {
        //Init wasEnable pour SetDefaults()
        wasEnabledOnStart = new bool[disableOnDeath.Length];
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            wasEnabledOnStart[i] = disableOnDeath[i].enabled;
        }
        SetDefaults();
    }

    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;

        //re init les behaviour comme au spawn
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabledOnStart[i];
        }
        
        //Active le collider de this ( SI plusieurs COllider faire une array)
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }
    }

    //respawn du joueur
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.repaswnTimer);
        SetDefaults();
        
        // Replace le joueur a un point de spawn
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }

    // Le serveur vers les clients
    [ClientRpc]
    public void RpcTakeDamage(float weaponDamage)
    {
        if (isDead)
            return;

        currentHealth -= weaponDamage;
        Debug.Log(transform.name + " a pris " + weaponDamage + " Il a maintenat " + currentHealth + "pv");

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        this.GetComponent<Animator>().Play("mort"); 
        transform.position = new Vector3(transform.position.x, transform.position.y-1, transform.position.z);
        
        // désactive les Behaviour (mouvement)
        for (int i = 0; i < disableOnDeath.Length; i++)
            disableOnDeath[i].enabled = false;
        
        //Désactive le collider de this
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
        
        Debug.Log(transform.name +  " est mort");
        
        //Respawn avec délai
        StartCoroutine(Respawn());
    }


    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        //S'infliger des dégats
        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(40f);
        }
    }
}