using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Throw_Taget : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;

    private Rigidbody2D rb;

    public void Init(Transform target, float speed)
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 targetPoint = (Vector2)target.position + Vector2.down * 0.8f;
        Vector2 dir = (targetPoint - (Vector2)transform.position).normalized;

        Debug.DrawLine(transform.position, target.position, Color.red, 1f);

        rb.linearVelocity = dir * speed;

        Destroy(gameObject, lifeTime);
    }
}