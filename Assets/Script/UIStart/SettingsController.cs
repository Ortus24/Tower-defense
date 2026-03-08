using UnityEngine;

public class SettingsController : MonoBehaviour
{
    [Header("Cấu hình UI")]
    [SerializeField] private GameObject settingsPanel; // Kéo bảng cài đặt của bạn vào đây

    private bool isSettingsOpen = false;

    private void Start()
    {
        // Đảm bảo bảng cài đặt đóng khi bắt đầu game
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    private void Update()
    {
        // Nhấn phím Escape để đóng nhanh nếu bảng đang mở
        if (isSettingsOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettings(false);
        }
    }

    // Hàm này gán vào nút hình bánh răng (Settings Button)
    public void ToggleSettingsFromButton()
    {
        ToggleSettings(!isSettingsOpen);
    }

    private void ToggleSettings(bool open)
    {
        if (settingsPanel == null) return;

        isSettingsOpen = open;
        settingsPanel.SetActive(open);

        // Tùy chọn: Dừng thời gian khi mở cài đặt
        Time.timeScale = open ? 0 : 1;
    }
    public void QuitGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartGameScreen");
    }

}
