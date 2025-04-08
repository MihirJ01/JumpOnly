using UnityEngine;

public class FallingWallTrigger : MonoBehaviour
{
    [SerializeField] private Animator wallAnimator; // Reference to the Animator
    [SerializeField] private Vector2 detectionSize = new Vector2(3f, 2f); // Width & Height of detection box
    [SerializeField] private LayerMask playerLayer; // Layer for detecting player
    [SerializeField] private float fallThreshold = 3f; // Time before the wall falls
    [SerializeField] private float fallDuration = 3f; // Time before resetting to idle

    [Header("Detection Box Offsets")]
    [SerializeField] private float offsetXPositive = 0f; // Move detection box to +X
    [SerializeField] private float offsetXNegative = 0f; // Move detection box to -X
    [SerializeField] private float offsetYPositive = 0f; // Move detection box to +Y
    [SerializeField] private float offsetYNegative = 0f; // Move detection box to -Y

    private float playerStayTime = 0f;
    private bool hasFallen = false;

    private void Update()
    {
        // Calculate detection box position with offsets
        Vector2 detectionCenter = new Vector2(
            transform.position.x + (offsetXPositive - offsetXNegative),
            transform.position.y + (offsetYPositive - offsetYNegative)
        );

        Collider2D player = Physics2D.OverlapBox(detectionCenter, detectionSize, 0, playerLayer);
        
        if (player != null)
        {
            playerStayTime += Time.deltaTime;

            if (playerStayTime >= fallThreshold && !hasFallen)
            {
                hasFallen = true;
                wallAnimator.SetTrigger("Fall"); // Trigger fall animation
                Invoke(nameof(ResetWall), fallDuration); // Reset after fallDuration
            }
        }
        else
        {
            playerStayTime = 0f; // Reset timer if player leaves
        }
    }

    private void ResetWall()
    {
        hasFallen = false;
        wallAnimator.SetTrigger("Idle"); // Trigger idle animation
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // Calculate detection box position with offsets
        Vector2 detectionCenter = new Vector2(
            transform.position.x + (offsetXPositive - offsetXNegative),
            transform.position.y + (offsetYPositive - offsetYNegative)
        );

        Gizmos.DrawWireCube(detectionCenter, detectionSize); // Draw the detection rectangle
    }
}
