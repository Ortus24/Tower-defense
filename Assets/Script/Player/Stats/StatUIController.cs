using UnityEngine;

public class StatUIController : MonoBehaviour
{
    public GameObject StatsUI;

    private void Start()
    {
        StatsUI.SetActive(false);
    }

    // Hàm này cho phép nút bấm phím hoặc chuột đều gọi được
    public void ToggleStatsUI()
    {
        if (StatsUI != null)
        {
            StatsUI.SetActive(!StatsUI.activeSelf);
            Time.timeScale = StatsUI.activeSelf ? 0f : 1f; 
        }
    }

    void Update()
    {
        // Vẫn giữ tính năng ấn nút Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleStatsUI();
        }
    }
}
