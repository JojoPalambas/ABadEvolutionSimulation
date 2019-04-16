using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgramManager : MonoBehaviour
{
    public enum Status
    {
        main_menu,
        survival
    }

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

    public void MainMenu()
    {
        this.status = Status.main_menu;
        if (CameraManager.instance)
            CameraManager.instance.ChangeProgramStatus(status);
    }

    public void Survival()
    {
        this.status = Status.survival;
        if (CameraManager.instance)
            CameraManager.instance.ChangeProgramStatus(status);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
