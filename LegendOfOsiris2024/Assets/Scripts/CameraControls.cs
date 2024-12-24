using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public Transform player;
    public Vector3 offset; 
    public float smoothSpeed = 0.125f;
    public float minX, minY, maxX, maxY;
    
    private void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        float clampedX = Mathf.Clamp(smoothPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(smoothPosition.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, smoothPosition.z);
    }
}
