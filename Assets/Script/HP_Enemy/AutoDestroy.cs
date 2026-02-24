using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float delayAppear = 0.3f;
    public float lifeTime = 0.9f;

    void Start()
    {
        gameObject.SetActive(false);     // Ẩn ngay khi spawn
        Invoke(nameof(Show), delayAppear);
    }

    void Show()
    {
        gameObject.SetActive(true);
        Destroy(gameObject, lifeTime);
    }
}
