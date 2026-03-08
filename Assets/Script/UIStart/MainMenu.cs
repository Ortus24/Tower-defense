using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("MainScrene");
    }

    public void QuitGame()
    {
        // Lệnh này sẽ đóng game hoàn toàn khi chạy trên bản build chính thức
        // Lưu ý: Lệnh này bị bỏ qua khi chạy trong trình biên tập Unity Editor
        Application.Quit();
        Debug.Log("Game đã thoát!"); // Dùng để kiểm tra trong Console vì Editor không tự thoát
    }
}
