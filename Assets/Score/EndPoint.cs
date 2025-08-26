using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public ScorePanelController scorePanelController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.StopTimer();

            if (scorePanelController != null)
            {
                int collectibles = ScoreManager.Instance.GetCollectibleCount();
                float timeElapsed = ScoreManager.Instance.GetElapsedTime();
                scorePanelController.ShowScorePanel(collectibles, timeElapsed);
            }
        }
    }
}
