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
    }
}
