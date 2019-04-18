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
    public MapManager mapManager;

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

        mapManager = new MapManager(tilemap);

        status = Status.starting;
        waitingTime = SurvivalModeConstants.startingTime;

        mice = new List<Mouse>();
        for (int i = 0; i < SurvivalModeConstants.miceNumber; i++)
        {
            GameObject mouseObject = Instantiate(mousePrefab, mapManager.mapPositionToWorldPosition(SurvivalModeConstants.miceStartingPosition), new Quaternion());
            Mouse mouse = mouseObject.GetComponent<Mouse>();
            mice.Add(mouse);
            mouse.SetTarget(SurvivalModeConstants.miceStartingPosition, SurvivalModeConstants.animationTime, MapManager.Direction.none);
            mouse.FixPositionToTarget();
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
                HurtMice();
                if (HasGameEnded())
                {
                    status = Status.ending;
                    waitingTime = SurvivalModeConstants.endingTime;
                    return;
                }

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

                waitingTime = SurvivalModeConstants.startingTime;
                status = Status.starting;
                return;
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
            PositionDirection nextMove = mapManager.GetNextPosition(mouse.GetMapPosition(), mouse.direction);
            mouse.SetTarget(nextMove.position, SurvivalModeConstants.animationTime, nextMove.direction);
        }
    }

    private void FixMicePositions()
    {
        foreach (Mouse mouse in mice)
        {
            mouse.FixPositionToTarget();
        }
    }

    private void HurtMice()
    {
        foreach (Mouse mouse in mice)
        {
            MapManager.TileType tile = mapManager.GetTileType(mouse.GetMapPosition());
            if (tile == MapManager.TileType.lava)
                mouse.Die();
            else if (tile == MapManager.TileType.spikes)
                mouse.hurt(1);
        }
    }

    private bool HasGameEnded()
    {
        foreach (Mouse mouse in mice)
        {
            if (!mouse.IsDead())
                return false;
        }
        return true;
    }

    private void NextGeneration()
    {
        Mouse best = GetBestMouse();
        foreach (Mouse mouse in mice)
        {
            mouse.Reset(best.dna);
            mouse.SetTarget(SurvivalModeConstants.miceStartingPosition, SurvivalModeConstants.animationTime, MapManager.Direction.none);
            mouse.FixPositionToTarget();
        }
    }

    private Mouse GetBestMouse()
    {
        Mouse best = mice[0];

        foreach (Mouse mouse in mice)
        {
            if (mouse.survivedRound > best.survivedRound)
                best = mouse;
        }
        return best;
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
