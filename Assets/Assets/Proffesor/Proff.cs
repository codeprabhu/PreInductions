using UnityEngine;

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

        // Store the player to apply damage when animation event fires
        explosionTarget = player;
    }

    // This will be called via **Animation Event** at the "damage frame"
    private GameObject explosionTarget = null;
    public void ApplyExplosionEffects()
    {
        if (explosionTarget == null) return;

        // Reduce score
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.ModifyScore(-scorePenalty);

        // Stun and knockback
        Damageable dmg = explosionTarget.GetComponent<Damageable>();
        if (dmg != null)
        {
            dmg.StunRoutine(stunDuration);        // public Stun() wrapper in Damageable
            dmg.ApplyKnockback(transform.position);
        }
    }

    // Optional: destroy mob at the end of the animation
    public void FinishExplosion()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
