using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNAElement
{
    public int strength;
    public MapManager.Direction direction;

    public DNAElement(int strength, MapManager.Direction direction)
    {
        this.strength = strength;
        this.direction = direction;
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
    public List<DNAElement> dna;

    // Start is called before the first frame update
    void Start()
    {
        timeToTarget = -1f;
        dna = new List<DNAElement>();
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
        if (hp < 0)
            return;

        this.mapTarget = target;
        Vector3 imprecision = new Vector3(Random.Range(-SurvivalModeManager.instance.mapManager.tilemap.cellSize.x / 4, SurvivalModeManager.instance.mapManager.tilemap.cellSize.x / 4),
                                          Random.Range(-SurvivalModeManager.instance.mapManager.tilemap.cellSize.y / 4, SurvivalModeManager.instance.mapManager.tilemap.cellSize.y / 4), 0);
        this.worldTarget = SurvivalModeManager.instance.mapManager.mapPositionToWorldPosition(target) + imprecision;
        this.timeToTarget = timeToTarget;
        this.direction = direction;

        dna.Add(new DNAElement(1, direction));
    }

    public void FixPositionToTarget()
    {
        this.mapPosition = mapTarget;
        this.transform.position = worldTarget;
        this.timeToTarget = -1f;
    }

    public bool IsDead()
    {
        return hp < 0;
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
}
