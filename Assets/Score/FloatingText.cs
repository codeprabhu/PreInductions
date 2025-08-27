using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float moveSpeed = 30f;
    public float fadeDuration = 1f;

    private CanvasGroup canvasGroup;
    private RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Initialize(string message, Color color)
    {
        text.text = message;
        text.color = color;
        canvasGroup.alpha = 1f;

        // Destroy after fade
        Destroy(gameObject, fadeDuration);
    }

    void Update()
    {
        // Move upward
        rect.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime;

        // Fade out
        canvasGroup.alpha -= Time.deltaTime / fadeDuration;
    }
}
