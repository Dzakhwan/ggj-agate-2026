using UnityEngine;
using System.Collections;

public class DualMaskSystem : MonoBehaviour
{
    [Header("Topeng Settings")]
    public GameObject maskE; // Drag Topeng E ke sini
    public GameObject maskC; // Drag Topeng C ke sini

    private Coroutine maskECoroutine;
    private bool isMaskCActive = false;

    void Update()
    {
        // LOGIKA TOPENG E (Durasi 5 Detik)
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Jika sedang pakai topeng C, kita lepas dulu supaya tidak tumpang tindih (Opsional)
            if (isMaskCActive) RemoveMaskC(); 

            if (maskECoroutine != null) StopCoroutine(maskECoroutine);
            maskECoroutine = StartCoroutine(ActivateMaskE(5f));
        }

        // LOGIKA TOPENG C (Toggle On/Off)
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Jika sedang pakai topeng E, matikan dulu (Opsional)
            if (maskECoroutine != null) 
            {
                StopCoroutine(maskECoroutine);
                maskE.SetActive(false);
            }

            ToggleMaskC();
        }
    }

    // Coroutine untuk Topeng E
    IEnumerator ActivateMaskE(float duration)
    {
        maskE.SetActive(true);
        yield return new WaitForSeconds(duration);
        maskE.SetActive(false);
    }

    // Fungsi Toggle untuk Topeng C
    void ToggleMaskC()
    {
        isMaskCActive = !isMaskCActive;
        maskC.SetActive(isMaskCActive);
    }

    void RemoveMaskC()
    {
        isMaskCActive = false;
        maskC.SetActive(false);
    }
}