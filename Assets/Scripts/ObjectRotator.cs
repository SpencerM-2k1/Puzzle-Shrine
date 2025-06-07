using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public float rotationSpeed = 100f; // Speed of rotation

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // Left (-1) and Right (+1) arrow keys
        float vertical = Input.GetAxis("Vertical"); // Down (-1) and Up (+1) arrow keys

        // Rotate around Y-axis (left/right) and X-axis (up/down)
        transform.Rotate(Vector3.up, -horizontal * rotationSpeed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.right, vertical * rotationSpeed * Time.deltaTime, Space.World);
    }
}

