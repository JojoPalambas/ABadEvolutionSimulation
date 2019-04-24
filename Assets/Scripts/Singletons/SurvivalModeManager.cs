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

    private int round;

    public RoundDisplayer maxRoundDisplay;
    public RoundDisplayer roundDisplay;

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
        round = 0;

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
        roundDisplay.SetDisplay(round);
        if (round > maxRoundDisplay.round)
            maxRoundDisplay.SetDisplay(round);

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
                round++;
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
            if (mouse.IsDead())
                continue;

            PositionDirection nextMove = mouse.GetNextPosition();
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
                mouse.Hurt(1);
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
        round = 0;
        Mouse best = GetBestMouse();
        for (int i = 0; i < mice.Count; i++)
        {
            if (i == 0)
                mice[i].Reset(best.dna, SurvivalModeConstants.miceStartingPosition, false);
            else
                mice[i].Reset(best.dna, SurvivalModeConstants.miceStartingPosition, true);
        }
    }

    private Mouse GetBestMouse()
    {
        Mouse best = mice[0];

        foreach (Mouse mouse in mice)
        {
            if (mouse.dna.elements.Count > best.dna.elements.Count)
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
        roundDisplay.SetDisplay(0);
        instance = null;
        Destroy(gameObject);
    }

    public int GetRound()
    {
        return round;
    }
}
