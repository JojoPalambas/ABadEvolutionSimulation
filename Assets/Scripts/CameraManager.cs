using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Vector2 targetPosition;
    public Vector2 targetRotation;
    [Range(0, 1)]
    public float bias;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPosition != null)
        {
            transform.position = transform.position * bias;
        }
    }
}
