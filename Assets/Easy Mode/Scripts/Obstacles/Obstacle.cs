//importing libraries
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float fallSpeed = 8f; // speed that the obstacle falls
    private float destroyTime = 7f; //time before the obstacle is destroyed
    private float timer = 0f; //timer to count up to destroyTime

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.down * fallSpeed; //sets custom fall speed
    }

    void Update()
    {
        //increase the timer every frame
        timer += Time.deltaTime;

        //if the timer reaches the destroy time it destroys the obstacle
        if (timer >= destroyTime)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //destroy the obstacle if it collides with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}

