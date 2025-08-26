using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Header("Timer Settings")]
    public float startDelay = 5f;

    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI collectibleText;

    private float startTime;
    private bool timerRunning = false;

    private int collectiblesCount = 0; // Number of collectibles picked

    [HideInInspector]
    private int internalScore = 0; // Hidden score variable

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Invoke(nameof(StartTimer), startDelay);
        UpdateCollectibleUI();
    }

    private void Update()
    {
        if (timerRunning)
        {
            float elapsedTime = Time.time - startTime;
            DisplayTime(elapsedTime);
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

    public void CollectiblePicked(int value = 1)
    {
        collectiblesCount += value;
        internalScore += value; // Increase hidden score as well
        UpdateCollectibleUI();
    }

    private void UpdateCollectibleUI()
    {
        if (collectibleText != null)
            collectibleText.text = $"x{collectiblesCount}";
    }
    // Add this method inside ScoreManager
    public float GetElapsedTime()
    {
        if (!timerRunning)
            return 0f; // or Time.time - startTime if you want final elapsed
        return Time.time - startTime;
    }


    // --- New method to modify internal score ---
    public void ModifyScore(int delta)
    {
        internalScore += delta;
    }

    // Expose internal score safely
    public int GetInternalScore()
    {
        return internalScore;
    }

    public int GetCollectibleCount()
    {
        return collectiblesCount;
    }
}
