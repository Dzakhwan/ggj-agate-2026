using UnityEngine;

public class trapDamage : MonoBehaviour
{
    private Movement playerMovement;

    void Start()
    {
        // Ambil referensi script Movement yang ada di object Player ini
        playerMovement = GetComponent<Movement>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Cek jika menabrak objek dengan Tag "Trap"
        if (collision.gameObject.CompareTag("Trap"))
        {
            Debug.Log("Player Hit Trap");

            // Panggil fungsi Respawn alih-alih Destroy
            if (playerMovement != null)
            {
                playerMovement.Respawn();
            }
        }
    }
}