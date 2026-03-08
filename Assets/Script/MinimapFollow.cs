using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        // Kiểm tra xem đã kéo nhân vật vào chưa để tránh lỗi
        if (player != null)
        {
            // Lấy vị trí hiện tại của nhân vật
            Vector3 newPosition = player.position;

            // Giữ nguyên độ cao Z của Camera (để camera không bị chìm xuống đất hoặc bay mất)
            // Camera của bạn đang ở Z = -10, dòng này giữ nó ở -10
            newPosition.z = transform.position.z;

            // Gán vị trí mới cho MiniCamera
            transform.position = newPosition;
        }
    }
}
