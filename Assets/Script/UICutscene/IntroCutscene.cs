using UnityEngine;
using TMPro; // Untuk TextMeshPro
using UnityEngine.UI; // PENTING: Untuk mengakses komponen Image
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroCutscene : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI textDisplay; // Drag objek TextMeshPro di sini
    public Image introImage; // Drag objek Image background di sini

    [Header("Timing Settings")]
    public float fadeDuration = 2.0f;    // Waktu durasi fade in/out
    public float displayDuration = 3.0f; // Waktu teks diam terbaca
    public float gapBetweenTexts = 0.5f; // Jeda antara teks 1 hilang dan teks 2 muncul
    public string nextSceneName = "GameplayScene";

    void Start()
    {
        // PENTING: Pastikan saat mulai, teks dan gambar transparan (Alpha 0)
        SetAlpha(textDisplay, 0f);
        SetAlpha(introImage, 0f);

        // Mulai urutan intro
        StartCoroutine(PlayIntroSequence());
    }

    IEnumerator PlayIntroSequence()
    {
        // ==================================================================
        // FASE 1: Munculkan GAMBAR dan KALIMAT PERTAMA secara bersamaan
        // ==================================================================
        textDisplay.text = "Will you sacrifice a part of yourself to move forward...";

        // Kita jalankan fade gambar secara paralel (tanpa 'yield return')
        StartCoroutine(FadeImageTo(1f));
        // Kita jalankan fade teks dan TUNGGU sampai selesai (pakai 'yield return')
        yield return StartCoroutine(FadeTextTo(1f));

        // Tunggu waktu baca
        yield return new WaitForSeconds(displayDuration);


        // ==================================================================
        // FASE 2: Hilangkan KALIMAT PERTAMA saja (Gambar tetap terlihat)
        // ==================================================================
        yield return StartCoroutine(FadeTextTo(0f));
        yield return new WaitForSeconds(gapBetweenTexts);


        // ==================================================================
        // FASE 3: Munculkan KALIMAT KEDUA (Gambar masih terlihat)
        // ==================================================================
        textDisplay.text = "...or remain whole yet trapped in the past?";
        yield return StartCoroutine(FadeTextTo(1f));

        // Tunggu waktu baca
        yield return new WaitForSeconds(displayDuration);


        // ==================================================================
        // FASE 4: Hilangkan GAMBAR dan KALIMAT KEDUA secara bersamaan
        // ==================================================================
        StartCoroutine(FadeImageTo(0f)); // Gambar hilang paralel
        yield return StartCoroutine(FadeTextTo(0f)); // Teks hilang ditunggu

        // ==================================================================
        // SELESAI & PINDAH SCENE
        // ==================================================================
        Debug.Log("Intro Selesai. Pindah Scene.");
        // Hapus tanda komentar '//' di bawah ini jika nama scene sudah benar
        SceneManager.LoadScene(nextSceneName);
    }

    // --- FUNGSI-FUNGSI PEMBANTU (HELPER) ---

    // Helper untuk mengatur alpha instan
    void SetAlpha(Graphic graphic, float alpha)
    {
        if (graphic == null) return;
        Color color = graphic.color;
        color.a = alpha;
        graphic.color = color;
    }

    // Coroutine untuk fade TEKS
    IEnumerator FadeTextTo(float targetAlpha)
    {
        Color startColor = textDisplay.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
        yield return StartCoroutine(FadeGraphic(textDisplay, startColor, targetColor));
    }

    // Coroutine untuk fade GAMBAR
    IEnumerator FadeImageTo(float targetAlpha)
    {
        if (introImage == null) yield break; // Safety check
        Color startColor = introImage.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);
        yield return StartCoroutine(FadeGraphic(introImage, startColor, targetColor));
    }

    // Coroutine utama yang menangani logika perubahan warna berjalannya waktu
    IEnumerator FadeGraphic(Graphic graphic, Color startColor, Color targetColor)
    {
        float time = 0;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float normalizedTime = time / fadeDuration;
            // Menggunakan SmoothStep agar efek fade lebih halus (tidak kaku)
            graphic.color = Color.Lerp(startColor, targetColor, Mathf.SmoothStep(0f, 1f, normalizedTime));
            yield return null;
        }
        graphic.color = targetColor; // Pastikan nilai akhir presisi
    }
}