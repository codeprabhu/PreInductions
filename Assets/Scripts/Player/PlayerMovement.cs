using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Four-direction top-down movement using Animator.Play() directly.
/// Plays walk animation when moving, then idle animation in last facing direction when stopped.
/// Directions: 0=Down,1=Left,2=Right,3=Up
/// </summary>
[RequireComponent(typeof(Animator))]
public class TopDownRunnerMovement : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("Transform to move. Defaults to this.transform if left empty.")]
    public Transform player;

    [Tooltip("Movement speed in units/sec for WASD/Arrows.")]
    public float moveSpeed = 10f;

    [Tooltip("Half-size of the allowed movement box for the player (centered at (0,0)).")]
    public Vector2 playAreaExtents = new Vector2(4.5f, 7f);

    private Animator animator;
    private Vector2 moveInput;
    private int lastDirection = 0; // default Down

    // Direction codes
    private const int DIR_DOWN = 0;
    private const int DIR_LEFT = 1;
    private const int DIR_RIGHT = 2;
    private const int DIR_UP = 3;

    // Animation state names in Animator
    private string[] walkStates = { "walk_down", "walk_left", "walk_right", "walk_up" };
    private string[] idleStates = { "idle_down", "idle_left", "idle_right", "idle_up" };

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (!player) player = transform;
        // Start facing down idle
        animator.Play(idleStates[lastDirection]);
    }

    void Update()
    {
        ReadInput();
        MovePlayer();
        UpdateAnimation();
        if (GetComponent<Damageable>().isStunned) return;

    }

    private void ReadInput()
    {
        float x = 0f, y = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) x -= 1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) x += 1f;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) y -= 1f;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) y += 1f;

        moveInput = new Vector2(x, y);
        if (moveInput.sqrMagnitude > 1f) moveInput.Normalize();
    }

    private void MovePlayer()
    {
        Vector3 delta = new Vector3(moveInput.x, moveInput.y, 0f) * moveSpeed * Time.deltaTime;
        player.position += delta;

        // Clamp inside play area (centered at origin)
        var p = player.position;
        p.x = Mathf.Clamp(p.x, -playAreaExtents.x, playAreaExtents.x);
        p.y = Mathf.Clamp(p.y, -playAreaExtents.y, playAreaExtents.y);
        player.position = p;
    }

    private void UpdateAnimation()
    {
        if (moveInput.sqrMagnitude > 0.0001f)
        {
            // Pick axis with larger magnitude for facing direction
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
                lastDirection = moveInput.x > 0f ? DIR_RIGHT : DIR_LEFT;
            else
                lastDirection = moveInput.y > 0f ? DIR_UP : DIR_DOWN;

            animator.Play(walkStates[lastDirection]);
        }
        else
        {
            animator.Play(idleStates[lastDirection]);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Vector3 size = new Vector3(playAreaExtents.x * 2f, playAreaExtents.y * 2f, 0f);
        Gizmos.DrawWireCube(Vector3.zero, size);
    }
}
