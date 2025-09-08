using UnityEngine;

public class CoinPrefab : MonoBehaviour
{
    public float fallSpeed = 4f; //speed that coin falls
    private float destroyTime = 10f; // time before the coin is destroyed
    private float timer = 0f; // timer to count up to destroyTime
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0; //disable default gravity
        rb.velocity = Vector2.down * fallSpeed; //apply custom fall speed to rigidbody
    }

    void Update()
    {
        //increase the timer every frame
        timer += Time.deltaTime;

        //when the timer reaches the destroy time it destroy the coin
        if (timer >= destroyTime)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //destroy the coin if it collides with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

        //stop the coin's movement when it hits the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.velocity = Vector2.zero;
        }
    }
}

