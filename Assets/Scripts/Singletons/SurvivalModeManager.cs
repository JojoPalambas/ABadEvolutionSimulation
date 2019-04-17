using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SurvivalModeManager : MonoBehaviour
{
    public Tilemap tilemap; 

    public static SurvivalModeManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        Debug.Log("Survival mode started");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Cleanup()
    {
    }

    public void Remove()
    {
        Cleanup();
        instance = null;
        Destroy(gameObject);
    }
}
