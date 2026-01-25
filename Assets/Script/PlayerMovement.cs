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
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");


        Vector2 moveDir = input.normalized;

        transform.position += (Vector3)(moveDir * moveSpeed * Time.deltaTime);

        animator.SetFloat("Speed", moveDir.magnitude);

        if (moveDir != Vector2.zero)
        {
            lastMoveDir = moveDir;
            HandleRotation(moveDir);
        }
        else
        {
            HandleRotation(lastMoveDir); 
        }
    }

    void HandleRotation(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            transform.localScale = new Vector3(
                dir.x > 0 ? 1 : -1,
                1,
                1
            );
        }
    }
}
