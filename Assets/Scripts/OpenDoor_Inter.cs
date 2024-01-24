using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor_Inter : MonoBehaviour
{
    private int nbPlayerAc;
    private int nbPlayer = GameManager.instance.nbplayer();
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        nbPlayerAc++;
        if (nbPlayerAc >= nbPlayer)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        nbPlayerAc--;
        if (nbPlayerAc <= 0)
        {
            gameObject.SetActive(true);
        }
    }
}
