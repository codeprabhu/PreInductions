using UnityEngine;
using System.Collections;

public class PlayerFlash : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public IEnumerator FlashRed(float duration = 0.4f)
    {
        sr.color = Color.red;  // Red glow
        yield return new WaitForSeconds(duration); // Glow duration
        sr.color = originalColor;
    }

    public IEnumerator FlashGreen(float duration = 0.4f)
    {
        sr.color = Color.green;  // Green glow
        yield return new WaitForSeconds(duration); // Glow duration
        sr.color = originalColor;
    }
}
