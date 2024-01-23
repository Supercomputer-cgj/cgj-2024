using System;
using Mirror;
using UnityEngine;

[RequireComponent(typeof(FeuFolletControler))]
public class EnemyShot : NetworkBehaviour
{
    [Command(requiresAuthority = false)]
    public void CmdEnemyShoot(Player player, float weaponDamage)
    {
        Debug.Log(player.transform.name + " a été touché par :" + gameObject.transform.name);
        player.RpcTakeDamage(weaponDamage);
    }
}