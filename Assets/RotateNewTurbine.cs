using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateNewTurbine : MonoBehaviour
{
    public GameObject objectToRotateAround;
    public float rotationSpeed;

    void Update()
    {
        Vector3 forward = objectToRotateAround.transform.forward;
        Vector3 up = objectToRotateAround.transform.up;
        Vector3 right = objectToRotateAround.transform.right;
        Vector3 axis = up * 0.1f + forward;
        transform.RotateAround(objectToRotateAround.transform.position, axis, Time.deltaTime * rotationSpeed);
    }
}
