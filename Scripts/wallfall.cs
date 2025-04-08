using UnityEngine;
using System.Collections;

public class FallingWallTrap : MonoBehaviour
{
    [Header("Trap Settings")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Vector2 detectionSize = new Vector2(2f, 1f);
    [SerializeField] private Vector2 detectionOffset = Vector2.zero;
    [SerializeField] private float detectionTimeRequired = 2f;
    [SerializeField] private float fallDelay = 0.5f;
    [SerializeField] private float resetTime = 3f; 
    [SerializeField] private float fallRotationSpeed = 100f; // Speed of falling rotation

    private Rigidbody2D rb;
    private Quaternion originalRotation;
    private Vector3 originalPosition;
    private float detectionTime = 0f;
    private bool isFalling = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = false; // Disable physics initially
        rb.freezeRotation = true; // Prevent rotation until triggered
        originalRotation = transform.rotation;
        originalPosition = transform.position;
    }

    private void Update()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        Vector2 detectionPos = (Vector2)transform.position + detectionOffset;
        Collider2D player = Physics2D.OverlapBox(detectionPos, detectionSize, 0, playerLayer);

        if (player != null)
        {
            detectionTime += Time.deltaTime;

            if (detectionTime >= detectionTimeRequired && !isFalling)
            {
                StartCoroutine(FallWall());
            }
        }
        else
        {
            detectionTime = 0f; // Reset timer if player leaves
        }
    }

    private IEnumerator FallWall()
    {
        isFalling = true;
        yield return new WaitForSeconds(fallDelay);

        rb.simulated = true; // Enable physics
        rb.freezeRotation = false; // Allow rotation
        rb.AddTorque(-fallRotationSpeed); // Make the wall fall

        StartCoroutine(ResetWall());
    }

    private IEnumerator ResetWall()
    {
        yield return new WaitForSeconds(resetTime);
        rb.simulated = false; // Disable physics again
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        isFalling = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 detectionPos = (Vector2)transform.position + detectionOffset;
        Gizmos.DrawWireCube(detectionPos, detectionSize);
    }
}
