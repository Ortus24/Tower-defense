using UnityEngine;

public class SkillCooldownUI : MonoBehaviour
{
    private float cooldownTime;

    private float timer;
    private bool isCooling = false;
    private Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
        Hide();
    }

    void Update()
    {
        if (!isCooling) return;

        timer -= Time.deltaTime;
        float percent = Mathf.Clamp01(timer / cooldownTime);

        // scale Y hạ xuống
        transform.localScale = new Vector3(
            startScale.x,
            startScale.y * percent,
            startScale.z
        );

        if (timer <= 0)
        {
            Hide();
        }
    }

    public void StartCooldown(float duration)
    {
        cooldownTime = duration;
        timer = cooldownTime;
        isCooling = true;
        transform.localScale = startScale;
    }

    void Hide()
    {
        isCooling = false;
        transform.localScale = new Vector3(startScale.x, 0f, startScale.z);
    }
}
