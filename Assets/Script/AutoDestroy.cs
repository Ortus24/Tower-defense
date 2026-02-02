using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifeTime = 0.9f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

}
