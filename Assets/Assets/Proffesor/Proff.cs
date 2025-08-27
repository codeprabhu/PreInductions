using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
public class MobExplosion : MonoBehaviour
{
    [Header("Detection Radius")]
    public float detectionRadius = 5f;

    [Header("Explosion Settings")]
    public int scorePenalty = 100;
    public float stunDuration = 0.5f;
    public float knockbackForce = 3f;

    private Animator animator;
    private bool isExploding = false;

    private GameObject explosionTarget = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        // Setup collider
        CircleCollider2D cc = GetComponent<CircleCollider2D>();
        cc.isTrigger = true;
        cc.radius = detectionRadius;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isExploding && other.CompareTag("Player"))
        {
            isExploding = true;
            StartExplosionSequence(other.gameObject);
        }
    }

    private void StartExplosionSequence(GameObject player)
    {
        // Trigger explosion animation
        animator.SetTrigger("Explode");
         ScoreManager.Instance.Explosion();

        // Store the player to apply damage when animation event fires
        explosionTarget = player;
    }

    // Called via Animation Event at the "damage frame"
    public void ApplyExplosionEffects()
    {
        if (explosionTarget == null) return;

        // Stun + knockback
        Damageable dmg = explosionTarget.GetComponent<Damageable>();

        dmg.StunRoutine(stunDuration);
    }

    // Destroy mob at the end of animation
    public void FinishExplosion()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(1.375f);
        Destroy(transform.root.gameObject);
    }

    // 🔹 Draw detection radius around mob
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.25f); // transparent red fill
        Gizmos.DrawSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red; // solid red outline
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}


