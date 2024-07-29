using UnityEngine;
using UnityEngine.SceneManagement; // Import SceneManager

public class Login : MonoBehaviour
{
    // Thay đổi cảnh khi bấm nút
    public void ChangeScene()
    {
        // Tên cảnh cần chuyển đến
        SceneManager.LoadScene("Scene2");
    }
}
