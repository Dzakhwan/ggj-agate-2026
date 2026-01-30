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
        // 1. Kembalikan izin gerak ke Manager
        if (abilityType == "Left") AbillityManager.instance.canMoveLeft = true;
        else if (abilityType == "Right") AbillityManager.instance.canMoveRight = true;
        else if (abilityType == "Jump") AbillityManager.instance.canJump = true;

        // 2. Update UI (Tombol nyala lagi)
        AbillityManager.instance.UpdateUIState();

        // 3. Hancurkan balok ini dari dunia
        Destroy(gameObject);
    }
}