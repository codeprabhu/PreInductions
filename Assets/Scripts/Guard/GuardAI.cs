using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GuardAI : MonoBehaviour
{
    public Transform[] patrolPoints;   // Assign 2+ points in Inspector
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRadius = 3f;
    public float chaseDuration = 3f;   // Time guard continues chasing

    private Transform player;
    private int currentPointIndex = 0;
    private bool chasing = false;
    private float chaseTimer = 0f;

    private LineRenderer lineRenderer;
    public int circleSegments = 64; // smoothness of circle

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Setup LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = circleSegments;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = new Color(1f, 0f, 0f, 0.3f); // Transparent red
        lineRenderer.endColor = new Color(1f, 0f, 0f, 0.3f);

        DrawCircle();
    }

    private void Update()
    {
        if (chasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
            DetectPlayer();
        }
    }

    private void Patrol()
    {
        Transform targetPoint = patrolPoints[currentPointIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }

    private void DetectPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) <= detectionRadius)
        {
            chasing = true;
            chaseTimer = chaseDuration;
        }
    }

    private void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

        chaseTimer -= Time.deltaTime;

        if (chaseTimer <= 0f)
        {
            chasing = false;
        }
    }

    private void DrawCircle()
    {
        Vector3[] points = new Vector3[circleSegments];
        for (int i = 0; i < circleSegments; i++)
        {
            float angle = (float)i / circleSegments * 2 * Mathf.PI;
            float x = Mathf.Cos(angle) * detectionRadius;
            float y = Mathf.Sin(angle) * detectionRadius;
            points[i] = new Vector3(x, y, 0f);
        }
        lineRenderer.SetPositions(points);
    }
}
