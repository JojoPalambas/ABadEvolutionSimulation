using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SurvivalModeManager : MonoBehaviour
{
    public Grid grid;
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

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Remove()
    {
        instance = null;
        Destroy(gameObject);
    }
}
