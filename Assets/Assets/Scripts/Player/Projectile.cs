using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    public float spinSpeed = 360f; // degrees per second

    private Vector2 moveDir;
    private float timer;
    private GameObject shooter;

    public void Launch(Vector2 direction, GameObject owner)
    {
        moveDir = direction.normalized;
        shooter = owner; // remember who fired
        timer = 0f;
        gameObject.SetActive(true); // activate if using pooling
    }

    private void Update()
    {
        // Move
        transform.position += (Vector3)(moveDir * speed * Time.deltaTime);

        // Spin
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);

        // Lifetime countdown
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Deactivate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore the shooter
        if (collision.gameObject == shooter)
            return;

        // Try to damage if the hit object has Damageable
        Damageable dmg = collision.GetComponent<Damageable>();
        if (dmg != null)
        {
            dmg.TakeDamage(transform.position); // pass projectile origin
            Deactivate();
            return;
        }

        // Otherwise, deactivate on hit
        Deactivate();
    }

    private void Deactivate()
    {
        shooter = null; // clear reference
        if (ProjectilePool.Instance != null)
            ProjectilePool.Instance.ReturnToPool(gameObject);
        else
            gameObject.SetActive(false);
    }
}
