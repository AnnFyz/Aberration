using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float followSpeed = 2;
    [SerializeField] float yOffset;
    [SerializeField] Transform target;
    [SerializeField] Transform cameraXR;

    private void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, cameraXR.position.y, cameraXR.position.z);
        cameraXR.position = Vector3.Slerp(cameraXR.position, newPos, followSpeed* Time.deltaTime);
    }
}
