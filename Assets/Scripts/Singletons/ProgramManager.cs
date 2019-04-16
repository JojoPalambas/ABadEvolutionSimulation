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
    }

    public void MainMenu()
    {
        status = Status.main_menu;
    }

    public void Survival()
    {
        status = Status.survival;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
