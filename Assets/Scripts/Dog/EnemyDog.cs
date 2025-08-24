using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyDog : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float chaseRadius = 5f;   // red circle radius
    public float attackRadius = 1.0f; // distance for attack
    public float attackCooldown = 1.0f; // time between attacks

    private Animator animator;
    private Vector2 lastDirection = Vector2.down; // default facing down
    private bool isAttacking = false;
    private float nextAttackTime = 0f;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > chaseRadius)
        {
            // Outside detection radius → idle
            PlayIdleAnim();
            return;
        }

        if (distance <= attackRadius)
        {
            // Close enough → attack
            if (Time.time >= nextAttackTime && !isAttacking)
            {
                StartCoroutine(Attack());
            }
        }
        else
        {
            // Within chase radius but not close enough → chase
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        if (isAttacking) return; // Don't chase while attacking

        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);

        lastDirection = GetCardinalDirection(direction);
        PlayRunAnim();
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;

        PlayAttackAnim();

        // Wait for animation length (or fixed short delay)
        yield return new WaitForSeconds(0.5f);

        isAttacking = false;
    }

    private void PlayIdleAnim()
    {
        string anim = "idle_" + DirectionToString(lastDirection);
        animator.Play(anim);
    }

    private void PlayRunAnim()
    {
        string anim = "run_" + DirectionToString(lastDirection);
        animator.Play(anim);
    }

    private void PlayAttackAnim()
    {
        string anim = "attack_" + DirectionToString(lastDirection);
        animator.Play(anim);
    }

    private Vector2 GetCardinalDirection(Vector2 input)
    {
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            return input.x > 0 ? Vector2.right : Vector2.left;
        else
            return input.y > 0 ? Vector2.up : Vector2.down;
    }

    private string DirectionToString(Vector2 dir)
    {
        if (dir == Vector2.up) return "up";
        if (dir == Vector2.down) return "down";
        if (dir == Vector2.left) return "left";
        return "right";
    }

    // Draw red detection circle in editor (moves with dog)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f); // transparent red
        Gizmos.DrawSphere(transform.position, chaseRadius);

        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f); // orange for attack range
        Gizmos.DrawSphere(transform.position, attackRadius);
    }
}
