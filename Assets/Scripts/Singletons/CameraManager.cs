using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Vector3 targetPosition;
    public Vector3 targetRotation;
    [Range(0, 1)]
    public float bias;
    //public float rotationSpeed;

    public static CameraManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPosition != null)
        {
            transform.position = transform.position * bias + targetPosition * (1 - bias);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), bias);
        }
    }
}
