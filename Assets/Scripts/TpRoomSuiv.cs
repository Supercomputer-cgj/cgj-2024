using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpRoomSuiv : MonoBehaviour
{
    [SerializeField] private Transform[] spawnRoom;
    private int nbPlayer;
    private void OnTriggerEnter(Collider player)
    {
        if (nbPlayer > 3)
            nbPlayer = 0;
        player.transform.position = spawnRoom[nbPlayer].position;
    }
}
