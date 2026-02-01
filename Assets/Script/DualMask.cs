using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DualMask : MonoBehaviour
{
    [Header("Visual Topeng (Di Player)")]
    public GameObject maskE;
    public GameObject maskC;

    [Header("UI Topeng (Di Canvas)")]
    public Image uiIconE; 
    public Image uiIconC; 

    private Coroutine maskECoroutine;
    private bool isMaskCActive = false; // Logic mandiri untuk Toggle C

    void Start()
    {
        // Set awal: Pastikan semua visual dan UI dalam keadaan non-aktif/transparan
        maskE.SetActive(false);
        maskC.SetActive(false);
        
        SetUIAlpha(uiIconE, 0.3f);
        SetUIAlpha(uiIconC, 0.3f);
    }

    void Update()
    {
        // LOGIKA E: Durasi 5 Detik (Sama seperti sebelumnya)
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (maskECoroutine != null) StopCoroutine(maskECoroutine);
            maskECoroutine = StartCoroutine(ActivateMaskE(5f));
        }

        // LOGIKA C: Toggle On/Off (Mandiri, tidak butuh script lain)
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleMaskC();
        }
    }

    // --- LOGIKA UNTUK TOPENG E ---
    IEnumerator ActivateMaskE(float duration)
    {
        maskE.SetActive(true);
        SetUIAlpha(uiIconE, 1f); 

        yield return new WaitForSeconds(duration);

        maskE.SetActive(false);
        SetUIAlpha(uiIconE, 0.3f); 
    }

    // --- LOGIKA UNTUK TOPENG C ---
    void ToggleMaskC()
    {
        // Membalikkan status (jika true jadi false, jika false jadi true)
        isMaskCActive = !isMaskCActive;

        // Terapkan ke Visual di Player
        maskC.SetActive(isMaskCActive);

        // Terapkan ke UI (Terang jika aktif, Transparan jika mati)
        float alphaC = isMaskCActive ? 1f : 0.3f;
        SetUIAlpha(uiIconC, alphaC);
    }

    // Fungsi pembantu untuk mengubah transparansi UI
    void SetUIAlpha(Image img, float alphaValue)
    {
        if (img != null)
        {
            Color tempColor = img.color;
            tempColor.a = alphaValue;
            img.color = tempColor;
        }
    }
}