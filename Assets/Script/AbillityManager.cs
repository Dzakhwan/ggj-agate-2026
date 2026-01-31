using UnityEngine;
using UnityEngine.UI; // Wajib ada untuk akses Button

public class AbillityManager : MonoBehaviour
{
    public static AbillityManager instance;

    [Header("Status Izin Gerak")]
    public bool canMoveLeft = true;
    public bool canMoveRight = true;
    public bool canJump = true;

    [Header("Platform Movement")]
    public float movementDistance = 2f; // Jarak gerak platform
    public float movementDuration = 0.5f; // Durasi animasi gerak

    [Header("Referensi UI")]
    // Drag tombol UI kamu ke sini di Inspector
    public Button btnLeft;
    public Button btnRight;
    public Button btnJump;

    [Header("Placed Objects (internal)")]
    public GameObject placedLeft;
    public GameObject placedRight;
    public GameObject placedJump;

    // track last placed to allow Shift -> return last placed
    private GameObject lastPlaced;
    private string lastPlacedType;

    void Awake()
    {
        // Singleton pattern sederhana
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        // Tekan Shift untuk mengembalikan semua blok yang ditempatkan
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            ReturnAllPlacedObjects();
        }
    }

    // Dipanggil oleh PlacementSystem saat menaruh objek
    public void RegisterPlacedObject(string type, GameObject obj)
    {
        if (type == "Left") placedLeft = obj;
        else if (type == "Right") placedRight = obj;
        else if (type == "Jump") placedJump = obj;

        lastPlaced = obj;
        lastPlacedType = type;
    }

    // Mengembalikan objek yang sudah ditempatkan (dipanggil dari UI, Shift, atau AbilityBlock)
    public void ReturnPlacedObject(string type)
    {
        GameObject target = null;
        if (type == "Left") target = placedLeft;
        else if (type == "Right") target = placedRight;
        else if (type == "Jump") target = placedJump;

        if (target == null) return;

        Destroy(target);

        if (type == "Left")
        {
            placedLeft = null;
            canMoveLeft = true;
        }
        else if (type == "Right")
        {
            placedRight = null;
            canMoveRight = true;
        }
        else if (type == "Jump")
        {
            placedJump = null;
            canJump = true;
        }

        if (lastPlaced == target)
        {
            lastPlaced = null;
            lastPlacedType = null;
        }

        UpdateUIState();
    }

    // Dipakai oleh UI: kalau ability sudah ditempatkan, tombol akan mengembalikannya
    public void OnAbilityButtonClicked(string type)
    {
        GameObject placed = null;
        if (type == "Left") placed = placedLeft;
        else if (type == "Right") placed = placedRight;
        else if (type == "Jump") placed = placedJump;

        if (placed != null)
        {
            ReturnPlacedObject(type);
            return;
        }

        // kalau belum ditempatkan, mulai mode penempatan lewat PlacementSystem
        PlacementSystem placement = FindFirstObjectByType<PlacementSystem>();
        if (placement != null) placement.SelectAbilityToPlace(type);
    }

    // Kembalikan semua objek yang sedang ditempatkan
    public void ReturnAllPlacedObjects()
    {
        // Panggil ReturnPlacedObject untuk tiap type yang ada
        if (placedLeft != null) ReturnPlacedObject("Left");
        if (placedRight != null) ReturnPlacedObject("Right");
        if (placedJump != null) ReturnPlacedObject("Jump");

        // Reset lastPlaced tracking
        lastPlaced = null;
        lastPlacedType = null;
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

    // Menggerakkan semua platform ke arah tertentu
    public void MovePlatforms(string direction)
    {
        Collider2D[] allColliders = FindObjectsByType<Collider2D>(FindObjectsSortMode.None);

        foreach (Collider2D collider in allColliders)
        {
            // Cek apakah object memiliki tag "Platform"
            if (collider.CompareTag("Move Platform"))
            {
                MovePlatform(collider.gameObject, direction);
            }
        }
    }

    // Menggerakkan platform individual
    public void MovePlatform(GameObject platform, string direction)
    {
        Vector3 targetPosition = platform.transform.position;

        switch (direction)
        {
            case "Left":
                targetPosition.x -= movementDistance;
                break;
            case "Right":
                targetPosition.x += movementDistance;
                break;
            case "Jump":
                targetPosition.y += movementDistance;
                break;
        }

        // Animasi pergerakan (lerp)
        StartCoroutine(MovePlatformCoroutine(platform.transform, targetPosition, movementDuration));
    }

    private System.Collections.IEnumerator MovePlatformCoroutine(Transform platform, Vector3 targetPos, float duration)
    {
        Vector3 startPos = platform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            platform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        platform.position = targetPos;
    }
}