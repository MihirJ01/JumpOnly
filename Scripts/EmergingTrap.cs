using UnityEngine;

public class TrapPlatform : MonoBehaviour
{
    [Header("Spikes Settings")]
    [SerializeField] private Transform spikeTransform; // Spikes GameObject
    [SerializeField] private float moveDistance = 2f;   // Distance to move
    [SerializeField] private float moveSpeed = 5f;   // Speed of movement
    [SerializeField] private float resetSpeed = 5f;   // Speed of resetting position

    [Header("Trigger Settings")]
    [SerializeField] private bool instantAttack = false; // Instantly emerge if detected
    [SerializeField] private float triggerTime = 2f;  // Time before spikes move
    [SerializeField] private float resetTime = 2f;    // Time before spikes reset

    [Header("Detection Settings")]
    [SerializeField] private LayerMask playerLayer;   // Player detection layer
    [SerializeField] private Vector2 baseDetectionSize = new Vector2(1f, 1f); // Base detection size
    [SerializeField] private float offsetXPositive = 0.5f;
    [SerializeField] private float offsetXNegative = 0.5f;
    [SerializeField] private float offsetYPositive = 0.2f;
    [SerializeField] private float offsetYNegative = 0.2f;

    [Header("Movement Directions")]
    [SerializeField] private bool moveUp = false;
    [SerializeField] private bool moveDown = false;
    [SerializeField] private bool moveLeft = false;
    [SerializeField] private bool moveRight = false;

    private float playerStayTime = 0f;
    private bool trapActivated = false;
    private bool isMoving = false;
    private bool isResetting = false;
    private Vector3 originalPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        originalPosition = spikeTransform.position;
    }

    private void Update()
    {
        Vector2 detectionSize = new Vector2(
            baseDetectionSize.x + offsetXPositive + offsetXNegative,
            baseDetectionSize.y + offsetYPositive + offsetYNegative
        );

        Vector2 detectionPosition = new Vector2(
            transform.position.x + (offsetXPositive - offsetXNegative) / 2,
            transform.position.y + (offsetYPositive - offsetYNegative) / 2
        );

        Collider2D player = Physics2D.OverlapBox(detectionPosition, detectionSize, 0, playerLayer);

        if (player != null)
        {
            if (instantAttack && !trapActivated)
            {
                ActivateTrap();
            }
            else
            {
                playerStayTime += Time.deltaTime;
                if (playerStayTime >= triggerTime && !trapActivated)
                {
                    ActivateTrap();
                }
            }
        }
        else
        {
            playerStayTime = 0f;
        }

        if (isMoving)
        {
            spikeTransform.position = Vector3.Lerp(spikeTransform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(spikeTransform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
            }
        }
        else if (isResetting)
        {
            spikeTransform.position = Vector3.Lerp(spikeTransform.position, originalPosition, resetSpeed * Time.deltaTime);
            if (Vector3.Distance(spikeTransform.position, originalPosition) < 0.01f)
            {
                isResetting = false;
                trapActivated = false;
            }
        }
    }

    private void ActivateTrap()
    {
        trapActivated = true;
        isMoving = true;
        isResetting = false;

        targetPosition = originalPosition;
        if (moveUp) targetPosition += Vector3.up * moveDistance;
        if (moveDown) targetPosition += Vector3.down * moveDistance;
        if (moveLeft) targetPosition += Vector3.left * moveDistance;
        if (moveRight) targetPosition += Vector3.right * moveDistance;

        Invoke(nameof(ResetTrap), resetTime);
    }

    private void ResetTrap()
    {
        isResetting = true;
        isMoving = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector2 detectionSize = new Vector2(
            baseDetectionSize.x + offsetXPositive + offsetXNegative,
            baseDetectionSize.y + offsetYPositive + offsetYNegative
        );

        Vector2 detectionPosition = new Vector2(
            transform.position.x + (offsetXPositive - offsetXNegative) / 2,
            transform.position.y + (offsetYPositive - offsetYNegative) / 2
        );

        Gizmos.DrawWireCube(detectionPosition, detectionSize);
    }
}
