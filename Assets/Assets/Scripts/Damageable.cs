using UnityEngine;
using System.Collections;

public class Damageable : MonoBehaviour
{
    [Header("Damage Settings")]
    public int health = 3;
    public bool isStunned = false;
    public float stunDuration = 2f;
    public float knockbackForce = 5f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(Vector2 hitOrigin)
    {
        // reduce health
        health--;

        // flash effect
        StartCoroutine(HurtFlash(0.2f, 3));

        // stun
        StartCoroutine(StunRoutine(stunDuration));

        // knockback
        ApplyKnockback(hitOrigin);

        // check if dead
        if (health <= 0)
        {
            Die();
        }
    }

    IEnumerator HurtFlash(float flashDuration, int flashCount)
    {
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(flashDuration / (flashCount * 2));
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(flashDuration / (flashCount * 2));
        }
    }

    public IEnumerator StunRoutine(float duration)
    {
        isStunned = true;
        yield return new WaitForSeconds(duration);
        isStunned = false;
    }

    public void ApplyKnockback(Vector2 sourcePosition)
    {
        if (rb != null)
        {
            Vector2 direction = ((Vector2)transform.position - sourcePosition).normalized;
            rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        }
    }

    void Die()
    {
        // disable or destroy
        Destroy(gameObject);
    }
}
