using System;
using System.Collections;
using Mirror;
using Mirror.Examples.AdditiveScenes;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(EnemyShot))]
public class FeuFolletControler : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] public EnemyData data;
    [SerializeField] private Transform spawnPoint;
    private EnemyShot _enemyShot;
    private Enemy _enemy;
    private bool isTrigger;
    private bool isTimer;
    private bool canShot = true;

    

    private void Start()
    {
        _enemyShot = GetComponent<EnemyShot>();
        _enemy = GetComponent<Enemy>();
        _enemy.SetDefaults();
        LoadEnemy();
    }

    private void LoadEnemy()
    {
        GameManager.RegisterEnemny(GetComponent<NetworkIdentity>().netId.ToString(), GetComponent<Enemy>());
        GameObject visuals = Instantiate(data.enemyModel, transform);
        visuals.transform.localPosition = spawnPoint.position;
        agent.speed = data.speed;
    }

    private void Update()
    {
        if (agent.remainingDistance < 1f && !isTrigger)
        {
            GetNewDestination();
        }
    }

    private void OnTriggerStay(Collider col)
    {
        Player player = col.GetComponent<Player>();
        Debug.DrawLine(transform.position, player.transform.position,Color.yellow);
        Vector3 direction = (player.transform.position - transform.position) * 1.1f;
        if (Physics.Raycast(transform.position,direction))
        {
            if (Math.Abs(direction.z) <= data.weaponRange && canShot)
            {
                StartCoroutine(shootTimer(data.attackDelay));
                _enemyShot.CmdEnemyShoot(player,data.weaponDamage);
            }
            agent.SetDestination(player.transform.position);
            isTrigger = true;
            if (!isTimer)
            {
                StartCoroutine(delaiTrigger(data.delaiTrigger));
            }
            
        }

    }

    private IEnumerator shootTimer(float timer)
    {
        canShot = false;
        yield return new WaitForSeconds(timer);
        canShot = true;
    }


    private IEnumerator delaiTrigger(float timer)
    {
        isTimer = true;
        yield return new WaitForSeconds(timer);
        isTrigger = false;
        isTimer = false;
    }

    private void GetNewDestination()
    {
        Vector3 nextDestination = transform.position;
        nextDestination += data.wanderDistance *
                           new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        Debug.DrawLine(transform.position, nextDestination, Color.blue);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(nextDestination, out hit, 3f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }


}