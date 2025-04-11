using UnityEngine;

public class projectile : MonoBehaviour
{
    public int damage = 10;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemyHealth enemy = collision.gameObject.GetComponent<enemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        Destroy(gameObject); // optional: tweak this for bouncy/projectile effects
    }
}
