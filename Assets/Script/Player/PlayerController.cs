using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Action<int, int> OnHPChanged;

    [SerializeField] private int maxHp = 100;
    private int currentHp;

    private void Start()
    {
        currentHp = maxHp;
        OnHPChanged?.Invoke(currentHp, maxHp);
    }

    public void TakeDamage(int damage)
    {

        Debug.Log($"Player takes {damage} damage in HP = {currentHp}" );

        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, maxHp);

        OnHPChanged?.Invoke(currentHp, maxHp);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Time.timeScale = 0f; // Đóng băng game
        GameManager.Instance.ShowGameOver();
    }
}