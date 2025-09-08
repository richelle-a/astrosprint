//importing libraries
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour
{
    public float fallSpeed = 4f; //speed the power up falls
    private float destroyTime = 7f; //time before the power p is destroyed
    private float timer = 0f; //timer to count up to destroyTime

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // disables default gravity
    }

    void Update()
    {
        timer += Time.deltaTime;
        rb.velocity = Vector2.down * fallSpeed; // applies new fall speed

        if (timer >= destroyTime)
        {
            Destroy(gameObject); //destroys the power up after the timer reaches destroyTime
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject); //destroy the power up on player collection
        }
    }
}



