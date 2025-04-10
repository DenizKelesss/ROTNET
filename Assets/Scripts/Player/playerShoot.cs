using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Camera playerCamera;  // Reference to the player's camera
    public GameObject projectilePrefab;  // Projectile prefab
    public Transform shootPoint;  // Point where the projectile spawns (e.g., in front of the player)
    public float shootForce = 20f;  // Force applied to the projectile
    public float shootRange = 100f;  // Max range of the shot
    public LayerMask targetLayer;  // Layer mask to specify what the ray can hit (e.g., enemies)
    public float rotationSpeed = 1000f;  // Speed of player rotation towards the click

    private void Update()
    {
        // Detect input for shooting (Left Mouse Button)
        if (Input.GetMouseButtonDown(0)) // 0 is for left mouse click
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Raycast from the center of the camera
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Check if the ray hits something within the shoot range
        if (Physics.Raycast(ray, out hit, shootRange, targetLayer))
        {
            // If we hit something, log the hit
            Debug.Log("Hit " + hit.transform.name);

            // Rotate the player towards the click position
            RotatePlayer(hit.point);

            // Launch the projectile
            LaunchProjectile(hit.point);

            if (hit.transform.CompareTag("Enemy"))
            {
                // Example: Apply damage to the enemy
                hit.transform.GetComponent<enemyHealth>().TakeDamage(10);
            }
        }
        else
        {
            Debug.Log("No hit");
        }
    }

    void RotatePlayer(Vector3 targetPosition)
    {
        // Calculate the direction from the player to the click position
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;  // Ignore vertical rotation (keep it on the same level)

        // Calculate the rotation needed to face the target position
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate the player towards the target direction
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void LaunchProjectile(Vector3 targetPosition)
    {
        // Instantiate the projectile at the shoot point
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

        // Get the Rigidbody component of the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // Calculate the direction of the projectile (towards the target position)
        Vector3 direction = (targetPosition - shootPoint.position).normalized;

        // Apply a force to the projectile to launch it
        rb.AddForce(direction * shootForce, ForceMode.VelocityChange);
    }
}
