using UnityEngine;

public class TrapFiring : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;   // Bullet Prefab
    [SerializeField] private Transform firePoint;       // Bullet spawn point
    [SerializeField] private Transform destinationPoint; // Target destination
    [SerializeField] private float fireRate = 1.5f;     // Time between shots
    [SerializeField] private Vector2 detectionSize = new Vector2(5f, 2f); // Rectangular detection size (Width, Height)
    [SerializeField] private LayerMask playerLayer;     // Layer to detect the player
    [SerializeField] private float bulletSpeed = 10f;   // Speed of bullet

    private float fireCooldown;

    private void Update()
    {
        if (fireCooldown > 0)
        {
            fireCooldown -= Time.deltaTime;
        }

        Collider2D player = Physics2D.OverlapBox(transform.position, detectionSize, 0f, playerLayer);
        if (player != null && fireCooldown <= 0)
        {
            FireBullet();
            fireCooldown = fireRate;
        }
    }

    private void FireBullet()
    {
        if (bulletPrefab == null || firePoint == null || destinationPoint == null)
        {
            Debug.LogError("Assign Bullet Prefab, Fire Point, and Destination Point in the Inspector!");
            return;
        }

        // Create bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.gravityScale = 0; // Prevent falling
            Vector2 direction = (destinationPoint.position - firePoint.position).normalized;
            rb.velocity = direction * bulletSpeed;
            Debug.Log("Bullet fired towards destination!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // Draw a wireframe rectangle for the detection area
        Gizmos.DrawWireCube(transform.position, detectionSize);

        if (destinationPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(firePoint.position, destinationPoint.position);
        }
    }
}
