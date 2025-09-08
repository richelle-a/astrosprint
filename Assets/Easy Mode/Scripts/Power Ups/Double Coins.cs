//importing libraries
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCoins : MonoBehaviour
{
    public float fallSpeed = 4f; //the speed that the power up falls
    private float destroyTime = 7f; //time before the power up is destroyed
    private float timer = 0f; //timer to count up to destroyTime

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; //changes gravity to 0
        rb.velocity = Vector2.down * fallSpeed; //set custom gravity
    }

    //uses timer to destroy power up after destroy time
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= destroyTime)
        {
            Destroy(gameObject);
        }
    }

    //destroy power up after colliding with player
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}



