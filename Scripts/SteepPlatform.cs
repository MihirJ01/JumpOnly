using UnityEngine;
using System.Collections;

public class TiltingPlatform : MonoBehaviour
{
    [SerializeField] private float delayBeforeTilt = 3f; // Time before tilting
    [SerializeField] private float tiltAngle = 20f; // Tilt angle in degrees
    [SerializeField] private float tiltSpeed = 2f; // Speed of tilting

    [Header("Tilt Direction Settings")]
    [SerializeField] private bool tiltRight = false;
    [SerializeField] private bool tiltLeft = false;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody2D rb;

    private bool isPlayerOnPlatform = false;
    private float timer = 0f;
    private bool isTilting = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true; // Ensures it doesn’t fall or rotate due to physics
    }

    private void Start()
    {
        // Store original position & rotation
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        if (isPlayerOnPlatform)
        {
            timer += Time.deltaTime;
            if (timer >= delayBeforeTilt && !isTilting)
            {
                StartCoroutine(TiltPlatform());
            }
        }
    }

    private IEnumerator TiltPlatform()
    {
        isTilting = true;
        float elapsedTime = 0f;
        float targetTiltAngle = tiltRight ? tiltAngle : (tiltLeft ? -tiltAngle : 0f);
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetTiltAngle);

        while (elapsedTime < 1f)
        {
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime * tiltSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = true;
            timer = 0f; // Reset timer
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = false;
            timer = 0f;
            ResetPlatform();
        }
    }

    private void ResetPlatform()
    {
        transform.position = initialPosition; // Reset position
        transform.rotation = initialRotation; // Reset rotation
        isTilting = false;
    }
}