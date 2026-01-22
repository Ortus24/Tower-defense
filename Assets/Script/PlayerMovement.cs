using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    Rigidbody2D rb;
    Animator animator;
    Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Nhận input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Gửi Speed sang Animator
        animator.SetFloat("Speed", movement.magnitude);

        // Lật sprite khi đi trái/phải
        if (movement.x != 0)
            transform.localScale = new Vector3(
                Mathf.Sign(movement.x),
                1,
                1
            );
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
