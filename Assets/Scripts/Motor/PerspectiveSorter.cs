using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Renderer))]
public class PerspectiveSorter : MonoBehaviour
{

    private const int IsometricRangePerYUnit = 100;

    void Update()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.sortingOrder = -(int) (transform.position.y * IsometricRangePerYUnit);
    }
}