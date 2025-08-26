using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Tooltip("How many points this collectible is worth.")]
    public int value = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Add to global collectible counter
            ScoreManager.Instance.CollectiblePicked(value);

            // Hide or destroy collectible
            gameObject.SetActive(false);
        }
    }
}
