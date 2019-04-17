using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SurvivalModeManager : MonoBehaviour
{
    public Tilemap tilemap;

    public GameObject mousePrefab;
    private List<Mouse> mice;

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

        mice = new List<Mouse>();

        for (int i = 0; i < SurvivalModeConstants.miceNumber; i++)
        {
            mice.Add(Instantiate(mousePrefab, SurvivalModeConstants.miceStartingPosition, new Quaternion()).GetComponent<Mouse>());
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Cleanup()
    {
        foreach (Mouse mouse in mice)
        {
            Destroy(mouse.gameObject);
        }
    }

    public void Remove()
    {
        Cleanup();
        instance = null;
        Destroy(gameObject);
    }
}
