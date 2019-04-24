using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNAElement
{
    public double strength;
    public MapManager.Direction direction;

    public DNAElement(double strength, MapManager.Direction direction)
    {
        this.strength = strength;
        this.direction = direction;
    }

    public DNAElement Copy()
    {
        return new DNAElement(strength, direction);
    }
}

public class DNA
{
    public double totalStrength;
    public List<DNAElement> elements;

    public DNA(List<DNAElement> elements)
    {
        this.elements = elements;

        totalStrength = 0;
        foreach (DNAElement elt in elements)
        {
            totalStrength += elt.strength;
        }
    }

    public DNA DeepCopy()
    {
        List<DNAElement> elementsCopy = new List<DNAElement>();
        foreach (DNAElement elt in elements)
        {
            elementsCopy.Add(elt.Copy());
        }
        return new DNA(elementsCopy);
    }

    public string ToString()
    {
        string ret = "[ ";
        foreach (DNAElement e in elements)
        {
            ret += e.direction.ToString() + " ";
        }
        return ret + "]";
    }
}

public class Mouse : MonoBehaviour
{
    public MapManager.Direction direction;
    private Vector2Int mapPosition;
    private Vector2Int mapTarget;
    private Vector3 worldTarget;
    private float timeToTarget;
    
    public int hp;
    public DNA dna;

    public Animator spriteAnimator;

    // Start is called before the first frame update
    void Start()
    {
        hp = SurvivalModeConstants.mouseHp;
        timeToTarget = -1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToTarget > 0f)
        {
            transform.Translate((worldTarget - transform.position) * (Time.deltaTime / timeToTarget));
            timeToTarget -= Time.deltaTime;
        }
    }

    public void SetTarget(Vector2Int target, float timeToTarget, MapManager.Direction direction)
    {
        if (IsDead())
            return;

        spriteAnimator.Play("Ork2Walk");

        this.mapTarget = target;
        Vector3 imprecision = new Vector3(Random.Range(-SurvivalModeManager.instance.mapManager.tilemap.cellSize.x / 4, SurvivalModeManager.instance.mapManager.tilemap.cellSize.x / 4),
                                          Random.Range(-SurvivalModeManager.instance.mapManager.tilemap.cellSize.y / 4, SurvivalModeManager.instance.mapManager.tilemap.cellSize.y / 4), 0);
        this.worldTarget = SurvivalModeManager.instance.mapManager.mapPositionToWorldPosition(target) + imprecision;
        this.timeToTarget = timeToTarget;
        this.direction = direction;

        if (dna == null)
            dna = new DNA(new List<DNAElement>());
    }

    public void FixPositionToTarget()
    {
        if (!IsDead())
            spriteAnimator.Play("Ork2Wait");

        this.mapPosition = mapTarget;
        this.transform.position = worldTarget;
        this.timeToTarget = -1f;
    }

    public bool IsDead()
    {
        return hp <= 0;
    }

    public void Die()
    {
        if (IsDead())
            return;

        spriteAnimator.Play("Ork2Burn");

        hp = -1;
    }

    public void Hurt(int damage)
    {
        if (IsDead())
            return;

        hp -= damage;

        if (IsDead())
            spriteAnimator.Play("Ork2Poison");
    }

    public Vector2Int GetMapPosition()
    {
        return mapPosition;
    }

    public PositionDirection GetNextPosition()
    {
        int round = SurvivalModeManager.instance.GetRound();
        // If DNA is written for this round
        if (round < dna.elements.Count)
        {
            // If the location given by the DNA is valid
            Debug.Log(SurvivalModeManager.instance.mapManager.IsMoveValid(mapPosition, dna.elements[round].direction));
            if (SurvivalModeManager.instance.mapManager.IsMoveValid(mapPosition, dna.elements[round].direction))
                return new PositionDirection(MapManager.MovePosition(mapPosition, dna.elements[round].direction), dna.elements[round].direction);

            PositionDirection ret = SurvivalModeManager.instance.mapManager.GetNextPosition(mapPosition, direction);
            dna.elements[round] = new DNAElement(1, ret.direction);
            return ret;
        }
        else
        {
            PositionDirection ret = SurvivalModeManager.instance.mapManager.GetNextPosition(mapPosition, direction);
            dna.elements.Add(new DNAElement(1, ret.direction));
            return ret;
        }
    }

    public void Reset(DNA dna, Vector2Int position)
    {
        spriteAnimator.Play("Ork2Wait");

        // Resetting target position
        this.mapTarget = position;
        Vector3 imprecision = new Vector3(Random.Range(-SurvivalModeManager.instance.mapManager.tilemap.cellSize.x / 4, SurvivalModeManager.instance.mapManager.tilemap.cellSize.x / 4),
                                          Random.Range(-SurvivalModeManager.instance.mapManager.tilemap.cellSize.y / 4, SurvivalModeManager.instance.mapManager.tilemap.cellSize.y / 4), 0);
        this.worldTarget = SurvivalModeManager.instance.mapManager.mapPositionToWorldPosition(position) + imprecision;
        
        // Fixing the position
        this.mapPosition = mapTarget;
        this.transform.position = worldTarget;
        this.timeToTarget = -1f;

        hp = SurvivalModeConstants.mouseHp;
        this.dna = dna.DeepCopy();
    }
}
