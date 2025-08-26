using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class TopDownRunnerMovement : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("Transform to move. Defaults to this.transform if left empty.")]
    public Transform player;

    [Tooltip("Movement speed in units/sec for WASD/Arrows.")]
    public float moveSpeed = 10f;

    [Tooltip("Horizontal play area extents (x) and bottom Y limit (y).")]
    public Vector2 playAreaLimits = new Vector2(4.5f, -7f);

    [Header("Gizmo Settings")]
    [Tooltip("Color of the play area lines drawn in Scene view.")]
    public Color gizmoColor = Color.green;

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
        animator.Play(idleStates[lastDirection]);
    }

    void Update()
    {
        ReadInput();
        MovePlayer();
        UpdateAnimation();
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

        var p = player.position;
        // Clamp X between -limit and +limit
        p.x = Mathf.Clamp(p.x, -playAreaLimits.x, playAreaLimits.x);
        // Clamp Y only with a bottom boundary
        if (p.y < playAreaLimits.y) p.y = playAreaLimits.y;

        player.position = p;
    }

    private void UpdateAnimation()
    {
        if (moveInput.sqrMagnitude > 0.0001f)
        {
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

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        float leftX = -playAreaLimits.x;
        float rightX = playAreaLimits.x;
        float bottomY = playAreaLimits.y;

        // Vertical lines for left & right X limits
        Gizmos.DrawLine(new Vector3(leftX, bottomY, 0), new Vector3(leftX, bottomY + 999f, 0));
        Gizmos.DrawLine(new Vector3(rightX, bottomY, 0), new Vector3(rightX, bottomY + 999f, 0));

        // Horizontal line for bottom Y limit
        Gizmos.DrawLine(new Vector3(leftX, bottomY, 0), new Vector3(rightX, bottomY, 0));
    }
}
