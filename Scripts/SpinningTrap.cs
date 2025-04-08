using UnityEngine;

public class AdvancedSpinningTrap : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 100f;

    [Header("Movement Settings")]
    [SerializeField] private bool enableMovement = false; // Checkbox to enable movement
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 targetPosition;

    void Start()
    {
        // Ensure the trap starts at one of the points
        if (enableMovement && pointA != null)
        {
            transform.position = pointA.position;
            targetPosition = pointB.position; // Move towards pointB first
        }
    }

    void Update()
    {
        RotateTrap();

        if (enableMovement)
        {
            MoveBetweenPoints();
        }
    }

    private void RotateTrap()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void MoveBetweenPoints()
    {
        if (pointA == null || pointB == null) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Swap target when reaching a point
        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            targetPosition = (targetPosition == pointA.position) ? pointB.position : pointA.position;
        }
    }
}
