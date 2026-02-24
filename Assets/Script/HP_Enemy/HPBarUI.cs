using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : MonoBehaviour
{
    public Image fill;
    private Transform target;
    private EnemyHealth health;
    private Camera cam;

    public void Init(EnemyHealth enemy, Transform followTarget)
    {
        health = enemy;
        target = followTarget;
        cam = Camera.main;

        health.OnHealthPercentChanged += UpdateFill;
        health.OnDead += DestroySelf;

        UpdateFill(health.CurrentHP / health.maxHP);
    }

    void Update()
    {
        if (target)
            transform.position = target.position;
    }

    public void SetPercent(float percent)
    {
        fill.fillAmount = percent;
    }

        void UpdateFill(float percent)
    {
        fill.fillAmount = percent;
    }

    void DestroySelf()
    {
        if (health != null)
        {
            health.OnHealthPercentChanged -= UpdateFill;
            health.OnDead -= DestroySelf;
        }

        Destroy(gameObject);
    }


    void OnDestroy()
    {
        if (health != null)
        {
            health.OnHealthPercentChanged -= UpdateFill;
            health.OnDead -= DestroySelf;
        }
    }

}
