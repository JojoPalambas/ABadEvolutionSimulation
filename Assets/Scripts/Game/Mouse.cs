using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public int hp;

    public MapManager.Direction direction;
    private Vector2Int mapPosition;
    private Vector2Int mapTarget;
    private Vector3 worldTarget;
    private float timeToTarget;

    // Start is called before the first frame update
    void Start()
    {
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
        this.mapTarget = target;
        this.worldTarget = SurvivalModeManager.instance.mapManager.mapPositionToWorldPosition(target);
        this.timeToTarget = timeToTarget;
        this.direction = direction;
    }

    public void FixPositionToTarget()
    {
        this.mapPosition = mapTarget;
        this.transform.position = worldTarget;
        this.timeToTarget = -1f;
    }

    public Vector2Int GetMapPosition()
    {
        return mapPosition;
    }
}
