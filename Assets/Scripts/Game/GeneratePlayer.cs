using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject go;
    void StartGame()
    {
        Instantiate(go, transform);
    }

}
