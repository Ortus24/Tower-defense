using System;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public ItemSO item;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    public int quantity;
    public static event Action<ItemSO, int> OnItemLooted;

    [Header("Cài đặt đặc biệt")]
    public bool isMonsterDrop;

    private void OnValidate()
    {
        if (item == null) return;
        spriteRenderer.sprite = item.itemIcon;
        this.name = item.itemName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Kiểm tra nếu là Đầu lâu thì cộng thẳng vào biến số tổng
            if (isMonsterDrop)
            {
                if (InventoryManager.instance != null)
                {
                    // Gọi hàm bạn vừa thêm để cộng vào biến monsterDropTotal
                    InventoryManager.instance.AddMonsterDrop(quantity);
                }
            }
            else
            {
                // Nếu là đồ thường thì mới gửi sự kiện vào hàm AddItem
                OnItemLooted?.Invoke(item, quantity);
            }

            animator.Play("LootPickUp");
            Destroy(gameObject, 1f);
        }
    }
}
