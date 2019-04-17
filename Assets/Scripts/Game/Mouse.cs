using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public int hp;

    private Vector3 target;
    private float timeToTarget;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Mouse created");

        timeToTarget = -1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToTarget > 0f)
        {
            transform.Translate((target - transform.position) * (Time.deltaTime / timeToTarget));
            timeToTarget -= Time.deltaTime;
        }
    }

    public void SetTarget(Vector3 target, float timeToTarget)
    {
        this.target = target;
        this.timeToTarget = timeToTarget;
    }

    public void FixPositionToTarget()
    {
        this.transform.position = target;
        this.timeToTarget = -1f;
    }
}
