using UnityEngine;

public class DeadlyTrapRectangle : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private Vector2 detectionSize = new Vector2(5f, 2f); // Width & height of the detection box
    [SerializeField] private Vector2 detectionOffset = Vector2.zero; // Offset from trap position
    [SerializeField] private LayerMask playerLayer;

    [Header("Attack Settings")]
    [SerializeField] private Transform attackTarget; // The point where the trap moves when triggered
    [SerializeField] private float attackSpeed = 15f; // Speed at which the trap moves to the target

    private Vector3 initialPosition; // Trap's starting position
    private bool isTriggered = false;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (!isTriggered)
        {
            DetectPlayer();
        }
    }

    void DetectPlayer()
    {
        Collider2D player = Physics2D.OverlapBox((Vector2)transform.position + detectionOffset, detectionSize, 0, playerLayer);
        if (player != null)
        {
            isTriggered = true;
            MoveToAttack();
        }
    }

    void MoveToAttack()
    {
        if (attackTarget != null)
        {
            StartCoroutine(ChargeAtTarget());
        }
    }

    System.Collections.IEnumerator ChargeAtTarget()
    {
        while (Vector3.Distance(transform.position, attackTarget.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, attackTarget.position, attackSpeed * Time.deltaTime);
            yield return null;
        }

        // (Optional) Reset after attack
        yield return new WaitForSeconds(1f);
        transform.position = initialPosition;
        isTriggered = false;
    }

    // Draws the detection rectangle in the Unity Editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 boxCenter = transform.position + (Vector3)detectionOffset;
        Gizmos.DrawWireCube(boxCenter, detectionSize);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            // Implement player death logic here
            Debug.Log("Player hit! Implement damage or respawn logic.");
        }
    }
}
