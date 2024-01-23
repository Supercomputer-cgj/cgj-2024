
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyData",menuName = "Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyType;
    public GameObject enemyModel;
    public float health = 100f;
    public float speed = 5f;
    public float damage = 5;
}