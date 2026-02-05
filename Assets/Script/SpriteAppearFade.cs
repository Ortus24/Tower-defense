using UnityEngine;
using System.Collections;

public class SpriteAppearFade : MonoBehaviour
{
    public float fadeInTime = 0.5f;   // thời gian hiện dần
    public float stayTime = 0.6f;     // đứng yên sau khi hiện xong
    public float fadeOutTime = 0.5f;  // thời gian biến mất dần

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeInOut());
    }

    IEnumerator FadeInOut()
    {
        Color c = sr.color;

        // ===== Fade In =====
        c.a = 0f;
        sr.color = c;

        float t = 0f;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeInTime);
            sr.color = c;
            yield return null;
        }

        // ===== Stay =====
        yield return new WaitForSeconds(stayTime);

        // ===== Fade Out =====
        t = 0f;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, t / fadeOutTime);
            sr.color = c;
            yield return null;
        }

        // ===== Destroy =====
        Destroy(gameObject);
    }
}
