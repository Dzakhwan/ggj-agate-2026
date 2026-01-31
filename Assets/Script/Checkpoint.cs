using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Ambil script Movement dari Player
            Movement player = collision.gameObject.GetComponent<Movement>();

            if (player != null)
            {
                // Update titik respawn ke posisi Checkpoint ini
                player.SetRespawnPoint(transform.position);
            }
        }
    }
}