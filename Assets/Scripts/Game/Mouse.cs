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
}

public class Mouse : MonoBehaviour
{
    public MapManager.Direction direction;
    private Vector2Int mapPosition;
    private Vector2Int mapTarget;
    private Vector3 worldTarget;
    private float timeToTarget;

    public int survivedRound;
    public int hp;
    public DNA dna;

    // Start is called before the first frame update
    void Start()
    {
        hp = 10;
        survivedRound = 0;
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

        this.mapTarget = target;
        Vector3 imprecision = new Vector3(Random.Range(-SurvivalModeManager.instance.mapManager.tilemap.cellSize.x / 4, SurvivalModeManager.instance.mapManager.tilemap.cellSize.x / 4),
                                          Random.Range(-SurvivalModeManager.instance.mapManager.tilemap.cellSize.y / 4, SurvivalModeManager.instance.mapManager.tilemap.cellSize.y / 4), 0);
        this.worldTarget = SurvivalModeManager.instance.mapManager.mapPositionToWorldPosition(target) + imprecision;
        this.timeToTarget = timeToTarget;
        this.direction = direction;

        if (dna == null)
            dna = new DNA(new List<DNAElement>());
        dna.elements.Add(new DNAElement(1, direction));
    }

    public void FixPositionToTarget()
    {
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
        hp = -1;
    }

    public void hurt(int damage)
    {
        hp -= damage;
    }

    public Vector2Int GetMapPosition()
    {
        return mapPosition;
    }

    public void Reset(DNA dna)
    {
        hp = SurvivalModeConstants.mouseHp;
        survivedRound = 0;
        dna = dna.DeepCopy();
    }
}
