using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpiner : MonoBehaviour
{
    public float rotationAnlgle = 20.0f;
    private Vector3 center;

    void Start()
    {
        center = this.transform.position;
    }

    void Update()
    {
        transform.RotateAround(center, Vector3.forward, rotationAnlgle * Time.deltaTime);
    }
}
