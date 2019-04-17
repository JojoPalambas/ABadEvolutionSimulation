using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SurvivalModeManager : MonoBehaviour
{
    public enum Status
    {
        starting,                   // The game is waiting before starting
        waitingForDecision,         // The game is waiting before making the mice move
        waitingForAnimations,       // The game is waiting for the moving / dying animations to complete
        ending,                     // All the mice are dead and the game is waiting before regenerating them
    }

    public Tilemap tilemap;

    public GameObject mousePrefab;
    private List<Mouse> mice;

    private Status status;
    private float waitingTime;

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

        status = Status.starting;
        waitingTime = SurvivalModeConstants.startingTime;

        mice = new List<Mouse>();
        for (int i = 0; i < SurvivalModeConstants.miceNumber; i++)
        {
            mice.Add(Instantiate(mousePrefab, SurvivalModeConstants.miceStartingPosition, new Quaternion()).GetComponent<Mouse>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if there is something to do, or if waiting is just fine
        if (waitingTime <= 0f)
        {
            if (status == Status.starting)
            {
                MakeMiceMove();

                waitingTime = SurvivalModeConstants.animationTime;
                status = Status.waitingForAnimations;
                return;
            }
            if (status == Status.waitingForAnimations)
            {
                FixMicePositions();

                waitingTime = SurvivalModeConstants.decisionTime;
                status = Status.waitingForDecision;
                return;
            }
            if (status == Status.waitingForDecision)
            {
                MakeMiceMove();

                waitingTime = SurvivalModeConstants.animationTime;
                status = Status.waitingForAnimations;
                return;
            }
            if (status == Status.ending)
            {
                NextGeneration();
            }
        }
        else
        {
            // Do nothing but reduce waiting time
            waitingTime -= Time.deltaTime;
        }
    }

    private void MakeMiceMove()
    {
        foreach (Mouse mouse in mice)
        {
            mouse.SetTarget(mouse.transform.position + new Vector3(0, 1, 0), SurvivalModeConstants.animationTime);
        }
    }

    private void FixMicePositions()
    {
        foreach (Mouse mouse in mice)
        {
            mouse.FixPositionToTarget();
        }
    }

    private void NextGeneration()
    {
        foreach (Mouse mouse in mice)
        {
            mouse.transform.position = SurvivalModeConstants.miceStartingPosition;
        }
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
