using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public GameObject floatingTextPrefab; // assign in Inspector
    public Transform floatingTextParent;  // usually the Canvas

    private void ShowFloatingText(string message, Color color)
    {
        if (floatingTextPrefab == null || floatingTextParent == null) return;

        GameObject go = Instantiate(floatingTextPrefab, floatingTextParent);
        FloatingText ft = go.GetComponent<FloatingText>();
        ft.Initialize(message, color);
    }

    public static ScoreManager Instance;

    [Header("Timer Settings")]
    public float startDelay = 10f;

    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI collectibleText;
    public TextMeshProUGUI scoreText; // show score if you want
    public GameObject player; // assign this in the Inspector
    private float startTime;
    private bool timerRunning = false;

    private int collectiblesCount = 0;
    private int baseScore = 1200;       // starting score
    private int modificationScore = 0;  // penalties/bonuses
    private int displayedScore = 0;     // what you actually show

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Invoke(nameof(StartTimer), startDelay);
        UpdateCollectibleUI();
        UpdateScoreUI(baseScore);
    }

    private void Update()
    {
        if (timerRunning)
        {
            float elapsedTime = Time.time - startTime;
            DisplayTime(elapsedTime);

            // Apply exponential decay after 60s
            if (elapsedTime > 60f)
            {
                float decayTime = elapsedTime - 60f;
                float k = 0.01f; // controls speed of decay

                int decayedBase = Mathf.RoundToInt(baseScore * Mathf.Exp(-k * decayTime));

                displayedScore = Mathf.Max(0,decayedBase + modificationScore);
                UpdateScoreUI(displayedScore);
            }
            else
            {
                // before 60s just normal scoring
                displayedScore = baseScore + modificationScore;
                UpdateScoreUI(displayedScore);
            }
        }
    }

    private void StartTimer()
    {
        startTime = Time.time;
        timerRunning = true;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    private void DisplayTime(float timeToDisplay)
    {
        int minutes = Mathf.FloorToInt(timeToDisplay / 60);
        int seconds = Mathf.FloorToInt(timeToDisplay % 60);
        int milliseconds = Mathf.FloorToInt((timeToDisplay * 1000) % 1000);

        if (timerText != null)
            timerText.text = $"{minutes:00}:{seconds:00}.{milliseconds:000}";
    }

    // --- Collectibles ---
    public void CollectiblePicked(int value = 1)
    {
        collectiblesCount += value;
        modificationScore += 60 * value; // each collectible = +30
        // Shake the camera
        //StartCoroutine(CameraShake.Instance.Shake(0.2f, 0.2f));

        // Flash the player
        StartCoroutine(player.GetComponent<PlayerFlash>().FlashGreen());

        ShowFloatingText("+30", Color.green);
        UpdateCollectibleUI();
    }

    private void UpdateCollectibleUI()
    {
        if (collectibleText != null)
            collectibleText.text = $"x{collectiblesCount}";
    }

    // --- External events ---
    public void DogBite()
    {
        modificationScore -= 25;
        // Shake the camera
//StartCoroutine(CameraShake.Instance.Shake(0.2f, 0.2f));

// Flash the player
StartCoroutine(player.GetComponent<PlayerFlash>().FlashRed());

        ShowFloatingText("-50", Color.red);
    }

    public void Explosion()
    {
        modificationScore -= 100;
        // Shake the camera
        //StartCoroutine(CameraShake.Instance.Shake(0.2f, 0.2f));

        // Flash the player
        StartCoroutine(player.GetComponent<PlayerFlash>().FlashRed());

        ShowFloatingText("-100", Color.red);
    }

    // --- UI Helpers ---
    private void UpdateScoreUI(int score)
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }

    // --- Public getters ---
    public int GetScore() => displayedScore;
    public int GetCollectibleCount() => collectiblesCount;
    public float GetElapsedTime() => Time.time - startTime;
}
