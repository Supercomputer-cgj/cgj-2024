using Mirror;
using UnityEngine;

[RequireComponent(typeof(EnemyData))]
public class Enemy : NetworkBehaviour
{
    private float currentHealth;
    private float maxHealth;

    public EnemyData data;
    
    private void Start()
    {
        transform.name = data.enemyType;
        maxHealth = data.health;
        currentHealth = maxHealth;
    }


    
    [ClientRpc]
    public void RpcTakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(transform.name + " a pris " + damage + " damage il a maintentant " + currentHealth);
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log(transform.name + " est mort destruction");
        Destroy(this);
    }
}