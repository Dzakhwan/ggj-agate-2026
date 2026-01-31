using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [Header("Prefabs Balok")]
    public GameObject prefabLeft;  // Drag prefab balok panah kiri
    public GameObject prefabRight; // Drag prefab balok panah kanan
    public GameObject prefabJump;  // Drag prefab balok lompat

    // Variable internal untuk tahu apa yang lagi mau ditaruh
    private GameObject objectToPlace;
    private string selectedAbilityType; // "Left", "Right", atau "Jump"
    private bool isPlacing = false;


    void Update()
    {
        // Jika sedang dalam mode menaruh (setelah klik tombol UI)
        if (isPlacing)
        {
            // 1. Balok mengikuti posisi mouse (Ghost mode)
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // Pastikan Z = 0 karena 2D

            // Update posisi object ghost
            objectToPlace.transform.position = mousePos;

            // 2. Klik Kiri Mouse untuk KONFIRMASI MENARUH
            if (Input.GetMouseButtonDown(0))
            {
                PlaceObject();
            }
        }
    }

    // Fungsi ini dipanggil oleh EVENT ON CLICK di Button UI
    public void SelectAbilityToPlace(string type)
    {
        // Cek dulu, kalau skillnya memang ada (true), baru boleh diambil
        if (type == "Left" && !AbillityManager.instance.canMoveLeft) return;
        if (type == "Right" && !AbillityManager.instance.canMoveRight) return;
        if (type == "Jump" && !AbillityManager.instance.canJump) return;

        selectedAbilityType = type;
        isPlacing = true;

        // Spawn prefabnya sebagai "Ghost" (bayangan)
        GameObject prefabTarget = null;
        if (type == "Left") prefabTarget = prefabLeft;
        else if (type == "Right") prefabTarget = prefabRight;
        else if (type == "Jump") prefabTarget = prefabJump;

        // Buat object sementara yang ngikutin mouse
        objectToPlace = Instantiate(prefabTarget);

        // Matikan collider sementara supaya tidak nabrak player pas lagi digeser-geser
        objectToPlace.GetComponent<Collider2D>().enabled = false;
    }

    void PlaceObject()
    {
        // 1. Nyalakan collider lagi (biar bisa diinjak)
        objectToPlace.GetComponent<Collider2D>().enabled = true;

        // 2. Pasang Script "AbilityBlock" supaya balok ini tahu dia jenis apa
        AbilityBlock blockScript = objectToPlace.AddComponent<AbilityBlock>();
        blockScript.abilityType = selectedAbilityType;

        // 2.1 Register objek yang ditempatkan ke AbillityManager
        if (AbillityManager.instance != null)
        {
            AbillityManager.instance.RegisterPlacedObject(selectedAbilityType, objectToPlace);
        }

        // 3. Matikan Skill di AbilityManager
        if (selectedAbilityType == "Left") AbillityManager.instance.canMoveLeft = false;
        else if (selectedAbilityType == "Right") AbillityManager.instance.canMoveRight = false;
        else if (selectedAbilityType == "Jump") AbillityManager.instance.canJump = false;

        // 4. Update UI (Tombol jadi abu-abu)
        AbillityManager.instance.UpdateUIState();
        // 5. Reset sistem
        isPlacing = false;
        objectToPlace = null;
    }
}