using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ScreenFlash : MonoBehaviour
{
    [Header("UI References")]
    public Image flashImage;           // Fullscreen image
    public TextMeshProUGUI flashText;  // Text to remove after flash
    public float flashDuration = 8f;   // Total time flashing lasts
    public float flashSpeed = 2f;      // How fast it fades in/out

    private void Start()
    {
        if (flashImage != null)
        {
            flashImage.gameObject.SetActive(true);
            if (flashText != null)
                flashText.gameObject.SetActive(true);

            StartCoroutine(FlashRoutine());
            Invoke(nameof(StopFlash), flashDuration);
        }
    }

    private IEnumerator FlashRoutine()
    {
        Color c = flashImage.color;

        while (true)
        {
            // Fade in
            while (c.a < 1f)
            {
                c.a += Time.deltaTime * flashSpeed;
                flashImage.color = c;
                yield return null;
            }

            // Fade out
            while (c.a > 0f)
            {
                c.a -= Time.deltaTime * flashSpeed;
                flashImage.color = c;
                yield return null;
            }
        }
    }

    private void StopFlash()
    {
        StopAllCoroutines();

        // Hide image
        if (flashImage != null)
        {
            Color c = flashImage.color;
            c.a = 0f;
            flashImage.color = c;
            flashImage.gameObject.SetActive(false);
        }

        // Hide text
        if (flashText != null)
        {
            flashText.gameObject.SetActive(false);
        }
    }
}
