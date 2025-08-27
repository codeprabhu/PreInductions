using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ScorePanelController : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform panel;        // The panel to animate
    public TextMeshProUGUI titleText;  // Optional: "You survived!"
    public TextMeshProUGUI collectiblesText;
    public TextMeshProUGUI timeText;

    [Header("Animation Settings")]
    public float slideDuration = 0.5f; // How long it takes to slide in
    public Vector2 hiddenPosition = new Vector2(0, 600f); // offscreen
    public Vector2 visiblePosition = new Vector2(0, 0);   // onscreen

    private void Awake()
    {
        if (panel != null)
        {
            panel.anchoredPosition = hiddenPosition;
            panel.gameObject.SetActive(false);
        }
    }

    public void ShowScorePanel(int collectibles, float timeElapsed)
    {
        if (panel == null) return;

        panel.gameObject.SetActive(true);

        if (collectiblesText != null)
            collectiblesText.text = $"x{collectibles}";

        if (timeText != null)
            timeText.text = $"{timeElapsed:0.00}s";

        StartCoroutine(SlideInPanel());
    }

    private IEnumerator SlideInPanel()
    {
        float elapsed = 0f;
        Vector2 startPos = hiddenPosition;
        Vector2 endPos = visiblePosition;

        while (elapsed < slideDuration)
        {
            panel.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsed / slideDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        panel.anchoredPosition = endPos;
    }
}
