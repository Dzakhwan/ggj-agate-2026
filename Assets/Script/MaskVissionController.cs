using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MaskVisionController : MonoBehaviour
{
    [Header("Settings")]
    public Camera mainCamera;
    public string hiddenLayerName = "HiddenWorld";
    public KeyCode toggleKey = KeyCode.E;

    [Header("Durasi & Cooldown")]
    public float usageDuration = 5.0f; // Lama topeng bisa dipakai (detik)
    public float cooldownDuration = 3.0f; // Lama nunggu sampai bisa pakai lagi (detik)

    [Header("Visual Effect")]
    public Image overlayImage; // Drag UI Image untuk overlay effect
    public float fadeInDuration = 0.3f; // Durasi fade saat mask ON
    public float fadeOutDuration = 0.3f; // Durasi fade saat mask OFF
    public Color maskColor = new Color(0.1f, 0.1f, 0.2f, 0.4f); // Warna overlay saat mask aktif

    private int normalMask;
    private int hiddenMask;

    // Status
    private bool isAbilityReady = true; // Apakah skill siap dipakai?

    void Start()
    {
        // --- SETUP CAMERA LAYER (Sama seperti code kamu) ---
        int currentMask = mainCamera.cullingMask;
        int hiddenLayerIndex = LayerMask.NameToLayer(hiddenLayerName);
        int hiddenLayerBit = 1 << hiddenLayerIndex;

        normalMask = currentMask & ~hiddenLayerBit; // Lihat semua KECUALI Hidden
        hiddenMask = currentMask | hiddenLayerBit;  // Lihat semua TERMASUK Hidden

        // Setup overlay image (buat transparent di awal)
        if (overlayImage != null)
        {
            overlayImage.color = Color.clear;
        }

        ApplyVision(false); // Default mati
    }

    void Update()
    {
        // Cek Input DAN Cek apakah Skill Siap
        if (Input.GetKeyDown(toggleKey) && isAbilityReady)
        {
            StartCoroutine(ActivateMaskSequence());
        }
    }

    // Coroutine ini mengatur urutan waktu: Nyala -> Mati -> Tunggu -> Siap lagi
    IEnumerator ActivateMaskSequence()
    {
        // 1. Kunci skill biar gak bisa dispam
        isAbilityReady = false;

        // 2. NYALAKAN TOPENG
        ApplyVision(true);
        Debug.Log($"Mask ON! (Sisa waktu: {usageDuration} detik)");

        // 3. Tunggu selama durasi pemakaian
        yield return new WaitForSeconds(usageDuration);

        // 4. MATIKAN TOPENG OTOMATIS
        ApplyVision(false);
        Debug.Log($"Mask OFF! (Cooldown: {cooldownDuration} detik)");

        // 5. Tunggu selama durasi cooldown (istirahat)
        yield return new WaitForSeconds(cooldownDuration);

        // 6. Skill siap dipakai lagi
        isAbilityReady = true;
        Debug.Log("Mask READY!");
    }

    // Tambahkan di ApplyVision()
    void ApplyVision(bool state)
    {
        if (state)
        {
            mainCamera.cullingMask = hiddenMask;
            StartCoroutine(FadeToColor(maskColor, fadeInDuration)); // Fade in gelap
            Debug.Log("Vision Mask ON - Fading to dark");
        }
        else
        {
            mainCamera.cullingMask = normalMask;
            StartCoroutine(FadeToColor(Color.clear, fadeOutDuration)); // Fade out kembali normal
            Debug.Log("Vision Mask OFF - Fading to normal");
        }
    }

    IEnumerator FadeToColor(Color targetColor, float duration)
    {
        if (overlayImage == null) yield break;

        float elapsed = 0f;
        Color startColor = overlayImage.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            overlayImage.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        overlayImage.color = targetColor;
    }
}