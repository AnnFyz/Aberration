using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hovering : MonoBehaviour
{
    [SerializeField] float amplitude = 0.5f;
    [SerializeField] float speed = 0.5f;
    void Update()
    {
        transform.Translate(new Vector3(0, Mathf.Sin(Time.time) * amplitude, 0) * speed * Time.deltaTime);
    }
}
