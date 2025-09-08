//importing libraries
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundMover1 : MonoBehaviour
{
    [SerializeField]
    public float speed = 4f; // speed that foreground moves

    [SerializeField]
    public float loopPositionX = 17.7905f; // The x-position to respawn the next foreground

    [SerializeField]
    public float resetPositionX = -53.38f; // The x-position where the foreground will reset

    [SerializeField]
    public float acceleration = 0.1f; // rate at which the speed increases

    void Update()
    {
        // gradual increase in speed
        speed += acceleration * Time.deltaTime;

        // moves the foreground to the left
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // if the foreground moves past the reset position, loop it back
        if (transform.position.x < resetPositionX)
        {
            transform.position = new Vector3(loopPositionX, transform.position.y, transform.position.z);
        }
    }
}

