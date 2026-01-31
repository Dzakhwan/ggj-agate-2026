using UnityEngine;

public class trapDamage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Trap")
        {
            Debug.Log("Player Hit Trap");
            Destroy(gameObject);
        }
    }
}
