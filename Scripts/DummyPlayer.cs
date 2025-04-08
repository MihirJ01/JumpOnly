using UnityEngine;

public class DummyPlayer : MonoBehaviour
{
    private Rigidbody2D rb;
    public float bounceForce = 10f; // Adjust to control bounce height

    private bool isGrounded = false; // To check if the player is on the ground

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Bounce only when touching the ground
        if (isGrounded)
        {
            rb.velocity = new Vector2(0, bounceForce);
            isGrounded = false; // Prevent multiple bounces until touching ground again
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Player can bounce again when grounded
        }
    }
}
