using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnMob : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private BoxCollider col;
    [SerializeField] private Transform[] spawnPoint;
    private int tourMax = 3;
    private int nbPlayer = GameManager.instance.nbplayer();
    private int nbTour;
    void Start()
    {
        door.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
        if (!col.isTrigger)
        {
            if (nbTour < tourMax)
            {
                nbTour++;
                for (int i = 0; i < (nbPlayer * 4); i++)
                {
                    int choix = (int)Random.Range(0f, 2f);
                    Transform current = spawnPoint[choix];
                    current.position = new Vector3(Random.Range(spawnPoint[choix].position.x - 5f, spawnPoint[choix].position.x + 5f),
                        spawnPoint[choix].position.y,
                        Random.Range(spawnPoint[choix].position.z - 5f, spawnPoint[choix].position.z + 5f));
                    StartCoroutine(delaispawn());
                    //Instantiate(prefab, curent)
                }
            }
            else
            {
                door.SetActive(false);
            }
            
        }
    }

    private IEnumerator delaispawn()
    {
       yield return new WaitForSeconds(5f);
    }
}
