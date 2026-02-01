using UnityEngine;
using UnityEngine.SceneManagement; // Wajib untuk pindah scene

public class FinishPortal : MonoBehaviour
{
    // Nama scene yang ingin dituju (bisa diatur di Inspector)
    [SerializeField] private string nextLevelName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Mengecek apakah yang menyentuh adalah Player
        if (collision.CompareTag("Player"))
        {
            MoveToNextLevel();
        }
    }

    void MoveToNextLevel()
    {
        // Memuat scene berdasarkan nama
        SceneManager.LoadScene(nextLevelName);
        
        // ATAU memuat scene berikutnya berdasarkan indeks di Build Settings:
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}