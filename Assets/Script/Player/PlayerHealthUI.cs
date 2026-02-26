using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Image healthFill;

    private void OnEnable()
    {
        Debug.Log("PlayerHealthUI enabled, subscribing to OnHPChanged event.");
        player.OnHPChanged += UpdateHealthBar;
    }

    private void OnDisable()
    {
        Debug.Log("PlayerHealthUI disabled, unsubscribing from OnHPChanged event.");
        player.OnHPChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(int current, int max)
    {
        Debug.Log($"Updating health bar: current HP = {current}, max HP = {max}");
        healthFill.fillAmount = (float)current / max;
    }
}