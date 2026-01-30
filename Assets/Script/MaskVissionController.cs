using UnityEngine;
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

    void ApplyVision(bool state)
    {
        if (state)
        {
            mainCamera.cullingMask = hiddenMask;
            // Tips: Pasang efek suara 'Mask On' disini
        }
        else
        {
            mainCamera.cullingMask = normalMask;
            // Tips: Pasang efek suara 'Mask Off' disini
        }
    }
}