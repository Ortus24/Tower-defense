using UnityEngine;
using UnityEngine.EventSystems; // Bắt buộc phải có dòng này để dùng UI Events

// Thêm IPointerClickHandler vào sau MonoBehaviour
public class ClickableObject : MonoBehaviour, IPointerClickHandler
{
    public StatUIController statUIController;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("UI Object was clicked!");
        if (statUIController != null)
        {
            statUIController.ToggleStatsUI();
        }
    }
}
