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
        // Gửi sự kiện để InventoryManager xử lý việc nhét vào ô túi đồ
        // Không còn phân biệt isMonsterDrop ở đây nữa
        OnItemLooted?.Invoke(item, quantity);

        if (animator != null) animator.Play("LootPickUp");
        Destroy(gameObject, 1f);
    }
    }
}
