using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ComingSoonController : MonoBehaviour
{
    [Header("Settings")]
    public string mainMenuSceneName = "Main Menu"; // Sesuaikan dengan nama scene menu kamu
    public float delayBeforeSwitch = 3f; // Durasi tulisan muncul (dalam detik)
    public CanvasGroup textCanvasGroup; // Drag CanvasGroup teks ke sini

    void Start()
    {
        // Mulai proses hitung mundur cutscene
        StartCoroutine(ExecuteCutscene());
    }

    IEnumerator ExecuteCutscene()
{
    // Fade In
    while (textCanvasGroup.alpha < 1) {
        textCanvasGroup.alpha += Time.deltaTime;
        yield return null;
    }

    yield return new WaitForSeconds(delayBeforeSwitch);

    // Fade Out
    while (textCanvasGroup.alpha > 0) {
        textCanvasGroup.alpha -= Time.deltaTime;
        yield return null;
    }

    SceneManager.LoadScene(mainMenuSceneName);
}
}