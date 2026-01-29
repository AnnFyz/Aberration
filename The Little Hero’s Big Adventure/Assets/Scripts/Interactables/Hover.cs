using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    public float amplitude = 1;
    public float speed = .5f;

    private void Update()
    {
        Vector3 p = transform.position;
        p.y = amplitude * Mathf.Cos(Time.time * speed);
        transform.position = p;
    }
}
