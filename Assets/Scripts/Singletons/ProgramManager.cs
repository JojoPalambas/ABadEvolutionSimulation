using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProgramManager : MonoBehaviour
{
    public enum Status
    {
        main_menu,
        survival
    }

    [Header("Survival mode instanciation")]
    public GameObject survivalModeManagerPrefab;
    public Tilemap survivalTileMap;

    public static ProgramManager instance;

    private Status status;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        MainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (status == Status.main_menu)
            {
                Quit();
            }
            else
            {
                MainMenu();
            }
        }
    }

    private void Cleanup()
    {
        if (SurvivalModeManager.instance != null)
        {
            SurvivalModeManager.instance.Remove();
        }
    }

    public void MainMenu()
    {
        Cleanup();

        this.status = Status.main_menu;
        if (CameraManager.instance)
            CameraManager.instance.ChangeProgramStatus(status);
    }

    public void Survival()
    {
        Cleanup();

        this.status = Status.survival;
        if (CameraManager.instance)
            CameraManager.instance.ChangeProgramStatus(status);

        survivalModeManagerPrefab.GetComponent<SurvivalModeManager>().tilemap = survivalTileMap;
        GameObject survivalModeManager = Instantiate(survivalModeManagerPrefab, new Vector3(), new Quaternion());
        /*
        if (survivalModeManager != null)
        {
            survivalModeManager.GetComponent<SurvivalModeManager>().tilemap = survivalTileMap;
        }
        */
    }

    public void Quit()
    {
        Cleanup();

        Application.Quit();
    }
}
