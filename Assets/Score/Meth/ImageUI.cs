using UnityEngine;
using UnityEngine.UI;

public class UIAnimatedIcon : MonoBehaviour
{
    public Sprite[] frames;
    public float frameRate = 10f;

    private Image img;
    private int currentFrame = 0;
    private float timer = 0f;

    void Awake()
    {
        img = GetComponent<Image>();
        if (frames.Length > 0)
            img.sprite = frames[0];
    }

    void Update()
    {
        if (frames.Length <= 1) return;

        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % frames.Length;
            img.sprite = frames[currentFrame];
        }
    }
}
