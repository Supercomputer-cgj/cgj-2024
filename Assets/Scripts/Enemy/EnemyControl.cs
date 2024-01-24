using Mirror;
 using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyControl : MonoBehaviour
{
    // Stockage du navmesh (pathfinding) et la distance de déplacement automatique
    private NavMeshAgent navAgent;
    private float wanderDistance = 4;

    // Variable contenant le fichier de notre ennemi (contenant les caractèristiques : nom, description, modèle 3D, etc.)
    public EnemyData enemyData;

    private void Start()
    {
        if (navAgent == null)
        {
            navAgent = GetComponent<NavMeshAgent>();
        }
        if (enemyData != null)
        {
            LoadEnemy(enemyData);

        }
    }
    

    private void LoadEnemy(EnemyData _data)
    {
        // Supprime tous les child de l'empty s'il y en a 
        foreach (Transform child in transform)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(child.gameObject);
            }
            else
            {
                Destroy(child.gameObject);
            }
        }

        // Fait apparaitre le modèle 3D de notre ennemi et le configure
        GameObject visuals = Instantiate(enemyData.enemyModel);
        visuals.transform.SetParent(transform);
        visuals.transform.localPosition = Vector3.zero;
        visuals.transform.rotation = Quaternion.identity;

        // Recupère la vitesse dans enemyData et l'attribue au navmesh
        navAgent.speed = enemyData.speed;

    }

    private void Update()
    {
        // Si enemyData n'est pas assigné, on ne fait rien dans Update
        if (enemyData == null)
        {
            return;
        }

        // Si l'ennemi a atteint sa destination, on lui attribue une nouvelle destination
        if (navAgent.remainingDistance < 1f)
        {
            GetNewDestination();
        }
    }

    // Recupère une nouvelle destination a proximité (basé sur la var wanderDistance)
    private void GetNewDestination()
    {
        Vector3 nextDestination = transform.position;
        nextDestination += wanderDistance * new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(nextDestination, out hit, 150f, NavMesh.AllAreas))
        {
            navAgent.SetDestination(hit.position);
        }
    }

}