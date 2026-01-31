using UnityEngine;

public class AbilityBlock : MonoBehaviour
{
    public string abilityType; // "Left", "Right", atau "Jump"

    void Start()
    {
        // Cari semua platform yang overlap dengan ability block ini
        DetectPlatformsInWorld();
    }

    void DetectPlatformsInWorld()
    {
        // Dapatkan collider dari ability block
        Collider2D abilityCollider = GetComponent<Collider2D>();
        if (abilityCollider == null)
        {
            Debug.LogWarning("AbilityBlock tidak memiliki Collider2D!");
            return;
        }

        // Cari semua collider yang overlap dengan ability block ini
        Collider2D[] overlappingColliders = new Collider2D[10];
        int count = abilityCollider.Overlap(new ContactFilter2D(), overlappingColliders);

        // Kumpulkan platform yang terdeteksi
        System.Collections.Generic.List<GameObject> detectedPlatforms =
            new System.Collections.Generic.List<GameObject>();

        for (int i = 0; i < count; i++)
        {
            if (overlappingColliders[i].CompareTag("Move Platform"))
            {
                detectedPlatforms.Add(overlappingColliders[i].gameObject);
                Debug.Log($"Ability {abilityType} mendeteksi platform: {overlappingColliders[i].gameObject.name}");
            }
        }

        // Daftarkan platform yang terdeteksi ke AbillityManager
        if (detectedPlatforms.Count > 0 && AbillityManager.instance != null)
        {
            AbillityManager.instance.RegisterAbilityPlatforms(gameObject, detectedPlatforms.ToArray());
        }
        else
        {
            Debug.LogWarning($"Ability {abilityType} tidak mendeteksi platform apapun!");
        }
    }

    // Cara ambil kembali: KLIK KANAN pada balok
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) // 0 = Kiri, 1 = Kanan
        {
            ReturnAbility();
        }
    }

    // Cara aktivasi: KLIK KIRI pada balok
    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) // 0 = Kiri
        {
            ActivateAbility();
        }
    }

    void ReturnAbility()
    {
        // Delegate return logic to AbillityManager so all returns are consistent
        if (AbillityManager.instance != null)
        {
            AbillityManager.instance.ReturnPlacedObject(abilityType);
        }
    }

    void ActivateAbility()
    {
        // Menggerakkan hanya platform yang terdeteksi/terhubung dengan ability ini
        if (AbillityManager.instance != null)
        {
            AbillityManager.instance.MovePlatforms(abilityType, abilityType);
        }
    }
}