using UnityEngine;
using UnityEngine.UI;

public class PlayerManaUI : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Image manaFill;

    private void OnEnable()
    {
        Debug.Log("PlayerManaUI enabled, subscribing to OnManaChanged event.");
        player.OnManaChanged += UpdateManaBar;
    }

    private void OnDisable()
    {
        Debug.Log("PlayerManaUI disabled, unsubscribing from OnManaChanged event.");
        player.OnManaChanged -= UpdateManaBar;
    }

    private void UpdateManaBar(int current, int max)
    {
        Debug.Log($"Updating mana bar: current Mana = {current}, max Mana = {max}");
        manaFill.fillAmount = (float)current / max;
    }
}
