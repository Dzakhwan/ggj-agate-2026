using UnityEngine;

public class AbilityBlock : MonoBehaviour
{
    public string abilityType; // "Left", "Right", atau "Jump"

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
        // Menggerakkan semua platform sesuai arah ability
        if (AbillityManager.instance != null)
        {
            AbillityManager.instance.MovePlatforms(abilityType);
        }
    }
}