using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector2 minClamp;
    public Vector2 maxClamp;

    void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = new Vector3(
                Mathf.Clamp(player.position.x, minClamp.x, maxClamp.x),
                Mathf.Clamp(player.position.y, minClamp.y, maxClamp.y),
                transform.position.z
            );

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
