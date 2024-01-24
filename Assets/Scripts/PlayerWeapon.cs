using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class PlayerWeapon : ScriptableObject
{

    public GameObject model;
    
    public string name;
    public float damage;
    public float range;
    public float timeAnimation;
    
    public string descirpiton;
    
}
