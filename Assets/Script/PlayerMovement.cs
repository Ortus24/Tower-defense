using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    Vector2 input;
    Vector2 lastMoveDir = Vector2.down;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Speed", input.magnitude);

        if (input != Vector2.zero)
        {
            lastMoveDir = input;
            HandleRotation(input);
        }

        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }


    void FixedUpdate()
    {
        rb.linearVelocity = input.normalized * moveSpeed;
    }


    void HandleRotation(Vector2 dir)
    {
        if (dir.x != 0)
        {
            transform.localScale = new Vector3(
                dir.x > 0 ? 1 : -1,
                1,
                1
            );
        }
    }

}
