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

    void ReturnAbility()
    {
        // Delegate return logic to AbillityManager so all returns are consistent
        if (AbillityManager.instance != null)
        {
            AbillityManager.instance.ReturnPlacedObject(abilityType);
        }
    }
}