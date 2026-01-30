using UnityEngine;
using UnityEngine.UI; // Wajib ada untuk akses Button

public class AbillityManager : MonoBehaviour
{
    public static AbillityManager instance;

    [Header("Status Izin Gerak")]
    public bool canMoveLeft = true;
    public bool canMoveRight = true;
    public bool canJump = true;

    [Header("Referensi UI")]
    // Drag tombol UI kamu ke sini di Inspector
    public Button btnLeft;
    public Button btnRight;
    public Button btnJump;

    void Awake()
    {
        // Singleton pattern sederhana
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    // Fungsi ini dipanggil setiap kali ada perubahan status (taruh/ambil balok)
    public void UpdateUIState()
    {
        // Kalau skill boleh dipakai (true), tombol nyala (interactable)
        // Kalau skill sudah ditaruh di world (false), tombol mati
        btnLeft.interactable = canMoveLeft;
        btnRight.interactable = canMoveRight;
        btnJump.interactable = canJump;

        // Opsional: Bikin agak transparan kalau mati
        SetAlpha(btnLeft, canMoveLeft ? 1f : 0.5f);
        SetAlpha(btnRight, canMoveRight ? 1f : 0.5f);
        SetAlpha(btnJump, canJump ? 1f : 0.5f);
    }

    void SetAlpha(Button btn, float alpha)
    {
        Color color = btn.image.color;
        color.a = alpha;
        btn.image.color = color;
    }
}