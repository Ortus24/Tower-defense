using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed ;
    public Rigidbody2D rb;

    [Header("Animation")]
    public Animator animator;

    private Vector2 input;
    private Vector2 lastMoveDir = Vector2.down;

    private bool isAttacking = false;

    void Start()
    {
        moveSpeed = StatesManager.Instance.speed;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        if (!isAttacking)
        {
            animator.SetFloat("Speed", input.magnitude);

            if (input != Vector2.zero)
            {
                lastMoveDir = input;
                HandleRotation(input);
            }
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }

        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            Attack();
        }
    }

    void FixedUpdate()
    {
        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;   // ✅ SỬA Ở ĐÂY
            return;
        }

        rb.linearVelocity = input.normalized * moveSpeed; // ✅ VÀ Ở ĐÂY
    }

    void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
    }

    // Animation Event ở frame cuối Attack
    public void EndAttack()
    {
        isAttacking = false;
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
