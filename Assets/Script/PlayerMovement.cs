using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    Animator animator;
    Vector2 input;
    Vector2 lastMoveDir = Vector2.down;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        Vector2 moveDir = input.normalized;

        transform.position += (Vector3)(moveDir * moveSpeed * Time.deltaTime);

        animator.SetFloat("Speed", moveDir.magnitude);

        // ✅ Chỉ cập nhật lastMoveDir khi đang di chuyển
        if (moveDir != Vector2.zero)
        {
            lastMoveDir = moveDir;
            HandleRotation(moveDir); // 🔥 dùng hướng hiện tại
        }
        else
        {
            HandleRotation(lastMoveDir); // idle → quay theo hướng cuối
        }
    }

    void HandleRotation(Vector2 dir)
    {
        // Lật trái / phải
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
