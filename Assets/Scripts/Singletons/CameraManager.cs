using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocationByStatus
{
    public ProgramManager.Status status;
    public Vector3 targetPosition;
    public Vector3 targetRotation;

    public LocationByStatus(ProgramManager.Status status, Vector3 targetPosition, Vector3 targetRotation)
    {
        this.status = status;
        this.targetPosition = targetPosition;
        this.targetRotation = targetRotation;
    }
}

public class CameraManager : MonoBehaviour
{
    public Vector3 targetPosition;
    public Vector3 targetRotation;
    [Range(0, 1)]
    public float bias;

    public List<LocationByStatus> locations;

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

    public void ChangeProgramStatus(ProgramManager.Status status)
    {
        foreach (LocationByStatus location in locations)
        {
            if (location.status == status)
            {
                targetPosition = location.targetPosition;
                targetRotation = location.targetRotation;
                return;
            }
        }
    }
}
