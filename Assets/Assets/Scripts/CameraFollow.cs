using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Tooltip("Target the camera should follow.")]
    public Transform target;

    [Tooltip("Smoothness of the follow. Higher = snappier.")]
    public float smoothSpeed = 5f;

    [Header("Camera Limits")]
    [Tooltip("Minimum and maximum X the camera can move.")]
    public Vector2 xLimits = new Vector2(-10f, 10f);

    [Tooltip("Lowest Y position the camera can go.")]
    public float minY = -5f;

    [Tooltip("Optional max Y just for gizmo visualization (not clamped in code).")]
    public float gizmoMaxY = 20f;

    [Header("Gizmo Settings")]
    public Color gizmoColor = Color.cyan;

    private void LateUpdate()
    {
        if (!target) return;

        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

        // Clamp X between limits
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, xLimits.x, xLimits.y);

        // Clamp Y only at bottom (no top limit)
        if (desiredPosition.y < minY)
            desiredPosition.y = minY;

        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        float leftX = xLimits.x;
        float rightX = xLimits.y;
        float bottomY = minY;
        float topY = gizmoMaxY;

        // Left vertical line
        Gizmos.DrawLine(new Vector3(leftX, bottomY, 0), new Vector3(leftX, topY, 0));

        // Right vertical line
        Gizmos.DrawLine(new Vector3(rightX, bottomY, 0), new Vector3(rightX, topY, 0));

        // Bottom horizontal line
        Gizmos.DrawLine(new Vector3(leftX, bottomY, 0), new Vector3(rightX, bottomY, 0));

        // Top horizontal line (visual only)
        Gizmos.DrawLine(new Vector3(leftX, topY, 0), new Vector3(rightX, topY, 0));
    }
}
