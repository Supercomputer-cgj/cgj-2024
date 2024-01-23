
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyData",menuName = "Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyType;
    public GameObject enemyModel;
    public float health = 100f;
    public float speed = 5f;
    public float wanderDistance = 10;
    public float weaponRange  = 0.5f;
    public float weaponDamage = 0.2f;
    public float attackDelay = 1f;
    public float delaiTrigger = 5f;
}   