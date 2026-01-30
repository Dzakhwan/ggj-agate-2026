using UnityEngine;
using System.Collections;

public class MaskVisionController : MonoBehaviour
{
    [Header("Settings")]
    public Camera mainCamera; // Drag Main Camera ke sini
    public string hiddenLayerName = "HiddenWorld";
    public KeyCode toggleKey = KeyCode.E; // Tombol pasang topeng

    private int normalMask;
    private int hiddenMask;
    private bool isMaskOn = false;

    void Start()
    {
        // 1. Ambil settingan kamera saat ini (biasanya melihat Everything)
        int currentMask = mainCamera.cullingMask;

        // 2. Siapkan bitmask untuk Layer HiddenWorld
        int hiddenLayerIndex = LayerMask.NameToLayer(hiddenLayerName);
        int hiddenLayerBit = 1 << hiddenLayerIndex;

        // 3. Setup Logic:
        // Normal Mask = Kamera melihat segalanya KECUALI HiddenWorld
        // (Tanda ~ artinya "inverse/kebalikan")
        normalMask = currentMask & ~hiddenLayerBit;

        // Hidden Mask = Kamera melihat segalanya TERMASUK HiddenWorld
        hiddenMask = currentMask | hiddenLayerBit;

        // Mulai game dengan kondisi topeng lepas (Normal)
        ApplyVision(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            isMaskOn = !isMaskOn;
            ApplyVision(isMaskOn);
        }
    }

    void ApplyVision(bool state)
    {
        if (state)
        {
            // Pakai Topeng: Lihat Hidden Layer
            mainCamera.cullingMask = hiddenMask;
            // Tambahkan efek suara atau post-processing di sini nanti
            Debug.Log("Mask ON: Melihat Dunia Gaib");
            StartCoroutine(CoolDown());
        }
        else
        {
            // Lepas Topeng: Hidden Layer hilang
            mainCamera.cullingMask = normalMask;
            Debug.Log("Mask OFF: Dunia Normal");
        }
    }
    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(5);
        isMaskOn = false;
        ApplyVision(isMaskOn);
    }
}